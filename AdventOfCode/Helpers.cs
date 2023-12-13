using System.Numerics;

namespace AdventOfCode;

public static class Helpers
{
    /// <summary>
    /// Maps a sequence to a sequence of nullable reference-type values and filters away null.
    /// </summary>
    public static IEnumerable<TResult> SelectNotNullR<T, TResult>(this IEnumerable<T> xs, Func<T, TResult?> f)
        where TResult : class
    {
        foreach (var x in xs)
        {
            if (f(x) is {} t) yield return t;
        }
    }

    /// <summary>
    /// Maps a sequence to a sequence of nullable value-type values and filters away null.
    /// </summary>
    public static IEnumerable<TResult> SelectNotNullV<T, TResult>(this IEnumerable<T> xs, Func<T, TResult?> f)
        where TResult : struct
    {
        foreach (var x in xs)
        {
            if (f(x) is {} t) yield return t;
        }
    }

    /// <summary>
    /// Creates a range of values of any numeric type.
    /// </summary>
    public static IEnumerable<T> Range<T>(T start, T count) where T : INumber<T>
    {
        for (var i = start; i < count; i += T.One) yield return i;
    }

    /// <summary>
    /// Generates a sequence by continually applying a function over the previous value.
    /// </summary>
    public static IEnumerable<T> Generate<T>(T x, Func<T, T> f)
    {
        while (true)
        {
            yield return x;
            x = f(x);
        }
    }

    /// <summary>
    /// Gets all pairs of items in a sequence.
    /// </summary>
    public static IEnumerable<(T, T)> Pairs<T>(this IEnumerable<T> xs)
    {
        var list = new List<T>();

        foreach (var x in xs)
        {
            foreach (var y in list) yield return (x, y);

            list.Add(x);
        }
    }

    /// <summary>
    /// Enumerates a sequences of enumerables and zips together the nth element of each enumerable.
    /// </summary>
    public static IEnumerable<IReadOnlyList<T>> ZipMany<T>(this IEnumerable<IEnumerable<T>> xs)
    {
        var es = xs
            .Select(x => x.GetEnumerator())
            .ToArray();

        while (true)
        {
            var list = new List<T>();

            foreach (var e in es)
            {
                if (!e.MoveNext()) yield break;
                list.Add(e.Current);
            }

            yield return list;
        }
    }
}
