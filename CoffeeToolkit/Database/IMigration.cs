namespace CoffeeToolkit.Database
{
    public interface IMigration
    {
        /// <summary>
        /// Internally defined database version used to determine which upgrades to execute.
        /// </summary>
        public int DbVersion { get; }

        /// <summary>
        /// SQL to be run on database upgrade.
        /// </summary>
        public string MigrationSql { get; }
    }
}
