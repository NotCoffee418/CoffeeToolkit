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
    class UnixTimeTests
    {
        [Test]
        [TestCase("2020-01-01 0:00:00", 1577836800)]
        [TestCase("2020-01-01 0:00:00.999", 1577836800, Description = "Expect floor")]
        [TestCase("1969-01-01 0:00:00", -31536000, Description = "Expect negative")]
        public void FromDateTime_TestResults(string dtStr, int expected)
        {
            DateTime input = DateTime.Parse(dtStr);
            int actual = UnixTime.FromDateTime(input);
            Assert.AreEqual(expected, actual);
        }

        [Test(Description = "Expect OutOfRangeException for compatability")]
        [TestCase("2040-01-01 0:00:00", Description = "Too high")]
        [TestCase("1900-01-01 0:00:00", Description = "Too low")]
        public void FromDateTime_OutOfRange_ExpectException(string dtStr)
        {
            DateTime input = DateTime.Parse(dtStr);
            try
            {
                int result = UnixTime.FromDateTime(input);
                Assert.Fail("Result type should be int to maintain compatability. Create another method.");
            }
            catch (ArgumentOutOfRangeException) { Assert.Pass(); };
        }

        [Test]
        [TestCase("2020-01-01 0:00:00", 1577836800)]
        [TestCase("1969-01-01 0:00:00", -31536000, Description = "With negative")]
        public void ToDateTime_TestResults(string dtStr, int input)
        {
            DateTime expected = DateTime.Parse(dtStr);
            DateTime actual = UnixTime.ToDateTime(input);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("2020-01-01 0:00:00", 1577836800)]
        public void ToUnixTime_ExtensionMethod(string dtStr, int expected)
        {
            DateTime input = DateTime.Parse(dtStr);
            int actual = input.ToUnixTime();
            Assert.AreEqual(expected, actual);
        }
    }
}
