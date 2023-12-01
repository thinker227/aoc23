namespace AdventOfCode;

public sealed class Day01 : BaseDay
{
    private readonly string input;

    public Day01() => input = File.ReadAllText(InputFilePath);

    public override ValueTask<string> Solve_1() =>
        Solve(xs => xs
            .Where(char.IsAsciiDigit)
            .Select(x => x - '0'));

    public override ValueTask<string> Solve_2() =>
        Solve(x => x
            .WindowLeft(5)
            .SelectNotNullV(int? (xs) => xs switch
            {
                [>= '0' and <= '9' and var c, ..] => c - '0',
                ['o', 'n', 'e', ..] => 1,
                ['t', 'w', 'o', ..] => 2,
                ['t', 'h', 'r', 'e', 'e', ..] => 3,
                ['f', 'o', 'u', 'r', ..] => 4,
                ['f', 'i', 'v', 'e', ..] => 5,
                ['s', 'i', 'x', ..] => 6,
                ['s', 'e', 'v', 'e', 'n', ..] => 7,
                ['e', 'i', 'g', 'h', 't', ..] => 8,
                ['n', 'i', 'n', 'e', ..] => 9,
                    _ => null
            }));

    private ValueTask<string> Solve(Func<string, IEnumerable<int>> f)
    {
        var result = input.Split('\n')
            .Select(x => f(x).ToList())
            .Select(xs => xs[0] * 10 + xs[^1])
            .Sum();

        return new(result.ToString());
    }
}
