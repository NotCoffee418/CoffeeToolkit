namespace CoffeeToolkit.Time;

public static class DateTimeRounder
{
    public static DateTime RoundUp(DateTime dt, TimeSpan d)
    {
        var delta = (d.Ticks - (dt.Ticks % d.Ticks)) % d.Ticks;
        return new DateTime(dt.Ticks + delta);
    }

    public static DateTime RoundDown(DateTime dt, TimeSpan d)
    {
        var delta = dt.Ticks % d.Ticks;
        return new DateTime(dt.Ticks - delta);
    }

    public static DateTime RoundToNearest(DateTime dt, TimeSpan d)
    {
        var delta = dt.Ticks % d.Ticks;
        bool roundUp = delta > d.Ticks / 2;

        return roundUp ? RoundUp(dt, d) : RoundDown(dt, d);
    }
}
