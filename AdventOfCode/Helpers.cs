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
}
