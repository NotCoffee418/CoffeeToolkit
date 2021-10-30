using CoffeeToolkit.Database;
using System.Data.SqlClient;

namespace CoffeeToolkit.Tests.TestHelpers
{
    internal static class MigrationHelpers
    {
        public static SqlConnection GetEmptyDbConnection()
            => new SqlConnection();

    }
}

/// <summary>
/// Namespaces needed for assembly tests
/// </summary>
namespace CoffeeToolkit.Tests.TestHelpers.MigratationsOne
{
    internal class v0001_DemoMigrationOne : IMigration
    {
        public int DbVersion => 1;
        public string MigrationSql => "SELECT 1;";
    }

    internal class v0002_DemoMigrationTwo : IMigration
    {
        public int DbVersion => 2;
        public string MigrationSql => "SELECT 2;";
    }
}

/// <summary>
/// Namespaces needed for assembly tests
/// </summary>
namespace CoffeeToolkit.Tests.TestHelpers.MigratationsTwo
{
    internal class v0002_DemoMigrationTwoAgain : IMigration
    {
        public int DbVersion => 2;
        public string MigrationSql => "SELECT 2;";

    }
    internal class v0003_DemoMigrationThree : IMigration
    {
        public int DbVersion => 3;
        public string MigrationSql => "SELECT 3;";
    }
}
