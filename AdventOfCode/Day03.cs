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

    private static Dictionary<int, Cluster> GetClusters(string text)
    {
        var n = null as int?;
        var l = 0;

        var clusters = new Dictionary<int, Cluster>();

        for (var i = 0; i < text.Length; i++)
        {
            var c = text[i];

            if (char.IsAsciiDigit(c))
            {
                n ??= 0;
                n *= 10;
                n += c - '0';
                l++;
            }
            else if (n is {} b)
            {
                for (var lx = 0; lx < l; lx++)
                    clusters[i - 1 - lx] = new(i - l, b);

                n = null;
                l = 0;
            }
        }

        return clusters;
    }

    private readonly record struct Cluster(int Start, int Num);
}
