using CoffeeToolkit.Data.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using System.Linq;
using System.Reflection;

namespace CoffeeToolkit.Database
{
    public class DatabaseUpgrader<T>
        where T : DbConnection
    {
        /// <summary>
        /// Installs the database and/or upgrades it with the latest IMigrations
        /// </summary>
        /// <param name="dbConnection">Database connection</param>
        public DatabaseUpgrader(T dbConnection)
        {
            _dbConnection = dbConnection;
            _customMigrationInitQueries = null;
        }

        /// <summary>
        /// Installs the database and/or upgrades it with the latest IMigrations
        /// </summary>
        /// <param name="dbConnection">Database connection</param>
        /// <param name="customMigrationInitQueries">Optionally override version tracking queries.</param>
        public DatabaseUpgrader(
            T dbConnection,
            MigrationInitQueries customMigrationInitQueries)
        {
            _dbConnection = dbConnection;
            _customMigrationInitQueries = customMigrationInitQueries;
        }

        private T _dbConnection;
        private MigrationInitQueries _customMigrationInitQueries;


        /// <summary>
        /// Check migrations and upgrade the database if needed
        /// </summary>
        /// <param name="migrationNamespace">Optionally restrict the IMigration classes to the ones inside the provided namespace</param>
        /// <returns></returns>
        public async Task UpgradeAsyc(string migrationNamespace = null)
        {
            try
            {
                // null check
                if (_dbConnection == null)
                    throw new ArgumentException(
                        "Database upgrader failed. Provided DbConnection was null");

                // Prepare queries            
                string versionChangeQuery = string.Empty;
                string isMigrationInstalledCheckQuery = string.Empty;
                string firstInstallQuery = string.Empty;
                string checkCurrentMigrationVersionQuery = string.Empty;

                // Attempt to get default queries for any supported database engine
                var defaultQueries = UpgraderQueries.GetFirstInstallQuery<T>();
                if (defaultQueries != null)
                {
                    versionChangeQuery = defaultQueries.VersionChangeQuery;
                    isMigrationInstalledCheckQuery = defaultQueries.IsMigrationInstalledCheckQuery;
                    firstInstallQuery = defaultQueries.FirstInstallQuery;
                    checkCurrentMigrationVersionQuery = defaultQueries.CheckCurrentMigrationVersionQuery;
                }

                // Get override queries if any
                if (_customMigrationInitQueries != null)
                {
                    versionChangeQuery = _customMigrationInitQueries.VersionChangeQuery;
                    isMigrationInstalledCheckQuery = _customMigrationInitQueries.IsMigrationInstalledCheckQuery;
                    firstInstallQuery = _customMigrationInitQueries.FirstInstallQuery;
                    checkCurrentMigrationVersionQuery = _customMigrationInitQueries.CheckCurrentMigrationVersionQuery;
                }

                // Validate queries
                if (string.IsNullOrEmpty(versionChangeQuery) ||
                    string.IsNullOrEmpty(isMigrationInstalledCheckQuery) ||
                    string.IsNullOrEmpty(firstInstallQuery))
                    throw new ArgumentException("Database upgrade failed. " +
                        "One or more required migration queries could not be resolved. " +
                        "Please ensure that you use a supported database or manually provide replacement queries.");

                // Open and test connection
                try
                {
                    // Open connection if needed
                    if (_dbConnection.State != ConnectionState.Open)
                        _dbConnection.Open();
                    await ExecuteNonQueryAsync(_dbConnection, "SELECT 1;");
                }
                catch (Exception ex)
                {
                    throw new Exception(
                        "There was an issue connecting to the database while upgrading", ex);
                }

                // Check if migration system is installed, install it if needed
                long foundTables = await ExecuteScalarAsync<long>(
                    _dbConnection, isMigrationInstalledCheckQuery);
                if (foundTables == 0)
                    await ExecuteNonQueryAsync(_dbConnection, firstInstallQuery);

                // Get the currently installed migration verion
                int dbVersion = await ExecuteScalarAsync<int>(
                    _dbConnection, checkCurrentMigrationVersionQuery);

                // Get all migrations
                List<IMigration> migrations = GetAllMigrations(migrationNamespace);
                int applicationVersion = migrations
                    .Select(x => x.DbVersion)
                    .LastOrDefault();

                // Return if we're up-to-date
                if (dbVersion >= applicationVersion)
                    return;

                // Run migrations which still need to be run one by one
                // and update dbversion
                var migrationsToRun = migrations
                    .Where(x => x.DbVersion > dbVersion);
                foreach (var migration in migrationsToRun)
                {
                    try
                    {
                        // Run query
                        await ExecuteNonQueryAsync(_dbConnection, migration.MigrationSql);

                        // Update version
                        await ExecuteNonQueryAsync(_dbConnection,
                            string.Format(versionChangeQuery, migration.DbVersion));
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(
                            $"An error has occured while applying migration #{migration.DbVersion}.", ex);
                    }
                }               
            }
            catch (Exception ex)
            {
                throw new Exception("There was a problem while upgrading the database.", ex);
            }
            finally
            {
                // Close connection
                if (_dbConnection != null && _dbConnection.State == ConnectionState.Open)
                    _dbConnection.Close();
            }
        }

        public List<IMigration> GetAllMigrations(string migrationNamespace = null)
        {
            // Get all IMigrations
            List<IMigration> result = new();
            AppDomain.CurrentDomain.GetAssemblies()
                // is assignable by us
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(IMigration).IsAssignableFrom(p))
                // No abtract to avoid including the interface itself
                .Where(p => !p.Attributes.HasFlag(TypeAttributes.Abstract))
                // Either undefined namespace or starts with the exact specified namespace
                .Where(p => migrationNamespace == null || p.Namespace.StartsWith(migrationNamespace))
                .ToList()
                .ForEach(x => result.Add((IMigration)Activator.CreateInstance(x)));

            // Check for IMigrations with duplicate DbVersion
            // Only one migration class per DbVersion is allowed
            var duplicateVersions = result
                .Select(x => x.DbVersion)
                .GroupBy(s => s)
                .SelectMany(grp => grp.Skip(1))
                .Distinct();
            if (duplicateVersions.Count() > 0)
                throw new ArgumentException("Database upgrade failed. " +
                    "Found multiple IMigration with the same database versions: " +
                    string.Join(", ", duplicateVersions));

            // Sort and return
            return result
                .OrderBy(x => x.DbVersion)
                .ToList();
        }

        #region Database interaction methods
        private static async Task<U> ExecuteScalarAsync<U>(
            DbConnection connection, string queryString)
        {
            // Create the command.
            DbCommand command = connection.CreateCommand();
            command.CommandText = queryString;
            command.CommandType = CommandType.Text;

            // Retrieve the data.
            return (U)(await command.ExecuteScalarAsync());
        }

        private static async Task ExecuteNonQueryAsync(
            DbConnection connection, string queryString)
        {
            // Create the command.
            DbCommand command = connection.CreateCommand();
            command.CommandText = queryString;
            command.CommandType = CommandType.Text;

            // Execute
            await command.ExecuteNonQueryAsync();
        }
        #endregion
    }
}
