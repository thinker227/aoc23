namespace AdventOfCode;

public sealed class Day4 : Day
{
    public override string Part1() =>
        Input
            .Split('\n')
            .Select(x => x
                .Split(':')[1]
                .Split('|')
                .Select(x => x
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse))
                .Aggregate((a, b) => a.Intersect(b)))
            .Select(x => (1 << x.Count()) / 2)
            .Sum()
            .ToString();
}
