namespace AdventOfCode;

public sealed class Day11 : Day
{
    // Doesn't work
    public override string Part1()
    {
        var rows = Expand(Input.Split('\n')).ToList();
        var cols = Expand(Rotate(rows)).ToList();
        
        var width = cols[0].Length;

        var galaxies = cols
            .SelectMany(x => x)
            .Index()
            .Where(x => x.item == '#')
            .Select(x => x.index);

        var pairs = galaxies.Pairs();

        var x = pairs.Aggregate(0, (acc, x) =>
        {
            var (a, b) = x;

            var ax = a % width;
            var ay = a / width;
            var bx = b % width;
            var by = b / width;

            var d = int.Abs(ax - bx) + int.Abs(ay - by);
            return acc + d;
        });

        return x.ToString();
    }

    private static IEnumerable<string> Expand(IEnumerable<string> xs) =>
        xs.SelectMany(IEnumerable<string> (x) =>
            x.Contains('#')
                ? [x]
                : [x, x]);

    private static IEnumerable<string> Rotate(IReadOnlyList<string> xs)
    {
        var length = xs[0].Length;

        for (var i = 0; i < length; i++)
            yield return new string(xs.Select(x => x[i]).ToArray());
    }
}
