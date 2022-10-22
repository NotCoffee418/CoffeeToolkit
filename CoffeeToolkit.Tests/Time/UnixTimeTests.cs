namespace CoffeeToolkit.Tests.Time;

public class UnixTimeTests
{
    [Theory]
    [InlineData("2020-01-01 0:00:00", 1577836800)]
    [InlineData("2020-01-01 0:00:00.999", 1577836800)] // Expect floor
    [InlineData("1969-01-01 0:00:00", -31536000)] // Expect negative
    public void FromDateTimeToSeconds_WithLocalDateTimeOverride_ExpectUtcTimestamp(string dtStr, int expected)
    {
        DateTime input = DateTime.Parse(dtStr);
        long actual = UnixTime.FromDateTimeToSeconds(input, true);
        Assert.Equal(expected, actual);
    }
    
    [Theory]
    [InlineData("2020-01-01 0:00:00", 1577836800)]
    [InlineData("1969-01-01 0:00:00", -31536000)] // With negative
    public void ToDateTime_TestResults(string dtStr, int input)
    {
        DateTime expected = DateTime.Parse(dtStr);
        DateTime actual = UnixTime.SecondsToDateTimeUtc(input);
        Assert.Equal(expected, actual);
    }
}
