namespace CoffeeToolkit.Linq;

public static class EnumerableExtensions
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

    #if NETSTANDARD2_1
    /// <summary>
    /// This works the same as DistinctBy for .NET versions that don't have access to it.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <typeparam name="TKey">The type of key to distinguish elements by.</typeparam>
    /// <param name="source">The sequence to remove duplicate elements from.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <returns>An IEnumerable<T> that contains distinct elements from the source sequence.</returns>
    public static IEnumerable<TSource> UniqueBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
        var seenKeys = new HashSet<TKey>();
        foreach (var element in source)
        {
            if (seenKeys.Add(keySelector(element)))
            {
                yield return element;
            }
        }
    }
    #endif

}
