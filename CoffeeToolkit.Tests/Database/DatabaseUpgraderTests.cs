using System;
using System.Collections.Generic;
using System.Data.Common;
using CoffeeToolkit.Database;
using CoffeeToolkit.Tests.TestHelpers;
using Xunit;

namespace CoffeeToolkit.Tests.Database
{
    public class DatabaseUpgraderTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("CoffeeToolkit.Tests.TestHelpers")]
        public void GetAllMigrations_ExpectDuplicateException(string namespaceBase)
        {
            DatabaseUpgrader<DbConnection> databaseUpgrader =
                new(MigrationHelpers.GetEmptyDbConnection());
            Assert.Throws<ArgumentException>(() => databaseUpgrader.GetAllMigrations(namespaceBase));
        }

        [Theory]
        [InlineData("CoffeeToolkit.Tests.TestHelpers.MigratationsOne")]
        [InlineData("CoffeeToolkit.Tests.TestHelpers.MigratationsTwo")]
        public void GetAllMigrations_ExpectCorrespondingQuery(
            string namespaceBase)
        {
            DatabaseUpgrader<DbConnection> databaseUpgrader =
                new(MigrationHelpers.GetEmptyDbConnection());

            List<IMigration> migrations = databaseUpgrader.GetAllMigrations(namespaceBase);
            Assert.Equal(
                $"SELECT {migrations[0].DbVersion};", 
                migrations[0].MigrationSql);
            Assert.Equal(
                $"SELECT {migrations[1].DbVersion};",
                migrations[1].MigrationSql);
        }
    }
}
