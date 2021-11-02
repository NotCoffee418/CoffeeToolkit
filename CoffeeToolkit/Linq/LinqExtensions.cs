using System.Collections.Generic;

namespace CoffeeToolkit.Linq
{
    public static class LinqExtensions
    {
        /// <summary>
        /// Returns the first element of sequence, or null if the sequence contains no elements
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static T? FirstOrNull<T>(this IEnumerable<T> items) where T : struct
        {
            var iter = items.GetEnumerator();
            if (!iter.MoveNext())
                return (T?)null;

            return iter.Current;
        }
    }
}
