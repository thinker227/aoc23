using System.Diagnostics;

namespace AdventOfCode;

public sealed class Day5 : Day
{
    public override string Part1()
    {
        var lines = Input.Split("\n\n");
        
        var seeds = lines[0][7..]
            .Split(' ')
            .Select(long.Parse);

        return lines[1..]
            .Select(map => map
                .Split('\n')[1..]
                .Select(x => x
                    .Split(' ')
                    .Select(long.Parse)
                    .ToList())
                .Select(ns => new Range(ns[0], ns[1], ns[2]))
                .ToList())
            .Aggregate(seeds, ApplyMap)
            .Min()
            .ToString();
    }

    private static IEnumerable<long> ApplyMap(IEnumerable<long> xs, IEnumerable<Range> map) =>
        xs.Select(x => map
            .SelectNotNullV(r => r.Map(x))
            .FirstOrDefault(x));

    [DebuggerDisplay("{Dest} {Source} {Length}")]
    private readonly record struct Range(long Dest, long Source, long Length)
    {
        public long? Map(long x)
        {
            var d = x - Source;
            return d >= 0 && d < Length
                ? Dest + d
                : null;
        }
    }
}
