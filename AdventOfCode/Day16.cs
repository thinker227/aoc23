namespace AdventOfCode;

public sealed class Day16 : Day
{
    public override string Part1() =>
        Energized(Input, new(0, Direction.Right)).ToString();

    public override string Part2()
    {
        var width = Input.IndexOf('\n');
        var yOffset = width + 1;

        var starts = Enumerable
            .Range(0, width)
            .Select(x => new Location(x, Direction.Down))
            .Concat(Enumerable
                .Range(yOffset * (width - 1), width)
                .Select(x => new Location(x, Direction.Up)))
            .Concat(SuperEnumerable
                .Range(0, width, yOffset)
                .Select(x => new Location(x, Direction.Right)))
            .Concat(SuperEnumerable
                .Range(width - 1, width, yOffset)
                .Select(x => new Location(x, Direction.Left)));

        return starts
            .Select(l => Energized(Input, l))
            .Max()
            .ToString();
    }

    private static int Energized(string map, Location start)
    {
        var yOffset = map.IndexOf('\n') + 1;
        var visited = new HashSet<Location>();
        var beams = new Stack<Location>([start]);

        int FromDir(Direction dir) => dir switch
        {
            Direction.Up => -yOffset,
            Direction.Down => yOffset,
            Direction.Left => -1,
            Direction.Right => 1,
            _ => throw new UnreachableException(),
        };

        while (beams.TryPop(out var current))
        {
            while (
                !visited.Contains(current) &&
                current.Position >= 0 &&
                current.Position < map.Length)
            {
                var (pos, dir) = current;
                var x = map[pos];
                if (x == '\n') break;

                visited.Add(current);

                var (main, split) = (x, dir) switch
                {
                    ('/', Direction.Up)                             => (Direction.Right, null as Direction?),
                    ('/', Direction.Down)                           => (Direction.Left, null),
                    ('/', Direction.Left)                           => (Direction.Down, null),
                    ('/', Direction.Right)                          => (Direction.Up, null),
                    ('\\', Direction.Up)                            => (Direction.Left, null),
                    ('\\', Direction.Down)                          => (Direction.Right, null),
                    ('\\', Direction.Left)                          => (Direction.Up, null),
                    ('\\', Direction.Right)                         => (Direction.Down, null),
                    ('.' or '|', Direction.Up or Direction.Down)    => (dir, null),
                    ('.' or '-', Direction.Left or Direction.Right) => (dir, null),
                    ('-', Direction.Up or Direction.Down)           => (Direction.Left, Direction.Right),
                    ('|', Direction.Left or Direction.Right)        => (Direction.Up, Direction.Down),
                    _ => throw new UnreachableException(),
                };

                current = new(pos + FromDir(main), main);

                if (split is not null)
                    beams.Push(new(pos + FromDir(split.Value), split.Value));
            }
        }

        return visited
            .DistinctBy(x => x.Position)
            .Count();
    }

    private readonly record struct Location(int Position, Direction Direction);

    private enum Direction
    {
        Up,
        Down,
        Left,
        Right,
    }
}
