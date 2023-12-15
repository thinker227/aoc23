using Pidgin;

namespace AdventOfCode;

public sealed class Day15 : Day
{
    public override string Part1() => Input
        .Split(',')
        .Select(Hash)
        .Sum()
        .ToString();

    public override string Part2()
    {
        var boxes = Enumerable
            .Range(0, 256)
            .Select(_ => new List<(string label, int strength)>())
            .ToArray();

        var parser =
            from str in Parser.Letter.ManyString()
            from op in Parser.Char('-').Or(Parser.Char('='))
            from num in Parser.DecimalNum.Optional().Map(x => x.GetValueOrDefault(0))
            select (str, op, num);

        foreach (var str in Input.Split(','))
        {
            var (label, op, strength) = parser.ParseOrThrow(str);
            var box = boxes[Hash(label)];

            if (op == '-') box.RemoveAll(x => x.label == label);
            else
            {
                var index = box.FindIndex(x => x.label == label);
                if (index != -1) box[index] = (label, strength);
                else box.Add((label, strength));
            }
        }

        return boxes
            .Index()
            .Select(box => box.item
                .Index()
                .Select(lens =>
                    (box.index + 1) * (lens.index + 1) * lens.item.strength)
                .Sum())
            .Sum()
            .ToString();
    }

    private static int Hash(IEnumerable<char> xs) =>
        xs.Aggregate(0, (acc, x) =>
            (acc + (byte)x) * 17 % 256);
}
