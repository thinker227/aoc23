namespace AdventOfCode;

public sealed class Day7 : Day
{
    public override string Part1() =>
        Input
            .Split('\n')
            .Select(x => x.Split(' '))
            .Select(xs => (hand: xs[0], bet: int.Parse(xs[1])))
            .OrderBy(x => HandType(x.hand))
            .ThenBy(x => HandOrder(x.hand))
            .Index()
            .Select(x => x.item.bet * (x.index + 1))
            .Sum()
            .ToString();

    private static int HandType(string hand) =>
        hand.GroupBy(x => x)
            .Select(x => x.Count())
            .OrderDescending()
            .ToList() switch
        {
            [5] => 6,
            [4, 1] => 5,
            [3, 2] => 4,
            [3, ..] => 3,
            [2, 2, 1] => 2,
            [2, ..] => 1,
            _ => 0
        };

    private static long HandOrder(string hand) =>
        hand.Aggregate(0, (acc, x) =>
            acc * 100 + CardOrder(x));

    private static int CardOrder(char c) => c switch
    {
        'A' => 12,
        'K' => 11,
        'Q' => 10,
        'J' => 9,
        'T' => 8,
        _ => c - '2',
    };
}
