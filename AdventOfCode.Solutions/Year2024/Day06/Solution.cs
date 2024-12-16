using System.Reflection.Metadata.Ecma335;

namespace AdventOfCode.Solutions.Year2024.Day06;

class Solution : SolutionBase
{
    public Solution() : base(06, 2024, "")
    {
        Debug = false;
    }

    enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }

    protected override string SolvePartOne()
    {
        // Read in the map
        var map = new List<List<char>>();
        var guard = (-1, -1);
        foreach (var line in Input.SplitByNewline())
        {
            map.Add([.. line]);
            if (line.Contains('^'))
            {
                guard = (map.Count - 1, line.IndexOf('^'));
            }
        }

        if (guard == (-1, -1))
        {
            throw new Exception("Guard not found");
        }

        // Set the guard's starting position to an X
        map[guard.Item1][guard.Item2] = 'X';

        // While the guard is still on the map, move the guard, turning right if a box is hit, marking the spots they've been with an X
        var direction = Direction.Up;
        while (MoveGuard(ref guard, ref direction, map)) { }

        // Count the number of spots (X's) that have been marked
        var spotsVisited = 0;
        for (var row = 0; row < map.Count; row++)
        {
            for (var col = 0; col < map[0].Count; col++)
            {
                if (map[row][col] == 'X')
                {
                    spotsVisited++;
                }
            }
        }

        return spotsVisited.ToString();
    }

    private static bool MoveGuard(ref (int, int) guard, ref Direction direction, List<List<char>> map)
    {
        // If the guard can move in its current direction, move it, if it is a box, turn right
        var (row, col) = guard;
        if (IsEdgeOfMap(row, col, direction, map))
        {
            map[row][col] = 'X';
            return false;
        }

        if (direction == Direction.Up)
        {
            if (map[row - 1][col] == '#')
            {
                direction = Direction.Right;
            }
            else
            {
                map[row - 1][col] = 'X';
                guard = (row - 1, col);
            }
        }
        else if (direction == Direction.Right)
        {
            if (map[row][col + 1] == '#')
            {
                direction = Direction.Down;
            }
            else
            {
                map[row][col + 1] = 'X';
                guard = (row, col + 1);
            }
        }
        else if (direction == Direction.Down)
        {
            if (map[row + 1][col] == '#')
            {
                direction = Direction.Left;
            }
            else
            {
                map[row + 1][col] = 'X';
                guard = (row + 1, col);
            }
        }
        else if (direction == Direction.Left)
        {
            if (map[row][col - 1] == '#')
            {
                direction = Direction.Up;
            }
            else
            {
                map[row][col - 1] = 'X';
                guard = (row, col - 1);
            }
        }

        return true;
    }

    private static bool IsEdgeOfMap(int row, int col, Direction direction, List<List<char>> map)
    {
        return (direction == Direction.Up && row == 0) ||
               (direction == Direction.Right && col == map[0].Count - 1) ||
               (direction == Direction.Down && row == map.Count - 1) ||
               (direction == Direction.Left && col == 0);
    }

    protected override string SolvePartTwo()
    {
        // Read in the map
        var map = new List<List<char>>();
        var guard = (-1, -1);
        foreach (var line in Input.SplitByNewline())
        {
            map.Add([.. line]);
            if (line.Contains('^'))
            {
                guard = (map.Count - 1, line.IndexOf('^'));
            }
        }

        if (guard == (-1, -1))
        {
            throw new Exception("Guard not found");
        }

        // Get the possible locations of looping
        var mapCopy = map.Select(row => row.ToList()).ToList();
        // Make a copy of the guards starting location
        var crosses = GetPotentialSpotsForObstacle(guard, mapCopy).Distinct().ToList();

        // Check each possible double cross location
        var result = 0;
        //var iter = 1;
        foreach(var cross in crosses)
        {
            if (cross.Item1 == guard.Item1 && cross.Item2 == guard.Item2)
            {
                continue;
            }
            if (CheckForLoop(cross, guard, map))
            {
                result++;
            }
            // Revert the map from the cross change
            map[cross.Item1][cross.Item2] = '.';
        }

        return result.ToString();
    }

    private bool CheckForLoop((int, int) cross, (int, int) guard, List<List<char>> map)
    {
        // Check for a loop by moving until the guard reaches the cross again
        // then start tracking the guards movements again, if the guard reaches the cross again, we have a loop
        var direction = Direction.Up;

        // Put a O at the cross
        // Make sure the point is not the guard or already an obstacle
        if (map[cross.Item1][cross.Item2] == '#')
        {
            return false;
        }
        map[cross.Item1][cross.Item2] = 'O';
        
        var hasHitCross = false;
        var visitedLocations = new List<(int, int, Direction)>();
        while (true)
        {
            // If the guard can move in its current direction, move it, if it is a box, turn right
            var (row, col) = guard;
            if (IsEdgeOfMap(row, col, direction, map))
            {
                map[row][col] = 'X';
                return false;
            }

            if (direction == Direction.Up)
            {
                if (map[row - 1][col] == '#')
                {
                    direction = Direction.Right;
                }
                else if (map[row - 1][col] == 'O')
                {
                    direction = Direction.Right;
                    hasHitCross = true;
                }
                else
                {
                    map[row - 1][col] = 'X';
                    guard = (row - 1, col);
                }
            }
            else if (direction == Direction.Right)
            {
                if (map[row][col + 1] == '#')
                {
                    direction = Direction.Down;
                }
                else if (map[row][col + 1] == 'O')
                {
                    direction = Direction.Down;
                    hasHitCross = true;
                }
                else
                {
                    map[row][col + 1] = 'X';
                    guard = (row, col + 1);
                }
            }
            else if (direction == Direction.Down)
            {
                if (map[row + 1][col] == '#')
                {
                    direction = Direction.Left;
                }
                else if(map[row + 1][col] == 'O')
                {
                    direction = Direction.Left;
                    hasHitCross = true;
                }
                else
                {
                    map[row + 1][col] = 'X';
                    guard = (row + 1, col);
                }
            }
            else if (direction == Direction.Left)
            {
                if (map[row][col - 1] == '#')
                {
                    direction = Direction.Up;
                }
                else if (map[row][col - 1] == 'O')
                {
                    direction = Direction.Up;
                    hasHitCross = true;
                }
                else
                {
                    map[row][col - 1] = 'X';
                    guard = (row, col - 1);
                }
            }

            if (hasHitCross)
            {
                if (visitedLocations.Contains((guard.Item1, guard.Item2, direction)))
                {
                    return true;
                }
                // start saving locations and directions to see if we loop
                visitedLocations.Add((guard.Item1, guard.Item2, direction));
            }
        }
    }

    private List<(int, int)> GetPotentialSpotsForObstacle((int, int) guard, List<List<char>> map)
    {
        // Set the guard's starting position to an X
        map[guard.Item1][guard.Item2] = 'X';
        var direction = Direction.Up;
        var crosses = new List<(int, int)>();
        while (true)
        {
            // If the guard can move in its current direction, move it, if it is a box, turn right
            var (row, col) = guard;
            if (IsEdgeOfMap(row, col, direction, map))
            {
                map[row][col] = 'X';
                return crosses;
            }

            if (direction == Direction.Up)
            {
                if (map[row - 1][col] == '#')
                {
                    direction = Direction.Right;
                    if (col == map[0].Count) continue;
                    for (int i = row + 1; i < map.Count; i++)
                    {
                        if (map[i][col] == '#')
                        {
                            crosses.Add((row, col + 1));
                        }
                    }
                }
                else
                {
                    for (int i = col + 1; i < map[0].Count; i++)
                    {
                        if (map[row][i] == '#')
                        {
                            crosses.Add((row - 1, col));
                        }
                    }

                    map[row - 1][col] = 'X';
                    guard = (row - 1, col);
                }
            }
            else if (direction == Direction.Right)
            {
                if (map[row][col + 1] == '#')
                {
                    direction = Direction.Down;
                    if (row == map.Count) continue;
                    for (int i = col - 1; i >= 0; i--)
                    {
                        if (map[row][i] == '#')
                        {
                            crosses.Add((row + 1, col));
                        }
                    }
                }
                else
                {
                    for (int i = row + 1; i < map.Count; i++)
                    {
                        if (map[i][col] == '#')
                        {
                            crosses.Add((row, col + 1));
                        }
                    }

                    map[row][col + 1] = 'X';
                    guard = (row, col + 1);
                }
            }
            else if (direction == Direction.Down)
            {
                if (map[row + 1][col] == '#')
                {
                    direction = Direction.Left;
                    if (col == 0) continue;
                    for (int i = row - 1; i >= 0; i--)
                    {
                        if (map[i][col] == '#')
                        {
                            crosses.Add((row, col - 1));
                        }
                    }
                }
                else
                {
                    for (int i = col - 1; i >= 0; i--)
                    {
                        if (map[row][i] == '#')
                        {
                            crosses.Add((row + 1, col));
                        }
                    }
                    map[row + 1][col] = 'X';
                    guard = (row + 1, col);
                }
            }
            else if (direction == Direction.Left)
            {
                if (map[row][col - 1] == '#')
                {
                    direction = Direction.Up;
                    if (row == 0) continue;
                    for (int i = col + 1; i < map[0].Count; i++)
                    {
                        if (map[row][i] == '#')
                        {
                            crosses.Add((row - 1, col));
                        }
                    }
                }
                else
                {
                    for (int i = row - 1; i >= 0; i--)
                    {
                        if (map[i][col] == '#')
                        {
                            crosses.Add((row, col - 1));
                        }
                    }
                    map[row][col - 1] = 'X';
                    guard = (row, col - 1);
                }
            }
        }
    }
}
