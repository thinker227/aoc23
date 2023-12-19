using Pidgin;
using static Pidgin.Parser;
using Part = System.Collections.Generic.Dictionary<char, int>;

namespace AdventOfCode;

public sealed class Day19 : Day
{
    public override string Part1()
    {
        var split = Input.Split("\n\n");

        var workflows = ParseWorkflows(split[0])
            .ToDictionary(w => w.Name);

        var parts = ParseParts(split[1]);

        var accepted = new List<Part>();

        foreach (var part in parts)
        {
            var workflow = workflows["in"];
            while (true)
            {
                var result = workflow.Rules
                    .SelectNotNullR(x => x(part))
                    .FirstOrDefault()
                    ?? workflow.Final;

                switch (result)
                {
                    case Result.Workflow w:
                        workflow = workflows[w.Name];
                        break;

                    case Result.Accept:
                        accepted.Add(part);
                        goto end;

                    case Result.Reject:
                        goto end;
                }
            }

            end:;
        }

        return accepted
            .Sum(part => part
                .Sum(x => x.Value))
            .ToString();
    }

    private static List<Workflow> ParseWorkflows(string str)
    {
        var result = OneOf(
            Char('A').ThenReturn<Result>(new Result.Accept()),
            Char('R').ThenReturn<Result>(new Result.Reject()),
            Letter.ManyString().Map<Result>(x => new Result.Workflow(x)));

        var rule =
            from x in Letter
            from t in OneOf('<', '>')
            from n in DecimalNum.Before(Char(':'))
            from r in result
            select new Func<Part, Result?>(part =>
                (t == '<' ? part[x] < n : part[x] > n) ? r : null);

        var workflow =
            from name in Letter.ManyString().Before(Char('{'))
            from rules in Try(rule).SeparatedAndTerminated(Char(','))
            from final in result.Before(Char('}'))
            select new Workflow(name, rules.ToList(), final);

        return workflow
            .Separated(Char('\n'))
            .ParseOrThrow(str)
            .ToList();
    }

    private static List<Part> ParseParts(string str)
    {
        var rating =
            from c in Letter.Before(Char('='))
            from n in DecimalNum
            select (c, n);

        return Char('{')
            .Then(rating.Separated(Char(',')))
            .Before(Char('}'))
            .Separated(Char('\n'))
            .ParseOrThrow(str)
            .Select(x => x.ToDictionary())
            .ToList();
    }

    private readonly record struct Workflow(
        string Name,
        IReadOnlyList<Func<Part, Result?>> Rules,
        Result Final);

    private abstract record Result
    {
        public sealed record Accept : Result;
        public sealed record Reject : Result;
        public sealed record Workflow(string Name) : Result;
    }
}
