namespace CoffeeToolkit.Time;

public static class UnixTime
{
    /// <summary>
    /// Converts Unix time in seconds to DateTime
    /// </summary>
    /// <param name="unixTime"></param>
    /// <returns></returns>
    public static DateTime SecondsToDateTimeUtc(long unixTime)
        => DateTimeOffset.FromUnixTimeSeconds(unixTime).UtcDateTime;

    /// <summary>
    /// Converts Unix time in milliseconds to DateTime
    /// </summary>
    /// <param name="unixTime"></param>
    /// <returns></returns>
    public static DateTime MillisecondsToDateTimeUtc(long unixTime)
        => DateTimeOffset.FromUnixTimeMilliseconds(unixTime).UtcDateTime;

    /// <summary>
    /// Converts value to UTC Unix Time in seconds
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="castToUtc"></param>
    /// <returns></returns>
    public static long FromDateTimeToSeconds(this DateTime dt, bool castToUtc = false)
        => dt
        .ToDateTimeOffsetUtc(castToUtc && dt.Kind != DateTimeKind.Utc ? DateTimeKind.Utc : null)
        .ToUnixTimeSeconds();

    /// <summary>
    /// Converts value to UTC Unix Time in milliseconds
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="castToUtc">Set to true when the input DateTime is UTC but it's kind is not specified as UTC.</param>
    /// <returns></returns>
    public static long FromDateTimeToMilliseconds(this DateTime dt, bool castToUtc = false)
        => dt
        .ToDateTimeOffsetUtc(castToUtc && dt.Kind != DateTimeKind.Utc ? DateTimeKind.Utc : null)
        .ToUnixTimeMilliseconds();
}
