namespace AdventOfCode;

public sealed class Day9 : Day
{
    public override string Part1() =>
        Solve(Input, (x, xs) => x + xs.Last());

    public override string Part2() =>
        Solve(Input, (x, xs) => xs.First() - x);

    private static string Solve(string input, Func<int, IEnumerable<int>, int> next) =>
        input
            .Split('\n')
            .Select(x => x
                .Split(' ')
                .Select(int.Parse))
            .Select(xs => Next(xs.ToList(), next))
            .Sum()
            .ToString();

    private static int Next(IEnumerable<int> xs, Func<int, IEnumerable<int>, int> next) =>
        DiffAll(xs)
            .Reverse()
            .Aggregate(0, next);

    private static IEnumerable<IEnumerable<int>> DiffAll(IEnumerable<int> xs) =>
        Helpers.Generate(xs, DiffSeq)
            .TakeUntil(xs => xs
                .All(x => x == 0));

    private static IEnumerable<int> DiffSeq(IEnumerable<int> xs) =>
        xs.Window(2)
            .Select(xs => xs[1] - xs[0]);
}
