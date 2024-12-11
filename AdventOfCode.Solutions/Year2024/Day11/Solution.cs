using System.Numerics;
using static System.Formats.Asn1.AsnWriter;

namespace AdventOfCode.Solutions.Year2024.Day11;

class Solution : SolutionBase
{
    Dictionary<string, BigInteger> memoTable = new Dictionary<string, BigInteger>();
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
        BigInteger result = 0;
        foreach(BigInteger stone in curStones)
        {
            result += IndividualBlink(stone, blinks);
        }

        return result.ToString();
    }

    BigInteger IndividualBlink(BigInteger startStone, int numberOfBlinks)
    {
        if(numberOfBlinks == 0)
        {
            return 1;
        }

        var stoneString = startStone.ToString();
        if (memoTable.TryGetValue($"{stoneString},{numberOfBlinks}", out BigInteger value))
        {
            return value;
        }

        BigInteger totalStones;
        if (startStone == 0)
        {
            totalStones = IndividualBlink(1, numberOfBlinks - 1);
        }
        else if (stoneString.Length % 2 == 0) // Check if even number of digits
        {
            totalStones = IndividualBlink(BigInteger.Parse(stoneString[0..(stoneString.Length / 2)]), numberOfBlinks - 1);
            totalStones += IndividualBlink(BigInteger.Parse(stoneString[(stoneString.Length / 2)..]), numberOfBlinks - 1);
        }
        else
        {
            totalStones = IndividualBlink(startStone * 2024, numberOfBlinks - 1);
        }
        memoTable.Add($"{stoneString},{numberOfBlinks}", totalStones);
        return totalStones;
    }
}
