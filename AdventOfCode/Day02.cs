using Pidgin;
using static Pidgin.Parser;

namespace AdventOfCode;

public sealed class Day02 : Day
{
    public override string Part1()
    {
        var games = Parse(Input);

        var possible =
            from x in FlattenCubes(games)
            let cubes = x.cubes
            where !cubes[0].Any(x => x.Count > 12)
            where !cubes[1].Any(x => x.Count > 13)
            where !cubes[2].Any(x => x.Count > 14)
            select x.game;

        return possible
            .Select(x => x.Index)
            .Sum()
            .ToString();
    }

    public override string Part2()
    {
        var games = Parse(Input);
        
        var possible =
            from x in FlattenCubes(games)
            let cubes = x.cubes
            let r = Min(cubes[0], 12)
            let g = Min(cubes[1], 13)
            let b = Min(cubes[2], 14)
            select r * g * b;

        return possible.Sum().ToString();
    }

    private static int Min(IEnumerable<Cubes> sets, int max) =>
        sets.Select(x => x.Count).Max();

    private static List<(Game game, List<List<Cubes>> cubes)> FlattenCubes(IEnumerable<Game> games) =>
        games.Select(game => (
            game,
            game.Sets
            .SelectMany(set => set.Cubes)
            .ToList()
            .OrderBy(x => x.Color)
            .GroupBy(x => x.Color)
            .Select(xs => xs.ToList())
            .ToList()))
            .ToList();

    private static List<Game> Parse(string input) =>
        String("Game ")
            .Then(DecimalNum)
            .Before(String(": "))
            .Bind(index => (
                     DecimalNum
                    .Before(String(" "))
                    .Bind(count =>
                         CIEnum<Color>()
                        .Map(color => new Cubes(color, count)))
                    .Separated(String(", ")))
                .Separated(String("; "))
                .Map(sets => new Game(
                    index,
                    sets.Select(cubes => new Set(cubes.ToList()))
                        .ToList())))
            .Separated(String("\n"))
            .ParseOrThrow(input)
            .ToList();

    private readonly record struct Game(int Index, List<Set> Sets);

    private readonly record struct Set(List<Cubes> Cubes);

    private readonly record struct Cubes(Color Color, int Count);

    private enum Color
    {
        Red,
        Green,
        Blue,
    }
}
