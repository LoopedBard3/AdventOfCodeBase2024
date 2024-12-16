using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2024.Day14;

partial class Solution : SolutionBase
{
    public Solution() : base(14, 2024, "", false) { }

    public class Robot
    {
        public (int x, int y) position;
        public (int x, int y) velocity;
    } 

    protected override string SolvePartOne()
    {
        var gridHeight = (Debug) ? 7 : 103;
        var gridWidth = (Debug) ? 11 : 101;
        var secondsToSimulate = 10000;

        var robots = new List<Robot>();
        foreach (var line in Input.SplitByNewline())
        {
            var match = RobotRegex().Match(line);
            if (!match.Success)
            {
                throw new Exception($"Failed to parse line: {line}");
            }
            var position = (int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
            var velocity = (int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value));
            robots.Add(new Robot { position = position, velocity = velocity });
        }

        foreach (var robot in robots)
        {
            var targetCol = robot.position.Item1 + robot.velocity.Item1 * secondsToSimulate;
            if (targetCol < 0)
            {
                targetCol = targetCol % gridWidth + gridWidth;
            }
            targetCol = targetCol % gridWidth;

            var targetRow = (robot.position.Item2 + robot.velocity.Item2 * secondsToSimulate);
            if (targetRow < 0)
            {
                targetRow = targetRow % gridHeight + gridHeight;
            }
            targetRow = targetRow % gridHeight;
            

            robot.position = (targetCol, targetRow);
        }

        var rowHalf = gridHeight / 2;
        var colHalf = gridWidth / 2;
        var grid0Total = 0;
        var grid1Total = 0;
        var grid2Total = 0;
        var grid3Total = 0;

        foreach (var robot in robots)
        {
            if (robot.position.Item1 < colHalf && robot.position.Item2 < rowHalf)
            {
                grid0Total++;
            }
            else if (robot.position.Item1 < colHalf && robot.position.Item2 > rowHalf)
            {
                grid1Total++;
            }
            else if (robot.position.Item1 > colHalf && robot.position.Item2 < rowHalf)
            {
                grid2Total++;
            }
            else if (robot.position.Item1 > colHalf && robot.position.Item2 > rowHalf)
            {
                grid3Total++;
            }
        }
        var safetyFactor = grid0Total * grid1Total * grid2Total * grid3Total;
        return safetyFactor.ToString();
    }

    protected override string SolvePartTwo() // My magic number is 8159
    {
        var gridHeight = (Debug) ? 7 : 103;
        var gridWidth = (Debug) ? 11 : 101;
        var secondsToSimulate = 10000;

        var robots = new List<Robot>();
        foreach (var line in Input.SplitByNewline())
        {
            var match = RobotRegex().Match(line);
            if (!match.Success)
            {
                throw new Exception($"Failed to parse line: {line}");
            }
            var position = (int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
            var velocity = (int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value));
            robots.Add(new Robot { position = position, velocity = velocity });
        }

        var second = 0;
        for (second = 0; second < secondsToSimulate; second++)
        {
            foreach (var robot in robots)
            {
                var targetCol = robot.position.Item1 + robot.velocity.Item1;
                if (targetCol < 0)
                {
                    targetCol = targetCol % gridWidth + gridWidth;
                }
                targetCol = targetCol % gridWidth;

                var targetRow = robot.position.Item2 + robot.velocity.Item2;
                if (targetRow < 0)
                {
                    targetRow = targetRow % gridHeight + gridHeight;
                }
                targetRow = targetRow % gridHeight;

                robot.position = (targetCol, targetRow);
            }

            //SaveRobotGridToFileAsImage(robots, gridWidth, gridHeight, (second + 1).ToString());
        }

        return second.ToString();
    }

    [GeneratedRegex(@"p=(-?\d+),(-?\d+) v=(-?\d+),(-?\d+)")]
    private static partial Regex RobotRegex();

    private static void PrintRobotGrid(List<Robot> Robots, int gridWidth, int gridHeight)
    {
        var grid = new List<List<char>>();
        for (var row = 0; row < gridHeight; row++)
        {
            var newRow = new List<char>();
            for (var col = 0; col < gridWidth; col++)
            {
                newRow.Add('.');
            }
            grid.Add(newRow);
        }

        foreach (var robot in Robots)
        {
            grid[robot.position.y][robot.position.x] = '#';
        }

        foreach(var row in grid)
        {
            Console.WriteLine(row.JoinAsStrings());
        }

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
    }

    private static void SaveRobotGridToFileAsImage(List<Robot> Robots, int gridWidth, int gridHeight, string fileName)
    {
        if (OperatingSystem.IsWindows())
        {
            // Make a black and white image of where the robots are and save it to a file
            // Setup the image storage type
            var image = new Bitmap(gridWidth, gridHeight);
            for (var row = 0; row < gridHeight; row++)
            {
                for (var col = 0; col < gridWidth; col++)
                {
                    image.SetPixel(col, row, Color.Black);
                }
            }

            foreach (var robot in Robots)
            {
                image.SetPixel(robot.position.x, robot.position.y, Color.White);
            }
            var imageSaveLocation = $"{System.IO.Directory.GetCurrentDirectory()}\\Images\\{fileName}.png";
            image.Save(imageSaveLocation, ImageFormat.Png);
            Console.WriteLine($"Image {fileName} saved to: {imageSaveLocation}");
        }
        else
        {
            throw new PlatformNotSupportedException("Bitmap.SetPixel is only supported on Windows.");
        }
    }
}
