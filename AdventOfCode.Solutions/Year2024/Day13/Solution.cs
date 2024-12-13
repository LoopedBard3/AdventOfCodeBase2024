using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2024.Day13;

partial class Solution : SolutionBase
{
    public class Machine
    {
        public (BigInteger x, BigInteger y) ButtonA { get; set; }
        public (BigInteger x, BigInteger y) ButtonB { get; set; }
        public (BigInteger x, BigInteger y) Prize { get; set; }
    }

    public Solution() : base(13, 2024, "") 
    {
        Debug = true;
    }

    protected override string SolvePartOne()
    {
        var machines = new List<Machine>();
        foreach (var section in Input.SplitByParagraph())
        {
            var machine = new Machine();
            var lines = section.SplitByNewline();

            var buttonMatch = ButtonRegex().Matches(lines[0]);
            machine.ButtonA = (BigInteger.Parse(buttonMatch[0].Groups[1].Value), BigInteger.Parse(buttonMatch[0].Groups[2].Value));
            buttonMatch = ButtonRegex().Matches(lines[1]);
            machine.ButtonB = (BigInteger.Parse(buttonMatch[0].Groups[1].Value), BigInteger.Parse(buttonMatch[0].Groups[2].Value));
            var prizeMatch = PrizeRegex().Matches(lines[2]);
            machine.Prize = (BigInteger.Parse(prizeMatch[0].Groups[1].Value), BigInteger.Parse(prizeMatch[0].Groups[2].Value));
            machines.Add(machine);
        }

        BigInteger tokensSpent = 0;
        foreach (var machine in machines)
        {
            tokensSpent += LeastTokensOnMachine(machine);
        }

        return tokensSpent.ToString();
    }

    private BigInteger LeastTokensOnMachine(Machine machine)
    {
        // Calculate the max presses from ButtonA to Prize
        BigInteger aMaxPresses = machine.Prize.x / machine.ButtonA.x;
        if(aMaxPresses < machine.Prize.y/machine.ButtonA.y) aMaxPresses = machine.Prize.y / machine.ButtonA.y;
        // Calculate the max presses from ButtonB to Prize
        BigInteger bMaxPresses = machine.Prize.x / machine.ButtonB.x;
        if(bMaxPresses < machine.Prize.y/machine.ButtonB.y) bMaxPresses = machine.Prize.y / machine.ButtonB.y;

        // Initialize the minimum tokens spent to a large number
        BigInteger minTokens = -1;

        // Try all possible combinations of A presses and B presses
        for (int aPresses = 0; aPresses <= aMaxPresses; aPresses++)
        {
            for (int bPresses = 0; bPresses <= bMaxPresses; bPresses++)
            {
                if (aPresses * machine.ButtonA.x + bPresses * machine.ButtonB.x == machine.Prize.x && aPresses * machine.ButtonA.y + bPresses * machine.ButtonB.y == machine.Prize.y)
                {
                    int totalTokens = aPresses * 3 + bPresses * 1;
                    if(minTokens == -1)
                    {
                        minTokens = totalTokens;
                    }
                    else if (totalTokens < minTokens)
                    {
                        minTokens = totalTokens;
                    }
                }
            }
        }

        if (minTokens == -1)
        {
            return 0;
        }
        return minTokens;
    }

    protected override string SolvePartTwo()
    {
        var machines = new List<Machine>();
        foreach (var section in Input.SplitByParagraph())
        {
            var machine = new Machine();
            var lines = section.SplitByNewline();

            var buttonMatch = ButtonRegex().Matches(lines[0]);
            machine.ButtonA = (int.Parse(buttonMatch[0].Groups[1].Value), int.Parse(buttonMatch[0].Groups[2].Value));
            buttonMatch = ButtonRegex().Matches(lines[1]);
            machine.ButtonB = (int.Parse(buttonMatch[0].Groups[1].Value), int.Parse(buttonMatch[0].Groups[2].Value));
            var prizeMatch = PrizeRegex().Matches(lines[2]);
            machine.Prize = (BigInteger.Parse(prizeMatch[0].Groups[1].Value) + 10000000000000, BigInteger.Parse(prizeMatch[0].Groups[2].Value) + 10000000000000);
            machines.Add(machine);
        }

        BigInteger tokensSpent = 0;
        foreach (var machine in machines)
        {
            tokensSpent += LeastTokensOnMachine(machine);
        }

        return tokensSpent.ToString();
    }

    [GeneratedRegex(@"X\+(\d+), Y\+(\d+)")]
    private static partial Regex ButtonRegex();
    
    [GeneratedRegex(@"X=(\d+), Y=(\d+)")]
    private static partial Regex PrizeRegex();
}
