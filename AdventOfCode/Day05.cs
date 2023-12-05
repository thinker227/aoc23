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

        var _seeds = lines[0][7..]
            .Split(' ')
            .Select(long.Parse)
            .Select(x => new SeedRange(x, 1));

        return GetMapRanges(lines)
            .Aggregate(seeds, ApplyMap)
            .Min()
            .ToString();
    }

    public override string Part2()
    {
        var lines = Input.Split("\n\n");

        var seeds = lines[0][7..]
            .Split(' ')
            .Select(long.Parse)
            .Batch(2)
            .Select(ns => new SeedRange(ns[0], ns[1]));

        return GetMapRanges(lines)
            .Aggregate(seeds, ApplyMap)
            .Select(x => x.Start)
            .Min()
            .ToString();
    }

    private static IEnumerable<IEnumerable<MapRange>> GetMapRanges(string[] lines) =>
        lines[1..]
            .Select(map => map
                .Split('\n')[1..]
                .Select(x => x
                    .Split(' ')
                    .Select(long.Parse)
                    .ToList())
                .Select(ns => new MapRange(ns[0], ns[1], ns[2]))
                .ToList());

    private static IEnumerable<long> ApplyMap(IEnumerable<long> xs, IEnumerable<MapRange> map) =>
        xs.Select(x => map
            .SelectNotNullV(r => r.Map(x))
            .FirstOrDefault(x));

    private static IEnumerable<SeedRange> ApplyMap(IEnumerable<SeedRange> xs, IEnumerable<MapRange> map) =>
        xs.SelectMany(x => Slice(x, map)
            .FallbackIfEmpty([x]));

    private static IEnumerable<SeedRange> Slice(SeedRange seeds, IEnumerable<MapRange> maps)
    {
        var normalized = maps
            .Select(m => MapRange.FromEnd(
                m.Dest,
                Math.Max(m.Source, seeds.Start),
                Math.Min(m.SourceEnd, seeds.End)))
            .Where(m => m.Length > 0)
            .OrderBy(m => m.Source)
            .ThenByDescending(m => m.Length);

        var start = seeds.Start;

        foreach (var map in normalized)
        {
            // Map range fully overlaps seed range
            // if (start >= map.Source && seeds.End <= map.SourceEnd)
            if (start == map.Source && seeds.End == map.SourceEnd)
            {
                yield return new(start + map.Delta, seeds.Length);
                yield break;
            }

            // Map intersects on the left
            if (map.Source <= start && map.SourceEnd > start && map.SourceEnd <= seeds.End)
            {
                yield return new(start + map.Delta, map.SourceEnd - start);
                start = map.SourceEnd;
                continue;
            }

            // Map intersects on the right
            if (map.Source > start && map.Source <= seeds.End && map.SourceEnd >= seeds.End)
            {
                yield return new(map.Source + map.Delta, seeds.End - map.Source);
                yield break;
            }

            // Map intersects partially
            yield return new(map.Source + map.Delta, map.Length);
            start = map.SourceEnd;
        }

        if (start < seeds.End) yield return new(start, seeds.End - start);
    }

    [DebuggerDisplay("{Start} {Length}")]
    private readonly record struct SeedRange(long Start, long Length)
    {
        public long End => Start + Length;
    }

    [DebuggerDisplay("{Dest} {Source} {Length}")]
    private readonly record struct MapRange(long Dest, long Source, long Length)
    {
        public static MapRange FromEnd(long dest, long source, long sourceEnd) =>
            new(dest, source, sourceEnd - source);

        public long SourceEnd => Source + Length;

        public long Delta => Dest - Source;

        public long? Map(long x)
        {
            var d = x - Source;
            return d >= 0 && d < Length
                ? Dest + d
                : null;
        }
    }
}
