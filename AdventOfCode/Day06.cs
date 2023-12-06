namespace AdventOfCode;

public sealed class Day6 : Day
{
    public override string Part1()
    {
        var lines = Input
            .Split('\n')
            .Select(x => x
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)[1..]
                .Select(long.Parse))
            .ToList();

        return lines[0]
            .Zip(lines[1])
            .Select(x => new Race(x.First, x.Second))
            .ToList()
            .Select(WinCases)
            .Aggregate((acc, x) => acc * x)
            .ToString();
    }

    public override string Part2()
    {
        var lines = Input
            .Split('\n')
            .Select(x => long.Parse(
                new(x
                    .Where(char.IsAsciiDigit)
                    .ToArray())))
            .ToList();

        return WinCases(new(lines[0], lines[1])).ToString();
    }

    private static long WinCases(Race race) =>
        Helpers.Range(0, race.Time + 1)
            .Where(t => IsRecord(t, race))
            .Count();

    private static bool IsRecord(long t, Race race) =>
        t * (race.Time - t) > race.Distance;

    private readonly record struct Race(long Time, long Distance);
}
