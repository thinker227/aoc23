namespace AdventOfCode;

public abstract class Day : BaseDay
{
    public string Input { get; }

    public Day() =>
        Input = File.ReadAllText(InputFilePath);

    public virtual string Part1() => throw new NotImplementedException();

    public virtual string Part2() => throw new NotImplementedException();

    public sealed override ValueTask<string> Solve_1() =>
        new(Part1());

    public sealed override ValueTask<string> Solve_2() =>
        new(Part2());
}
