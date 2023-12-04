namespace AdventOfCode;

public sealed class Day4 : Day
{
    public override string Part1() =>
        Matches(Input)
            .Select(x => (1 << x) / 2)
            .Sum()
            .ToString();

    public override string Part2()
    {
        var matches = Matches(Input)
            .ToList();

        var copies = Enumerable.Repeat(1, matches.Count).ToList();

        return matches
            .Select(cardMatches =>
            {
                var cardCopies = copies[0];
                copies.RemoveAt(0);
                for (var i = 0; i < cardMatches; i++) copies[i] += cardCopies;
                return cardCopies;
            })
            .Sum()
            .ToString();
    }

    private static IEnumerable<int> Matches(string input) =>
        input
            .Split('\n')
            .Select(s => s
                .Split(':')[1]
                .Split('|')
                .Select(x => x
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse))
                .Aggregate((a, b) => a.Intersect(b)))
            .Select(x => x.Count());
}
