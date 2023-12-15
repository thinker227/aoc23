namespace AdventOfCode;

public sealed class Day15 : Day
{
    public override string Part1() => Input
        .Split(',')
        .Select(xs => xs
            .Aggregate(0, (acc, x) =>
                (acc + (byte)x) * 17 % 256))
        .Sum()
        .ToString();
}
