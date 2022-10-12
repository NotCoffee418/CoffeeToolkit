namespace CoffeeToolkit.Time;

public static class Interval
{
    public static TimeSpan Second { get; } = TimeSpan.FromSeconds(1);
    public static TimeSpan Minute { get; } = TimeSpan.FromMinutes(1);
    public static TimeSpan FiveMinutes { get; } = TimeSpan.FromMinutes(5);
    public static TimeSpan FifteenMinutes { get; } = TimeSpan.FromMinutes(15);
    public static TimeSpan HalfHour { get; } = TimeSpan.FromMinutes(30);
    public static TimeSpan Hour { get; } = TimeSpan.FromHours(1);
    public static TimeSpan SixHours { get; } = TimeSpan.FromHours(6);
    public static TimeSpan TwelveHours { get; } = TimeSpan.FromHours(12);
    public static TimeSpan Day { get; } = TimeSpan.FromDays(1);
}
