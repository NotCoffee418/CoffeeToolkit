using CoffeeToolkit.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeToolkit.Extensions
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
    }
}
