namespace AdventOfCode;

public sealed class Day14 : Day
{
    public override string Part1()
    {
        var size = Input.IndexOf('\n');

        return Input
            .Split('\n')
            .ZipMany()
            .Select(xs => xs
                .Index()
                .Split(x => x.item == '#')
                .Where(g => g.Any())
                .Select(g => (
                    start: size - g.First().index,
                    count: g.Count(x => x.item == 'O')))
                .Select(g => Enumerable
                    .Range(g.start - g.count + 1, g.count)
                    .Sum())
                .Sum())
            .Sum()
            .ToString();
    }
}
