using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeToolkit.Database;
using CoffeeToolkit.Tests.TestHelpers;
using NUnit.Framework;

namespace CoffeeToolkit.Tests.Database
{
    class DatabaseUpgraderTests
    {
        [Test]
        [TestCase(null)]
        [TestCase("CoffeeToolkit.Tests.TestHelpers")]
        public void GetAllMigrations_ExpectDuplicateException(string namespaceBase)
        {
            DatabaseUpgrader<DbConnection> databaseUpgrader =
                new(MigrationHelpers.GetEmptyDbConnection());
            Assert.Throws<ArgumentException>(() => databaseUpgrader.GetAllMigrations());
        }

        [Test]
        [TestCase("CoffeeToolkit.Tests.TestHelpers.MigratationsOne")]
        [TestCase("CoffeeToolkit.Tests.TestHelpers.MigratationsTwo")]
        public void GetAllMigrations_ExpectCorrespondingQuery(
            string namespaceBase)
        {
            DatabaseUpgrader<DbConnection> databaseUpgrader =
                new(MigrationHelpers.GetEmptyDbConnection());

            List<IMigration> migrations = databaseUpgrader.GetAllMigrations(namespaceBase);
            Assert.AreEqual(
                $"SELECT {migrations[0].DbVersion};", 
                migrations[0].MigrationSql);
            Assert.AreEqual(
                $"SELECT {migrations[1].DbVersion};",
                migrations[1].MigrationSql);
        }
    }
}
