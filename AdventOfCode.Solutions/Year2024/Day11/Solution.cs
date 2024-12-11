using System.Numerics;
using static System.Formats.Asn1.AsnWriter;

namespace AdventOfCode.Solutions.Year2024.Day11;

class Solution : SolutionBase
{
    public Solution() : base(11, 2024, "") 
    {
        Debug = false;
    }

    protected override string SolvePartOne()
    {
        List<BigInteger> curStones = new List<BigInteger>();
        curStones = Input.Split(' ').Select(BigInteger.Parse).ToList();
        var blinks = 25;
        for(int blink = 0; blink < blinks; blink++)
        {
            curStones = BasicBlink(curStones);
            Console.WriteLine($"Values: {String.Join(" ", curStones)}");
        }

        return curStones.Count.ToString();
    }

    List<BigInteger> BasicBlink(List<BigInteger> curStones)
    {
        List<BigInteger> holdStones = new List<BigInteger>();
        foreach (BigInteger stone in curStones)
        {
            var stoneString = stone.ToString();
            if(stone == 0)
            {
                holdStones.Add(1);
            } 
            else if (stoneString.Length % 2 == 0) // Check if even number of digits
            {
                holdStones.Add(BigInteger.Parse(stoneString[0..(stoneString.Length / 2)]));
                holdStones.Add(BigInteger.Parse(stoneString[(stoneString.Length / 2)..]));
            }
            else
            {
                holdStones.Add(stone * 2024);
            }
        }
        return holdStones;
    }

    protected override string SolvePartTwo()
    {
        List<BigInteger> curStones = new List<BigInteger>();
        curStones = Input.Split(' ').Select(BigInteger.Parse).ToList();
        var blinks = 75;
        for (int blink = 0; blink < blinks; blink++)
        {
            curStones = BasicBlink(curStones);
            Console.WriteLine($"Blink: {blink}");
        }

        return curStones.Count.ToString();
    }

}
