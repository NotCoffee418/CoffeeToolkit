namespace CoffeeToolkit.Time;

public static class UnixTime
{
    private static readonly DateTime unixStartTime = new(1970, 1, 1);

    /// <summary>
    /// Converts DateTime to Unix time
    /// </summary>
    /// <param name="dt"></param>
    /// <exception cref="ArgumentOutOfRangeException">aaa</exception>
    /// <returns></returns>
    public static int FromDateTime(DateTime dt)
    {
        int result = (int)dt.Subtract(unixStartTime).TotalSeconds;
        if (result == int.MinValue)
            throw new ArgumentOutOfRangeException(
                nameof(dt), "Input DateTime was too high or low to convert to Unix time.");
        else return result;
    }

    /// <summary>
    /// Converts Unix time to DateTime
    /// </summary>
    /// <param name="unixTime"></param>
    /// <returns></returns>
    public static DateTime ToDateTime(int unixTime)
        => unixStartTime.AddSeconds(unixTime);
}
