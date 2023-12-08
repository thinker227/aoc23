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

        var current = nodes["AAA"];
        foreach (var (index, path) in lines[0].Repeat().Index())
        {
            var x = path == 'L'
                ? current.l
                : current.r;

            if (x == "ZZZ") return (index + 1).ToString();
            
            current = nodes[x];
        }

        throw new UnreachableException();
    }
}
