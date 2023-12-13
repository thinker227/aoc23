namespace AdventOfCode;

public sealed class Day13 : Day
{
    public override string Part1()
    {
        var patterns = Input.Split("\n\n");

        var scores = patterns
            .Select(Handle);

        var (rows, cols) = scores
            .Aggregate((acc, x) =>
                (acc.rows + x.rows, acc.cols + x.cols));

        return (cols + rows * 100).ToString();
    }

    private static (int rows, int cols) Handle(string pattern)
    {
        var rows = pattern.Split('\n');
        var cols = rows
            .ZipMany()
            .Select(x => new string(x.ToArray()))
            .ToArray();

        return (CheckMirror(rows) ?? 0, CheckMirror(cols) ?? 0);
    }

    private static int? CheckMirror(string[] lines)
    {
        for (var i = 1; i < lines.Length; i++)
        {
            if (lines[i - 1] == lines[i])
            {
                if (lines[..(i - 1)]
                    .Reverse()
                    .Zip(lines[(i + 1)..])
                    .Aggregate(true, (acc, x) =>
                        acc && x.First == x.Second))
                {
                    return i;
                }
            }
        }

        return null;
    }
}
