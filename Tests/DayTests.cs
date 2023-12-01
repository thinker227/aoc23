using Xunit;
using AoCHelper;
using SuperLinq;
using AdventOfCode;

namespace Tests;

public class DayTests
{
    public static IEnumerable<object[]> Problems() =>
        from type in typeof(Day).Assembly.GetTypes()
        where type.IsAssignableTo(typeof(BaseProblem))
        where !type.IsAbstract
        let ctor = type.GetConstructor([])
        where ctor is not null
        let problem = (BaseProblem)ctor.Invoke([])
        let index = problem.CalculateIndex()
        let fileName = $"Solutions/{index:D2}.txt"
        where File.Exists(fileName)
        let lines = File.ReadAllLines(fileName)
        let part1 = lines.ElementAtOrDefault(0)
        let part2 = lines.ElementAtOrDefault(1)
        select new object[] { problem, part1, part2 };

    [Theory]
    [MemberData(nameof(Problems))]
    public async Task Test(BaseProblem problem, string part1Answer, string? part2Answer)
    {
        try
        {
            var part1 = await problem.Solve_1();
            Assert.Equal(part1Answer, part1);
        }
        catch (NotImplementedException) {}

        if (part2Answer is null) return;

        try
        {
            var part2 = await problem.Solve_2();
            Assert.Equal(part2Answer, part2);
        }
        catch (NotImplementedException) {}
    }
}
