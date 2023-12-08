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
            .Scan(
                (node: "AAA", paths: nodes["AAA"]),
                (current, path) => path == 'L'
                    ? (current.paths.l, nodes[current.paths.l])
                    : (current.paths.r, nodes[current.paths.r]))
            .Index()
            .First(x => x.item.node == "ZZZ")
            .index;

        return x.ToString();
    }
}
