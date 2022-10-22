namespace CoffeeToolkit.Time
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Converts value to UTC Unix Time in seconds
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="castToUtc"></param>
        /// <returns></returns>
        public static long ToUnixTimeSeconds(this DateTime dt, bool castToUtc = false)
            => UnixTime.FromDateTimeToSeconds(dt, castToUtc);

        /// <summary>
        /// Converts value to UTC Unix Time in milliseconds
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="castToUtc">Set to true when the input DateTime is UTC but it's kind is not specified as UTC.</param>
        /// <returns></returns>
        public static long ToUnixTimeMilliseconds(this DateTime dt, bool castToUtc = false)
            => UnixTime.FromDateTimeToMilliseconds(dt, castToUtc);

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
        /// Get DateTime for the last millisecond of the month
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime EndOfMonth(this DateTime dt)
            => DateTimeRounder.GetEndOfMonth(dt.Year, dt.Month, dt.Kind);

        /// <summary>
        /// Converts DateTime into DateTimeOffset with the option to override the DateTimeKind
        /// </summary>
        /// <param name="dt">DateTime to parse</param>
        /// <param name="overrideKind">Optionally override the DateTime kind</param>
        /// <returns></returns>
        public static DateTimeOffset ToDateTimeOffsetUtc(this DateTime dt, DateTimeKind? overrideKind)
            => new DateTimeOffset(overrideKind.HasValue ?
                DateTime.SpecifyKind(dt, overrideKind.Value) : dt);
    }
}
