namespace CoffeeToolkit.Time;

public static class DateTimeRounder
{
    /// <summary>
    /// Rounds up to the nearest increment by a given TimeSpan
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="d"></param>
    /// <returns></returns>
    public static DateTime RoundUp(DateTime dt, TimeSpan d)
    {
        var delta = (d.Ticks - (dt.Ticks % d.Ticks)) % d.Ticks;
        return new DateTime(dt.Ticks + delta);
    }

    /// <summary>
    /// Rounds down to the nearest increment by a given TimeSpan
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="d"></param>
    /// <returns></returns>
    public static DateTime RoundDown(DateTime dt, TimeSpan d)
    {
        var delta = dt.Ticks % d.Ticks;
        return new DateTime(dt.Ticks - delta);
    }

    /// <summary>
    /// Rounds to the nearest increment by a given TimeSpan
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="d"></param>
    /// <returns></returns>
    public static DateTime RoundToNearest(DateTime dt, TimeSpan d)
    {
        var delta = dt.Ticks % d.Ticks;
        bool roundUp = delta > d.Ticks / 2;

        return roundUp ? RoundUp(dt, d) : RoundDown(dt, d);
    }

    /// <summary>
    /// Get DateTime for the first tick of the month
    /// </summary>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <param name="kind"></param>
    /// <returns></returns>
    public static DateTime GetStartOfMonth(
        int year, 
        int month, 
        DateTimeKind kind = DateTimeKind.Unspecified)
        => new DateTime(year, month, 1, 0,0,0, kind);

    /// <summary>
    /// Get DateTime for the last milisecond of the month
    /// </summary>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <param name="kind"></param>
    /// <returns></returns>
    public static DateTime GetEndOfMonth(
        int year, 
        int month, 
        DateTimeKind kind = DateTimeKind.Unspecified)
        => new DateTime(year, month, 
            DateTime.DaysInMonth(year, month),
            23, 59, 59, 999,
            kind);
}
