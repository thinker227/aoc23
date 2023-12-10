namespace AdventOfCode;

public sealed class Day10 : Day
{
    public override string Part1()
    {
        var yOffset = Input.IndexOf('\n') + 1;
        var text = Input.ReplaceLineEndings(".");

        var start = text.IndexOf('S');
        var startDir = text[start - 1] is '-' or 'F' or 'L'
            ? Dir.Left
            : text[start + 1] is '-' or 'J' or '7'
                ? Dir.Right
                : Dir.Up;

        int DirOf(Dir dir) => dir switch
        {
            Dir.Up    => -yOffset,
            Dir.Down  =>  yOffset,
            Dir.Left  => -1,
            Dir.Right =>  1,
            _ => throw new UnreachableException(),
        };

        (int pos, Dir dir) Walk(int pos, Dir dir)
        {
            var newPos = pos + DirOf(dir);

            var newDir = (text[newPos], dir) switch
            {
                ('-', Dir.Left)  => Dir.Left,
                ('-', Dir.Right) => Dir.Right,
                ('|', Dir.Up)    => Dir.Up,
                ('|', Dir.Down)  => Dir.Down,
                ('L', Dir.Left)  => Dir.Up,
                ('L', Dir.Down)  => Dir.Right,
                ('J', Dir.Right) => Dir.Up,
                ('J', Dir.Down)  => Dir.Left,
                ('7', Dir.Right) => Dir.Down,
                ('7', Dir.Up)    => Dir.Left,
                ('F', Dir.Left)  => Dir.Down,
                ('F', Dir.Up)    => Dir.Right,
                ('S', _)         => startDir,
                _ => throw new UnreachableException(),
            };

            return (newPos, newDir);
        }

        var path = Helpers.Generate(
                Walk(start, startDir),
                x => Walk(x.pos, x.dir))
            .TakeUntil(x => x.pos == start);

        return (path.Count() / 2).ToString();
    }

    private enum Dir
    {
        Up,
        Down,
        Left,
        Right,
    }
}
