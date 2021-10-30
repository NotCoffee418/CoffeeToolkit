using System.Data.Common;
using CoffeeToolkit.Database;

namespace CoffeeToolkit.Data.Database
{
    internal static class UpgraderQueries
    {
        internal static MigrationInitQueries GetFirstInstallQuery<T>()
            where T : DbConnection
        {
            switch (typeof(T).FullName)
            {
                // PostgreSQL
                case "Npgsql.NpgsqlConnection":
                    return new()
                    {
                        FirstInstallQuery = @"BEGIN;
                            CREATE TABLE dbmigration (dbversion integer);
                            INSERT INTO dbmigration (dbversion) VALUES (0);
                            COMMIT;",
                        IsMigrationInstalledCheckQuery = @"SELECT EXISTS(
                            SELECT * FROM information_schema.tables 
                            WHERE table_name = 'dbmigration');",
                        VersionChangeQuery =
                            @"UPDATE dbmigration SET dbversion = {0};",
                        CheckCurrentMigrationVersionQuery =
                            @"SELECT dbversion FROM dbmigration",
                    };

                // MySQL / MariaDB
                case "MySql.Data.MySqlClient.MySqlConnection":
                case "MySqlConnector.MySqlConnection":
                    return new()
                    {
                        FirstInstallQuery = @"START TRANSACTION;
                            CREATE TABLE dbmigration (dbversion integer);
                            INSERT INTO dbmigration (dbversion) VALUES (0);
                            COMMIT;",
                        IsMigrationInstalledCheckQuery = @"
                            SELECT count(*) FROM information_schema.TABLES
                            WHERE TABLE_NAME = 'dbmigration' AND TABLE_SCHEMA in (SELECT DATABASE());",
                        VersionChangeQuery =
                            @"UPDATE dbmigration SET dbversion = {0};",
                        CheckCurrentMigrationVersionQuery =
                            @"SELECT dbversion FROM dbmigration",
                    };

                // SQLite
                case "Microsoft.Data.Sqlite.SqliteConnection":
                case "System.Data.SQLite.SQLiteConnection":
                    return new()
                    {
                        FirstInstallQuery = @"BEGIN TRANSACTION;
                            CREATE TABLE dbmigration (dbversion integer);
                            INSERT INTO dbmigration (dbversion) VALUES (0);
                            COMMIT;",
                        IsMigrationInstalledCheckQuery = 
                            @"SELECT count(*) FROM sqlite_master WHERE type='table' AND name='dbmigration';",
                        VersionChangeQuery =
                            @"UPDATE dbmigration SET dbversion = {0};",
                        CheckCurrentMigrationVersionQuery =
                            @"SELECT dbversion FROM dbmigration",
                    };
                
                // MSSQL
                case "System.Data.SqlClient.SqlConnection":
                case "Microsoft.Data.SqlClient.SqlConnection":
                    return new()
                    {
                        FirstInstallQuery = @"BEGIN TRANSACTION;
                            CREATE TABLE dbmigration (dbversion integer);
                            INSERT INTO dbmigration (dbversion) VALUES (0);
                            COMMIT;",
                        IsMigrationInstalledCheckQuery = 
                            @"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'dbmigration'",
                        VersionChangeQuery =
                            @"UPDATE dbmigration SET dbversion = {0};",
                        CheckCurrentMigrationVersionQuery =
                            @"SELECT dbversion FROM dbmigration",
                    };

                // Unsupported, manually define queries
                default: 
                    return null;
            }
        }
    }
}