using MirrorCheck = System.Func<System.Collections.Generic.IEnumerable<(string a, string b)>, bool>;

namespace AdventOfCode;

public sealed class Day13 : Day
{
    public override string Part1() => Solve(
        Input,
        zip => zip.Aggregate(true, (acc, x) =>
            acc && x.a == x.b));

    // Doesn't work
    public override string Part2() => Solve(Input, Part2MirrorCheck);

    private static bool Part2MirrorCheck(IEnumerable<(string a, string b)> xs)
    {
        static (int, int) Count(string s) =>
            (s.Count(c => c == '.'), s.Count(c => c == '#'));

        var z = xs
            .Select(x => (Count(x.a), Count(x.b)))
            .ToArray();

        if (z.Length == 0) return false;

        foreach (var ((a1, b1), (a2, b2)) in z)
        {
            var a = a1 - a2 is not (< -1 or > 1);
            var b = b1 - b2 is not (< -1 or > 1);
            if (a && b || !a && b || a && !b) continue;
            return false;
        }

        return true;
    }

    private static string Solve(string input, MirrorCheck checkMirror)
    {
        var patterns = input.Split("\n\n");

        var scores = patterns
            .Select(x => Handle(x, checkMirror));

        var (rows, cols) = scores
            .Aggregate((acc, x) =>
                (acc.rows + x.rows, acc.cols + x.cols));

        return (cols + rows * 100).ToString();
    }

    private static (int rows, int cols) Handle(string pattern, MirrorCheck checkMirror)
    {
        var rows = pattern.Split('\n');
        var cols = rows
            .ZipMany()
            .Select(x => new string(x.ToArray()))
            .ToArray();

        return (CheckMirror(rows, checkMirror) ?? 0, CheckMirror(cols, checkMirror) ?? 0);
    }

    private static int? CheckMirror(string[] lines, MirrorCheck checkMirror)
    {
        for (var i = 1; i < lines.Length; i++)
        {
            var zip = lines[..(i - 1)]
                .Reverse()
                .Zip(lines[(i + 1)..]);

            if (checkMirror(zip))
            {
                return i;
            }
        }

        return null;
    }
}
