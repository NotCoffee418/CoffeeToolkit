namespace CoffeeToolkit.Time
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Converts value to Unix Time
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static int ToUnixTime(this DateTime dt)
            => UnixTime.FromDateTime(dt);

        /// <summary>
        /// Rounds up to the nearest increment by a given TimeSpan
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="d">Increment</param>
        /// <returns>Rounded DateTime</returns>
        public static DateTime RoundUp(this DateTime dt, TimeSpan d)
            => DateTimeRounder.RoundUp(dt, d);

        /// <summary>
        /// Rounds down to the nearest increment by a given TimeSpan
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="d">Increment</param>
        /// <returns>Rounded DateTime</returns>
        public static DateTime RoundDown(this DateTime dt, TimeSpan d)
            => DateTimeRounder.RoundDown(dt, d);

        /// <summary>
        /// Rounds to the nearest increment by a given TimeSpan
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="d">Increment</param>
        /// <returns>Rounded DateTime</returns>
        public static DateTime RoundToNearest(this DateTime dt, TimeSpan d)
            => DateTimeRounder.RoundToNearest(dt, d);

        /// <summary>
        /// Get DateTime for the first tick of the month
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime StartOfMonth(this DateTime dt)
            => DateTimeRounder.GetStartOfMonth(dt.Year, dt.Month, dt.Kind);

        /// <summary>
        /// Get DateTime for the last milisecond of the month
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime EndOfMonth(this DateTime dt)
            => DateTimeRounder.GetEndOfMonth(dt.Year, dt.Month, dt.Kind);
        
    }
}
