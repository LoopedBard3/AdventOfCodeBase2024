using System.Net.WebSockets;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2024.Day03;

partial class Solution : SolutionBase
{
    public Solution() : base(03, 2024, "") 
    {
        Debug = false;
    }

    protected override string SolvePartOne()
    {
        var matches = mulBasicRegex().Matches(Input);
        var result = 0;
        foreach (Match match in matches)
        {
            result += int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value);
        }
        return result.ToString();
    }

    protected override string SolvePartTwo()
    {
        var matches = enabledMulRegex().Matches(Input);
        var result = 0;
        var enabled = true;
        foreach (Match match in matches)
        {
            if (match.Value.StartsWith("don"))
            {
                enabled = false;
            } 
            else if (match.Value.StartsWith("do("))
            {
                enabled = true;
            } 
            else if (match.Value.StartsWith("mul") && enabled)
            {
                result += int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value);
            }
        }
        return result.ToString();
    }

    [GeneratedRegex(@"mul\((\d+),(\d+)\)")]
    private static partial Regex mulBasicRegex();

    [GeneratedRegex(@"mul\((\d+),(\d+)\)|do\(\)|don\'t\(\)")]
    private static partial Regex enabledMulRegex();
}
