using System.Xml.XPath;

namespace AdventOfCode.Solutions.Year2024.Day12;

class Solution : SolutionBase
{
    public Solution() : base(12, 2024, "") 
    {
        Debug = false;
    }

    protected override string SolvePartOne()
    {
        // Load in the grid
        var letterGrid = new List<List<Char>>(); // Character, visited
        var visitedGrid = new List<List<bool>>();
        foreach (var line in Input.SplitByNewline())
        {
            var charListHold = new List<Char>();
            var visitedListhold = new List<bool>();
            foreach(var character in line)
            {
                charListHold.Add(character);
                visitedListhold.Add(false);
            }
            letterGrid.Add(charListHold);
            visitedGrid.Add(visitedListhold);
        }
        var width = letterGrid[0].Count;
        var height = letterGrid.Count;

        // Determine the regions (and the number or regions) (make sure values are set properly, iterate right then down)
        var price = 0;
        for (var row = 0; row < height; row++)
        {
            for (var col = 0; col < width; col++)
            {
                if (!visitedGrid[row][col])
                {
                    var result = CalculateRegion(letterGrid, visitedGrid, row, col);
                    price += result.Item1 * result.Item2;
                    //Console.WriteLine($"Received perimeter {result.Item2} and area {result.Item1} for character {letterGrid[row][col]}");
                }
            }
        }

        return price.ToString();
    }

    private static Tuple<int, int> CalculateRegion(List<List<Char>> charGrid, List<List<bool>> visitedGrid, int row, int col)
    {
        var area = 0;
        var perimeter = 4;
        var character = charGrid[row][col];
        visitedGrid[row][col] = true;

        // Recursively call for all 4 regions
        if (row > 0 && charGrid[row - 1][col] == character) 
        {
            perimeter -= 1;
            if(!visitedGrid[row - 1][col])
            {
                var result = CalculateRegion(charGrid, visitedGrid, row - 1, col);
                area += result.Item1;
                perimeter += result.Item2;
            }
        }
        
        if (row < charGrid[row].Count - 1 && charGrid[row + 1][col] == character) 
        {
            perimeter -= 1;
            if(!visitedGrid[row + 1][col])
            {
                var result = CalculateRegion(charGrid, visitedGrid, row + 1, col);
                area += result.Item1;
                perimeter += result.Item2;
            }
        }
        
        if (col > 0 && charGrid[row][col - 1] == character) 
        {
            perimeter -= 1;
            if(!visitedGrid[row][col - 1])
            {
                var result = CalculateRegion(charGrid, visitedGrid, row, col - 1);
                area += result.Item1;
                perimeter += result.Item2;
            }
        }

        if (col < charGrid.Count - 1 && charGrid[row][col + 1] == character)
        {
            perimeter -= 1;
            if (!visitedGrid[row][col + 1])
            {
                var result = CalculateRegion(charGrid, visitedGrid, row, col + 1);
                area += result.Item1;
                perimeter += result.Item2;
            }
        }

        return new Tuple<int, int>(area + 1, perimeter);
    }

    protected override string SolvePartTwo()
    {
        // Load in the grid
        var letterGrid = new List<List<Char>>(); // Character
        var visitedGrid = new List<List<bool>>();
        foreach (var line in Input.SplitByNewline())
        {
            var charListHold = new List<Char>();
            var visitedListhold = new List<bool>();
            foreach (var character in line)
            {
                charListHold.Add(character);
                visitedListhold.Add(false);
            }
            letterGrid.Add(charListHold);
            visitedGrid.Add(visitedListhold);
        }
        var width = letterGrid[0].Count;
        var height = letterGrid.Count;

        // Determine the regions (and the number or regions) (make sure values are set properly, iterate right then down)
        var price = 0;
        for (var row = 0; row < height; row++)
        {
            for (var col = 0; col < width; col++)
            {
                if (!visitedGrid[row][col])
                {
                    var result = CalculateRegionWithSides(letterGrid, visitedGrid, row, col);
                    price += result.Item1 * result.Item2;
                    Console.WriteLine($"Received area {result.Item1} and sides {result.Item2} for character {letterGrid[row][col]}");
                }
            }
        }

        return price.ToString();
    }

    private static Tuple<int, int> CalculateRegionWithSides(List<List<Char>> charGrid, List<List<bool>> visitedGrid, int row, int col)
    {
        var area = 0;
        var sides = 0;
        var character = charGrid[row][col];
        visitedGrid[row][col] = true;

        // Calculate the number of corners/sides
        // Cover the 4 primary corners of the grid
        if (row == 0 && col == 0 || row == 0 && col == charGrid[0].Count - 1 || row == charGrid.Count - 1 && col == 0 || row == charGrid.Count - 1 && col == charGrid[0].Count - 1)
        {
            sides += 1;

            // Check to see if any of the other perimeters are sides
            if (row == 0)
            {
                if (col == 0)
                {
                    // Check right char and down char
                    if (charGrid[0][1] != character)
                    {
                        sides += 1;
                    }
                    if (charGrid[1][0] != character)
                    {
                        sides += 1;
                    }

                } else if (col == charGrid.Count - 1)
                {
                    // Check left char and down char
                    if (charGrid[0][charGrid.Count - 2] != character)
                    {
                        sides += 1;
                    }
                    if (charGrid[1][charGrid.Count - 1] != character)
                    {
                        sides += 1;
                    }
                }
            }
            else if (row ==  charGrid.Count - 1)
            {
                if (col == 0)
                {
                    // Check right char and up char
                    if (charGrid[charGrid.Count - 1][1] != character)
                    {
                        sides += 1;
                    }
                    if (charGrid[charGrid.Count - 2][0] != character)
                    {
                        sides += 1;
                    }

                }
                else if (col == charGrid.Count - 1)
                {
                    // Check left char and up char
                    if (charGrid[charGrid.Count - 1][charGrid.Count - 2] != character)
                    {
                        sides += 1;
                    }
                    if (charGrid[charGrid.Count - 2][charGrid.Count - 1] != character)
                    {
                        sides += 1;
                    }
                }
            }

        }

        // top left (both up and left are same or different)
        if (row > 0 && col > 0 && ((charGrid[row - 1][col] == character && charGrid[row][col - 1] == character && charGrid[row - 1][col - 1] != character) || (charGrid[row - 1][col] != character && charGrid[row][col - 1] != character)))
        {
            sides += 1;
        }

        // top right
        if (row > 0 && col < charGrid[0].Count - 1 && ((charGrid[row - 1][col] == character && charGrid[row][col + 1] == character && charGrid[row - 1][col + 1] != character) || (charGrid[row - 1][col] != character && charGrid[row][col + 1] != character)))
        {
            sides += 1;
        }

        // bottom left
        if (row < charGrid.Count - 1 && col > 0 && ((charGrid[row + 1][col] == character && charGrid[row][col - 1] == character && charGrid[row + 1][col - 1] != character) || (charGrid[row + 1][col] != character && charGrid[row][col - 1] != character)))
        {
            sides += 1;
        }

        // bottom right
        if (row < charGrid.Count - 1 && col < charGrid[0].Count - 1 && ((charGrid[row + 1][col] == character && charGrid[row][col + 1] == character && charGrid[row + 1][col + 1] != character) || (charGrid[row + 1][col] != character && charGrid[row][col + 1] != character)))
        {
            sides += 1;
        }

        // or check for against edges
        if((row == 0 || row == charGrid.Count - 1) && col > 0 && col < charGrid[row].Count - 1) 
        {
            // single side
            if(charGrid[row][col - 1] != charGrid[row][col + 1] && (charGrid[row][col - 1] == character || charGrid[row][col + 1] == character))
            {
                sides += 1;
            } 
            else if (charGrid[row][col - 1] != character && charGrid[row][col + 1] != character)
            {
                sides += 2;
            }
        }
        
        if((col == 0 || col == charGrid[0].Count - 1) && row > 0 && row < charGrid.Count - 1) 
        {
            // single side
            if (charGrid[row - 1][col] != charGrid[row + 1][col] && (charGrid[row - 1][col] == character || charGrid[row + 1][col] == character))
            {
                sides += 1;
            }
            else if (charGrid[row - 1][col] != character && charGrid[row + 1][col] != character)
            {
                sides += 2;
            }
        }

        // Recursively call for all 4 regions
        if (row > 0 && charGrid[row - 1][col] == character)
        {
            if (!visitedGrid[row - 1][col])
            {
                var result = CalculateRegionWithSides(charGrid, visitedGrid, row - 1, col);
                area += result.Item1;
                sides += result.Item2;
            }
        }

        if (row < charGrid[row].Count - 1 && charGrid[row + 1][col] == character)
        {
            if (!visitedGrid[row + 1][col])
            {
                var result = CalculateRegionWithSides(charGrid, visitedGrid, row + 1, col);
                area += result.Item1;
                sides += result.Item2;
            }
        }

        if (col > 0 && charGrid[row][col - 1] == character)
        {
            if (!visitedGrid[row][col - 1])
            {
                var result = CalculateRegionWithSides(charGrid, visitedGrid, row, col - 1);
                area += result.Item1;
                sides += result.Item2;
            }
        }

        if (col < charGrid.Count - 1 && charGrid[row][col + 1] == character)
        {
            if (!visitedGrid[row][col + 1])
            {
                var result = CalculateRegionWithSides(charGrid, visitedGrid, row, col + 1);
                area += result.Item1;
                sides += result.Item2;
            }
        }

        return new Tuple<int, int>(area + 1, sides);
    }
}
