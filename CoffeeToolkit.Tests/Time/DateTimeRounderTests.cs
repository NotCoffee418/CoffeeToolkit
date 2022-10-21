using CoffeeToolkit.Time;
using System;
using Xunit;

namespace CoffeeToolkit.Tests.Time
{
    public class DateTimeRounderTests
    {
        [Fact]
        public void DateTimeRounderExtensionMethods_ValidResults()
        {
            // Prepare data
            TimeSpan increment = TimeSpan.FromHours(1);
            DateTime actualLow = DateTime.Parse("2020-01-01 0:11:00");
            DateTime actualHigh = DateTime.Parse("2020-01-01 0:51:00");
            DateTime expectLow = DateTime.Parse("2020-01-01 0:00:00");
            DateTime expectHigh = DateTime.Parse("2020-01-01 1:00:00");

            // Run tests
            Assert.Equal(expectLow, actualLow.RoundDown(increment));
            Assert.Equal(expectLow, actualHigh.RoundDown(increment));
            Assert.Equal(expectHigh, actualHigh.RoundUp(increment));
            Assert.Equal(expectHigh, actualLow.RoundUp(increment));
            Assert.Equal(expectLow, actualLow.RoundToNearest(increment));
            Assert.Equal(expectHigh, actualHigh.RoundToNearest(increment));
        }

        [Theory]
        [InlineData("2020-01-01 0:00:00", "2020-01-01 0:00:00")] // RoundDown to same value
        [InlineData("2020-01-01 0:07:00", "2020-01-01 0:00:00")] // RoundDown to lower value
        [InlineData("2020-01-01 0:57:00", "2020-01-01 0:00:00")] // RoundDown to lower value from high
        public void RoundDown_TestResults(string inputDtStr, string expectedDtStr)
        {
            DateTime input = DateTime.Parse(inputDtStr);
            DateTime actual = DateTimeRounder.RoundDown(input, TimeSpan.FromHours(1));
            Assert.Equal(DateTime.Parse(expectedDtStr), actual);
        }

        [Theory]
        [InlineData("2020-01-01 0:00:00", "2020-01-01 0:00:00")] // RoundUp to same value
        [InlineData("2020-01-01 0:07:00", "2020-01-01 1:00:00")] // RoundUp to higher value from low
        [InlineData("2020-01-01 0:57:00", "2020-01-01 1:00:00")] // RoundDown to higher value
        public void RoundUp_TestResults(string inputDtStr, string expectedDtStr)
        {
            DateTime input = DateTime.Parse(inputDtStr);
            DateTime actual = DateTimeRounder.RoundUp(input, TimeSpan.FromHours(1));
            Assert.Equal(DateTime.Parse(expectedDtStr), actual);
        }

        [Theory]
        [InlineData("2020-01-01 0:51:00", "2020-01-01 1:00:00")] // Expect RoundUp from high
        [InlineData("2020-01-01 0:11:00", "2020-01-01 0:00:00")] // Expect RoundDown from low
        public void RoundToNearest_TestResults(string inputDtStr, string expectedDtStr)
        {
            DateTime input = DateTime.Parse(inputDtStr);
            DateTime actual = DateTimeRounder.RoundToNearest(input, TimeSpan.FromHours(1));
            Assert.Equal(DateTime.Parse(expectedDtStr), actual);
        }

        [Theory]
        [InlineData("2020-01-31 23:59:59.999", "2020-01-01 0:00:00")]
        [InlineData("2020-01-15", "2020-01-01 0:00:00")]
        public void GetStartOfMonth_TestResults_UsingExtensionMethod(string inputDtStr, string expectedDtStr)
        {
            DateTime actual = DateTime.Parse(inputDtStr).StartOfMonth();
            DateTime expected = DateTime.Parse(expectedDtStr);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("2020-01-01 0:00:00", "2020-01-31 23:59:59.999")]
        [InlineData("2020-01-15", "2020-01-31 23:59:59.999")]
        [InlineData("2022-02-15", "2022-02-29 23:59:59.999")]
        [InlineData("2021-02-15", "2022-02-28 23:59:59.999")]
        public void GetEndOfMonth_TestResults_UsingExtensionMethod(string inputDtStr, string expectedDtStr)
        {
            DateTime actual = DateTime.Parse(inputDtStr).EndOfMonth();
            DateTime expected = DateTime.Parse(expectedDtStr);
            Assert.Equal(expected, actual);
        }
    }
}
