using CoffeeToolkit.Extensions;
using CoffeeToolkit.Time;
using System;
using Xunit;

namespace CoffeeToolkit.Tests.Time
{
    public class UnixTimeTests
    {
        [Theory]
        [InlineData("2020-01-01 0:00:00", 1577836800)]
        [InlineData("2020-01-01 0:00:00.999", 1577836800)] // Expect floor
        [InlineData("1969-01-01 0:00:00", -31536000)] // Expect negative
        public void FromDateTime_TestResults(string dtStr, int expected)
        {
            DateTime input = DateTime.Parse(dtStr);
            int actual = UnixTime.FromDateTime(input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("2040-01-01 0:00:00")] // Too high
        [InlineData("1900-01-01 0:00:00")] //Too low
        public void FromDateTime_OutOfRange_ExpectException(string dtStr)
        {
            DateTime input = DateTime.Parse(dtStr);
            Assert.Throws<ArgumentOutOfRangeException>(() => UnixTime.FromDateTime(input));
        }

        [Theory]
        [InlineData("2020-01-01 0:00:00", 1577836800)]
        [InlineData("1969-01-01 0:00:00", -31536000)] // With negative
        public void ToDateTime_TestResults(string dtStr, int input)
        {
            DateTime expected = DateTime.Parse(dtStr);
            DateTime actual = UnixTime.ToDateTime(input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("2020-01-01 0:00:00", 1577836800)]
        public void ToUnixTime_ExtensionMethod(string dtStr, int expected)
        {
            DateTime input = DateTime.Parse(dtStr);
            int actual = input.ToUnixTime();
            Assert.Equal(expected, actual);
        }
    }
}
