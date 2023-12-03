using System.Linq;

namespace AdventOfCode;

public sealed class Day3 : Day
{
    public override string Part1()
    {
        var lineLength = Input.IndexOf('\n');
        var yOffset = lineLength + 3;
        var text = Input.Replace("\n", "...");

        var clusters = GetClusters(text);

        return text
            .Index()
            .Where(x => x.item is not ('.' or >= '0' and <= '9'))
            .SelectMany(x => GetAdjacentClusters(clusters, lineLength, x.index))
            .Select(x => x.Num)
            .Sum()
            .ToString();
    }

    public override string Part2()
    {
        var lineLength = Input.IndexOf('\n');
        var yOffset = lineLength + 3;
        var text = Input.Replace("\n", "...");

        var clusters = GetClusters(text);

        return text
            .Index()
            .Where(x => x.item == '*')
            .Select(x => GetAdjacentClusters(clusters, lineLength, x.index))
            .Where(x => x.Count == 2)
            .Select(x => x.Aggregate(1, (acc, cluster) => acc * cluster.Num))
            .Sum()
            .ToString();
    }

    private static HashSet<Cluster> GetAdjacentClusters(
        Dictionary<int, Cluster> clusters,
        int lineLength,
        int location)
    {
        var yOffset = lineLength + 3;
        var cs = new HashSet<Cluster>();

        void Add(int offset)
        {
            if (clusters.TryGetValue(location + offset, out var c)) cs!.Add(c);
        }

        Add(-yOffset - 1);
        Add(-yOffset    );
        Add(-yOffset + 1);
        Add(         - 1);
        Add(         + 1);
        Add(+yOffset - 1);
        Add(+yOffset    );
        Add(+yOffset + 1);

        return cs;
    }

    private static Dictionary<int, Cluster> GetClusters(string text) =>
        text.Index()
            .Segment((x, p, _) =>
                char.IsAsciiDigit(x.item) ^ char.IsAsciiDigit(p.item))
            .Where(x => char.IsAsciiDigit(x.First().item))
            .Select(x => (
                start: x.First().index,
                indicies: x.Select(x => x.index).ToList(),
                num: x.Aggregate(0, (acc, x) =>
                    acc * 10 + (x.item - '0'))))
            .SelectMany(x => x.indicies
                .Select(i => (i, new Cluster(x.start, x.num))))
            .ToDictionary();

    private readonly record struct Cluster(int Start, int Num);
}
