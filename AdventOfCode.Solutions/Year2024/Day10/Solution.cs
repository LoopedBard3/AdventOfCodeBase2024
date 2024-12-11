
namespace AdventOfCode.Solutions.Year2024.Day10;

class Solution : SolutionBase
{
    public Solution() : base(10, 2024, "") 
    {
        Debug = false;
    }

    protected override string SolvePartOne()
    {
        var numberGrid = new List<List<int>>();
        foreach (var line in Input.SplitByNewline())
        {
            var lineList = new List<int>();
            foreach(var number in line)
            {
                lineList.Add(number - 48);
            }
            numberGrid.Add(lineList);
        }
        var width = numberGrid[0].Count;
        var height = numberGrid.Count;

        var trailheadSum = 0;
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                if (numberGrid[row][col] == 0)
                {
                    var trailheads = CheckDirectionsForNextHeight(numberGrid, row, col, numberGrid[row][col]);
                    trailheadSum += trailheads.Distinct().Count();
                }
            }
        }
        return trailheadSum.ToString();
    }

    private List<Tuple<int, int>> CheckDirectionsForNextHeight(List<List<int>> numberGrid, int row, int col, int curHeight)
    {
        if(curHeight == 9)
        {
            return [new(row, col)];
        }

        List<Tuple<int, int>> trailheads = [];
        // Check each of the four cardinal directions for next number and call if valid
        if (row > 0 && numberGrid[row - 1][col] == curHeight + 1)
        {
            trailheads.AddRange(CheckDirectionsForNextHeight(numberGrid, row - 1, col, curHeight + 1));
        } 
        if (row < numberGrid.Count - 1 && numberGrid[row + 1][col] == curHeight + 1)
        {
            trailheads.AddRange(CheckDirectionsForNextHeight(numberGrid, row + 1, col, curHeight + 1));
        }
       if (col > 0 && numberGrid[row][col - 1] == curHeight + 1)
        {
            trailheads.AddRange(CheckDirectionsForNextHeight(numberGrid, row, col - 1, curHeight + 1));
        }
        if (col < numberGrid[0].Count - 1 && numberGrid[row][col + 1] == curHeight + 1)
        {
            trailheads.AddRange(CheckDirectionsForNextHeight(numberGrid, row, col + 1, curHeight + 1));
        }

        return trailheads;
    }

    protected override string SolvePartTwo()
    {
        var numberGrid = new List<List<int>>();
        foreach (var line in Input.SplitByNewline())
        {
            var lineList = new List<int>();
            foreach (var number in line)
            {
                lineList.Add(number - 48);
            }
            numberGrid.Add(lineList);
        }
        var width = numberGrid[0].Count;
        var height = numberGrid.Count;

        var trailheadSum = 0;
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                if (numberGrid[row][col] == 0)
                {
                    var trailheads = CheckDirectionsForNextHeight(numberGrid, row, col, numberGrid[row][col]);
                    trailheadSum += trailheads.Count();
                }
            }
        }
        return trailheadSum.ToString();
    }
}
