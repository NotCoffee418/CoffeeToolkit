using CoffeeToolkit.Extensions;
using CoffeeToolkit.Time;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeToolkit.Tests.Time
{
    class DateTimeRounderTests
    {
        [Test]
        public void DateTimeRounderExtensionMethods_ValidResults()
        {
            // Prepare data
            TimeSpan increment = TimeSpan.FromHours(1);
            DateTime actualLow = DateTime.Parse("2020-01-01 0:11:00");
            DateTime actualHigh = DateTime.Parse("2020-01-01 0:51:00");
            DateTime expectLow = DateTime.Parse("2020-01-01 0:00:00");
            DateTime expectHigh = DateTime.Parse("2020-01-01 1:00:00");

            // Run tests
            Assert.AreEqual(expectLow, actualLow.RoundDown(increment));
            Assert.AreEqual(expectLow, actualHigh.RoundDown(increment));
            Assert.AreEqual(expectHigh, actualHigh.RoundUp(increment));
            Assert.AreEqual(expectHigh, actualLow.RoundUp(increment));
            Assert.AreEqual(expectLow, actualLow.RoundToNearest(increment));
            Assert.AreEqual(expectHigh, actualHigh.RoundToNearest(increment));
        }

        [Test]
        [TestCase("2020-01-01 0:00:00", "2020-01-01 0:00:00", Description = "RoundDown to same value")]
        [TestCase("2020-01-01 0:07:00", "2020-01-01 0:00:00", Description = "RoundDown to lower value")]
        [TestCase("2020-01-01 0:57:00", "2020-01-01 0:00:00", Description = "RoundDown to lower value from high")]
        public void RoundDown_TestResults(string inputDtStr, string expectedDtStr)
        {
            DateTime input = DateTime.Parse(inputDtStr);
            DateTime actual = DateTimeRounder.RoundDown(input, TimeSpan.FromHours(1));
            Assert.AreEqual(DateTime.Parse(expectedDtStr), actual);
        }

        [Test]
        [TestCase("2020-01-01 0:00:00", "2020-01-01 0:00:00", Description = "RoundUp to same value")]
        [TestCase("2020-01-01 0:07:00", "2020-01-01 1:00:00", Description = "RoundUp to higher value from low")]
        [TestCase("2020-01-01 0:57:00", "2020-01-01 1:00:00", Description = "RoundDown to higher value")]
        public void RoundUp_TestResults(string inputDtStr, string expectedDtStr)
        {
            DateTime input = DateTime.Parse(inputDtStr);
            DateTime actual = DateTimeRounder.RoundUp(input, TimeSpan.FromHours(1));
            Assert.AreEqual(DateTime.Parse(expectedDtStr), actual);
        }

        [Test]
        [TestCase("2020-01-01 0:51:00", "2020-01-01 1:00:00", Description = "Expect RoundUp from high")]
        [TestCase("2020-01-01 0:11:00", "2020-01-01 0:00:00", Description = "Expect RoundDown from low")]
        public void RoundToNearest_TestResults(string inputDtStr, string expectedDtStr)
        {
            DateTime input = DateTime.Parse(inputDtStr);
            DateTime actual = DateTimeRounder.RoundToNearest(input, TimeSpan.FromHours(1));
            Assert.AreEqual(DateTime.Parse(expectedDtStr), actual);
        }
    }
}
