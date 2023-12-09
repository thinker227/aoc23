namespace AdventOfCode;

public sealed class Day8 : Day
{
    public override string Part1()
    {
        var lines = Input
            .Split('\n');

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

    // Doesn't work
    public override string Part2()
    {
        var lines = Input.Split('\n');

        var seq = lines[0];

        var nodes = lines[2..]
            .ToDictionary(
                x => x[0..3],
                x => new Node(
                    x[0..3],
                    x[7..10],
                    x[12..15]));

        List<Node> FindLoop(Node start)
        {
            var visited = new HashSet<(Node, int)>();
            var path = new List<Node>();

            Visit(start, 0);

            void Visit(Node node, int pos)
            {
                if (visited.Contains((node, pos))) return;
                visited.Add((node, pos));

                var p = seq![pos];
                var t = p == 'L' ? node.Left : node.Right;
                var n = nodes![t];
                path.Add(node);

                Visit(n, (pos + 1) % seq.Length);
            }

            return path;
        }

        foreach (var node in nodes.Values.Where(x => x.Name.EndsWith('A')))
        {
            var loop = FindLoop(node);
        }

        throw new NotImplementedException();
    }

    private readonly record struct Node(string Name, string Left, string Right)
    {
        public override string ToString() => $"{Name} = ({Left}, {Right})";
    }
}
