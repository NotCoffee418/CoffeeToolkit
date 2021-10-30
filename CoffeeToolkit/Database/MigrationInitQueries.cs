using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeToolkit.Database
{
    /// <summary>
    /// Adjust any or all queries to change the behavior of database migration version handling.
    /// This class can be used to add support for an unsupported database provider.
    /// </summary>
    public class MigrationInitQueries
    {
        /// <summary>
        /// Query to create the table where the currently
        /// installed migration version will be stored
        /// </summary>
        public string FirstInstallQuery { get; set; } = "";

        /// <summary>
        /// Query to check if the database migration system is installed.
        /// Ideally this is done by checking if the migrations table exists.
        /// This query should return 1 or 0, parsable as int.
        /// </summary>
        public string IsMigrationInstalledCheckQuery { get; set; } = "";

        /// <summary>
        /// Query that is run after applying a migration to store 
        /// the latest installed migration version into the database.
        /// Use {0} to indicate the database version.
        /// </summary>
        public string VersionChangeQuery { get; set; } = "";

        /// <summary>
        /// This query returns the currently installed migration version on the database.
        /// </summary>
        public string CheckCurrentMigrationVersionQuery { get; set; } = "";
    }
}
