namespace AdventOfCode.Solutions.Year2024.Day02;

class Solution : SolutionBase
{
    public Solution() : base(02, 2024, "") {
        Debug = false;
    }

    protected override string SolvePartOne()
    {
        var countSafe = 0;
        foreach (string line in Input.SplitByNewline())
        {
            var isSafeCheck = IsLineSafe(line.Split(" ").Select(int.Parse));
            var isSafe = isSafeCheck == -1;
            if (isSafe)
            {
                countSafe++;
            }
        }
        return countSafe.ToString();
    }

    protected override string SolvePartTwo()
    {
        var countSafe = 0;
        foreach (string line in Input.SplitByNewline())
        {
            int isSafeCheck;
            var lineSplit = line.Split(" ").Select(int.Parse);
            if ((isSafeCheck = IsLineSafe(lineSplit)) == -1)
            {
                Console.WriteLine($"Safe: {line}");
                countSafe++;
                continue;
            }

            // Failed without taking out any items, try again by taking out the "bad" item       
            if (isSafeCheck == 1)
            {
                var lineSplitLeft = lineSplit.Take(isSafeCheck - 1).Concat(lineSplit.Skip(isSafeCheck));
                if (IsLineSafe(lineSplitLeft) == -1)
                {
                    Console.WriteLine($"Safe: {line}");
                    countSafe++;
                    continue;
                }
            }

            var lineSplitCurrent = lineSplit.Take(isSafeCheck).Concat(lineSplit.Skip(isSafeCheck + 1));
            var lineSplitRight = lineSplit.Take(isSafeCheck + 1).Concat(lineSplit.Skip(isSafeCheck + 2));
            if (IsLineSafe(lineSplitCurrent) == -1 || IsLineSafe(lineSplitRight) == -1)
            {
                countSafe++;
                continue;
            }
        }
        return countSafe.ToString();
    }

    static int IsLineSafe(IEnumerable<int> line, int minDiff = 1, int maxDiff = 3)
    {
        var increasing = true;
        if (line.ElementAt(0) - line.ElementAt(1) > 0)
        {
            increasing = false;
        }
        else if (line.ElementAt(0) - line.ElementAt(1) == 0)
        {
            return 0;
        }

        // Check if the line is safe in the correct direction
        var curElementIndex = 0;
        var isSafe = true;
        while (curElementIndex < line.Count() - 1)
        {
            var change = line.ElementAt(curElementIndex) - line.ElementAt(curElementIndex + 1);
            if (increasing)
            {
                change *= -1;
            }
            if (!(change >= minDiff && change <= maxDiff))
            {
                isSafe = false;
                break;
            }
            curElementIndex++;
        }
        if (isSafe)
        {
            return -1;
        }
        else 
        { 
            return curElementIndex; 
        }
    }
}
