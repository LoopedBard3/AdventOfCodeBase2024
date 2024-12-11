using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace AdventOfCode.Solutions.Year2024.Day04;

class Solution : SolutionBase
{
    private List<Tuple<int, int>> XmasDirections = new List<Tuple<int, int>>()
    {
        new Tuple<int, int>(0, 1),  // Up
        new Tuple<int, int>(0, -1), // Down
        new Tuple<int, int>(-1, 0), // Left
        new Tuple<int, int>(1, 0),  // Right
        new Tuple<int, int>(-1, 1),  // Up-Left
        new Tuple<int, int>(1, 1),   // Up-Right
        new Tuple<int, int>(-1, -1), // Down-Left
        new Tuple<int, int>(1, -1)   // Down-Right
    };

    public Solution() : base(04, 2024, "") 
    {
        Debug = false;
    }

    protected override string SolvePartOne()
    {
        var letterGrid = new List<List<Char>>();
        foreach (var line in Input.SplitByNewline())
        {
            letterGrid.Add([.. line]);
        }
        var width = letterGrid[0].Count;
        var height = letterGrid.Count;

        var xmasCount = 0;
        // Iterate over all characters for the char X
        for(int row = 0; row < height; row++)
        {
            for(int col = 0; col < width; col++)
            {
                if (letterGrid[row][col] == 'X')
                {
                    var xmases = CheckDirectionsForXMAS(letterGrid, row, col);
                    xmasCount += xmases;
                }
            }
        }
        return xmasCount.ToString();
    }

    private int CheckDirectionsForXMAS(List<List<Char>> grid, int row, int col)
    {
        var xmases = 0;
        if (grid[row][col] != 'X')
        {
            throw new Exception($"Use CheckDirectionsForXMAS from X locations; {row} {col} is a {grid[row][col]}");
        }
        foreach (var direction in XmasDirections)
        {
            // Make sure the endpoint is in the grid
            var finalRow = direction.Item1 * 3 + row;
            var finalCol = direction.Item2 * 3 + col;
            if(finalRow < 0 || finalCol < 0 || finalRow >= grid.Count || finalCol >= grid[0].Count)
            {
                continue;
            }
            // Check for M, A, and S in the same direction x spots
            // Check M
            if (!(grid[row + direction.Item1][col + direction.Item2] == 'M'))
            {
                continue;
            }
            // Check A
            if (!(grid[row + direction.Item1 * 2][col + direction.Item2 * 2] == 'A'))
            {
                continue;
            }

            // Check S
            if (!(grid[row + direction.Item1 * 3][col + direction.Item2 * 3] == 'S'))
            {
                continue;
            }
            xmases++;
        }
        return xmases;
    }

    protected override string SolvePartTwo()
    {
        var letterGrid = new List<List<Char>>();
        foreach (var line in Input.SplitByNewline())
        {
            letterGrid.Add([.. line]);
        }
        var width = letterGrid[0].Count;
        var height = letterGrid.Count;

        var masCount = 0;
        // Iterate over all characters for the char X
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                if (letterGrid[row][col] == 'A')
                {
                    if(CheckForXMAS(letterGrid, row, col))
                    {
                        masCount++;
                    }
                }
            }
        }
        return masCount.ToString();
    }

    private bool CheckForXMAS(List<List<char>> grid, int row, int col)
    {
        if (grid[row][col] != 'A')
        {
            throw new Exception($"Use CheckForXMAS from A locations; {row} {col} is a {grid[row][col]}");
        }

        // Check to make sure we are 1 away from border
        if(row < 1 || row > grid.Count - 2 || col < 1 || col > grid[0].Count - 2)
        {
            return false;
        }

        // Check top left + bottom right S & M
        if (grid[row - 1][col - 1] == 'S' && grid[row + 1][col + 1] == 'M' || grid[row - 1][col - 1] == 'M' && grid[row + 1][col + 1] == 'S')
        {
            // Check top right + bottom left for S & M
            if (grid[row - 1][col + 1] == 'S' && grid[row + 1][col - 1] == 'M' || grid[row - 1][col + 1] == 'M' && grid[row + 1][col - 1] == 'S')
            {
                return true;
            }
        }

        return false;
    }
}
