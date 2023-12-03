namespace AdventOfCode;

public sealed class Day3 : Day
{
    public override string Part1()
    {
        var lineLength = Input.IndexOf('\n');
        var yOffset = lineLength + 3;
        var text = Input.Replace("\n", "...");

        var n = null as int?;
        var l = 0;

        var clusterLocations = new Dictionary<int, Cluster>();

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
                    clusterLocations[i - 1 - lx] = new(i - l, b);

                n = null;
                l = 0;
            }
        }

        var clusters = new HashSet<Cluster>();

        void AddCluster(int index)
        {
            if (clusterLocations.TryGetValue(index, out var cluster))
                clusters.Add(cluster);
        }

        for (var i = 0 ; i < text.Length; i++)
        {
            var c = text[i];

            if (char.IsAsciiDigit(c) || c == '.') continue;

            AddCluster(i - yOffset - 1);
            AddCluster(i - yOffset    );
            AddCluster(i - yOffset + 1);
            AddCluster(i           - 1);
            AddCluster(i           + 1);
            AddCluster(i + yOffset - 1);
            AddCluster(i + yOffset    );
            AddCluster(i + yOffset + 1);
        }

        return clusters
            .Select(x => x.Num)
            .Sum()
            .ToString();
    }

    private readonly record struct Cluster(int Start, int Num);
}
