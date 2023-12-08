namespace AdventOfCode;

public sealed class Day8 : Day
{
    public override string Part1()
    {
        var lines = Input.Split('\n');

        var nodes = lines[2..]
            .ToDictionary(
                x => x[0..3],
                x => (l: x[7..10], r: x[12..15]));

        var x = lines[0]
            .Repeat()
            .SelectState(
                nodes["AAA"],
                (path, current) => path == 'L'
                    ? (nodes[current.l], current.l)
                    : (nodes[current.r], current.r))
            .Index()
            .First(x => x.item == "ZZZ")
            .index + 1;

        return x.ToString();
    }
}
