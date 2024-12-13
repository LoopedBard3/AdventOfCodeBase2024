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
        Debug = false;
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
        if(aMaxPresses > 100) aMaxPresses = 100;

        // Calculate the max presses from ButtonB to Prize
        BigInteger bMaxPresses = machine.Prize.x / machine.ButtonB.x;
        if(bMaxPresses < machine.Prize.y/machine.ButtonB.y) bMaxPresses = machine.Prize.y / machine.ButtonB.y;
        if (bMaxPresses > 100) bMaxPresses = 100;

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
            tokensSpent += LeastTokensOnMachineUncapped2(machine);
        }

        return tokensSpent.ToString();
    }

    private BigInteger LeastTokensOnMachineUncapped(Machine machine)
    {
        // Calculate the max presses from ButtonA to Prize
        BigInteger aMaxPresses = machine.Prize.x / machine.ButtonA.x;
        if (aMaxPresses > machine.Prize.y / machine.ButtonA.y) aMaxPresses = machine.Prize.y / machine.ButtonA.y;
        // Calculate the max presses from ButtonB to Prize
        BigInteger bMaxPresses = machine.Prize.x / machine.ButtonB.x;
        if (bMaxPresses > machine.Prize.y / machine.ButtonB.y) bMaxPresses = machine.Prize.y / machine.ButtonB.y;
        
        var aDistancePerPressUnsquared = machine.ButtonA.x * machine.ButtonA.x + machine.ButtonA.y * machine.ButtonA.y;
        var bDistancePerPressUnsquared = machine.ButtonB.x * machine.ButtonB.x + machine.ButtonB.y * machine.ButtonB.y;
        bool aWorthUsingFirst = aDistancePerPressUnsquared > bDistancePerPressUnsquared * 3;

        // Initialize the minimum tokens spent to a large number
        BigInteger minTokens = -1;

        if (aWorthUsingFirst)
        {
            // Try all possible combinations of A presses and B presses, starting with A at its max presses and going B X * Y times
            var aToTest = machine.ButtonA.x * machine.ButtonA.y * machine.ButtonB.x * machine.ButtonB.y / 100;
            Console.WriteLine($"Check with params: a max - toTest: {aMaxPresses} {aToTest}");
            for (BigInteger aPresses = aMaxPresses; aPresses > aMaxPresses - machine.ButtonB.x * machine.ButtonB.y; aPresses--)
            {
                for (BigInteger bPresses = 0; bPresses <= bMaxPresses; bPresses++)
                {
                    if (aPresses * machine.ButtonA.x + bPresses * machine.ButtonB.x == machine.Prize.x && aPresses * machine.ButtonA.y + bPresses * machine.ButtonB.y == machine.Prize.y)
                    {
                        BigInteger totalTokens = aPresses * 3 + bPresses * 1;
                        if (minTokens == -1)
                        {
                            minTokens = totalTokens;
                        }
                        else if (totalTokens < minTokens)
                        {
                            minTokens = totalTokens;
                        }
                    }

                    // If we are over the prize for x or y, break
                    if(aPresses * machine.ButtonA.x + bPresses * machine.ButtonB.x > machine.Prize.x || aPresses * machine.ButtonA.y + bPresses * machine.ButtonB.y > machine.Prize.y)
                    {
                        break;
                    }
                }
            }
        }
        else
        {
            // Try all possible combinations of A presses and B presses, starting with A at its max presses and going B X * Y times
            var bToTest = machine.ButtonA.x * machine.ButtonA.y * machine.ButtonB.x * machine.ButtonB.y / 100;
            Console.WriteLine($"Check with params: b max - toTest: {bMaxPresses} {bToTest}");
            for (BigInteger bPresses = bMaxPresses; bPresses > bMaxPresses - bToTest; bPresses--)
            {
                for (BigInteger aPresses = 0; aPresses <= aMaxPresses; aPresses++)
                {
                    //Console.WriteLine($"Checking a: {aPresses} b: {bPresses}");
                    if (aPresses * machine.ButtonA.x + bPresses * machine.ButtonB.x == machine.Prize.x && aPresses * machine.ButtonA.y + bPresses * machine.ButtonB.y == machine.Prize.y)
                    {
                        BigInteger totalTokens = aPresses * 3 + bPresses * 1;
                        if (minTokens == -1)
                        {
                            minTokens = totalTokens;
                        }
                        else if (totalTokens < minTokens)
                        {
                            minTokens = totalTokens;
                        }
                    }

                    // If we are over the prize for x or y, break
                    if (aPresses * machine.ButtonA.x + bPresses * machine.ButtonB.x > machine.Prize.x || aPresses * machine.ButtonA.y + bPresses * machine.ButtonB.y > machine.Prize.y)
                    {
                        break;
                    }
                }
            }
        }


        if (minTokens == -1)
        {
            Console.WriteLine("Not Found");
            return 0;
        }
        Console.WriteLine("Found");
        return minTokens;
    }

    private BigInteger LeastTokensOnMachineUncapped2(Machine machine)
    {
        // We are going to solve the system of equations:
        // machine.Prize.x = aPresses * machine.ButtonA.x + bPresses * machine.ButtonB.x
        // machine.Prize.y = aPresses * machine.ButtonA.y + bPresses * machine.ButtonB.y
        // We are going to solve this by multiplying the first equation by machine.ButtonB.y and the second by machine.ButtonA.y
        // and subtracting the two equations to get aPresses = (machine.Prize.x * machine.ButtonB.y - machine.Prize.y * machine.ButtonB.x) / (machine.ButtonA.x * machine.ButtonB.y - machine.ButtonA.y * machine.ButtonB.x)
        var aPresses = (machine.Prize.x * machine.ButtonB.y - machine.Prize.y * machine.ButtonB.x) / (machine.ButtonA.x * machine.ButtonB.y - machine.ButtonA.y * machine.ButtonB.x);
        // Then we can solve for bPresses by plugging aPresses into the first equation
        // bPresses = (machine.Prize.x - aPresses * machine.ButtonA.x) / machine.ButtonB.x
        var bPresses = (machine.Prize.x - aPresses * machine.ButtonA.x) / machine.ButtonB.x;
        // verify that aPresses and bPresses actually solve the sysetm of equations to take into account rounding errors
        if (aPresses * machine.ButtonA.x + bPresses * machine.ButtonB.x == machine.Prize.x && aPresses * machine.ButtonA.y + bPresses * machine.ButtonB.y == machine.Prize.y)
            return aPresses * 3 + bPresses * 1;

        return 0;
    }

    [GeneratedRegex(@"X\+(\d+), Y\+(\d+)")]
    private static partial Regex ButtonRegex();
    
    [GeneratedRegex(@"X=(\d+), Y=(\d+)")]
    private static partial Regex PrizeRegex();
}
