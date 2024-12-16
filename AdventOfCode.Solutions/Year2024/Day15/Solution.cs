
namespace AdventOfCode.Solutions.Year2024.Day15;

class Solution : SolutionBase
{
    public Solution() : base(15, 2024, "", true) { }

    protected override string SolvePartOne()
    {
        var input = Input.SplitByParagraph();
        var map = new List<List<char>>();
        var moves = new List<char>();
        var robotLocation = (0, 0);

        foreach (var line in input[0].SplitByNewline())
        {
            map.Add([.. line]);
            if (line.Contains('@'))
            {
                robotLocation = (map.Count - 1, line.IndexOf('@'));
            }
        }

        foreach (var line in input[1].SplitByNewline())
        {
            moves.AddRange([.. line]);
        }

        foreach (var move in moves)
        {
            TryMove(map, ref robotLocation, move);
        }

        // iterate over the map and get the GPS coordinate sum of all the boxes
        var sum = 0;
        for (int i = 0; i < map.Count; i++)
        {
            for (int j = 0; j < map[i].Count; j++)
            {
                if (map[i][j] == 'O')
                {
                    sum += i * 100 + j;
                }
            }
        }

        // Print the final map
        foreach (var line in map)
        {
            Console.WriteLine(line.JoinAsStrings());
        }

        return sum.ToString();
    }

    private void TryMove(List<List<char>> map, ref (int, int) robotLocation, char move)
    {
        if (move == '^')
        {
            if (map[robotLocation.Item1 - 1][robotLocation.Item2] == '.')
            {
                map[robotLocation.Item1 - 1][robotLocation.Item2] = '@';
                map[robotLocation.Item1][robotLocation.Item2] = '.';
                robotLocation = (robotLocation.Item1 - 1, robotLocation.Item2);
            }
            else if (map[robotLocation.Item1 - 1][robotLocation.Item2] == 'O')
            {
                // Check if we can push the box(s)
                var locationToCheck = (robotLocation.Item1 - 1, robotLocation.Item2);
                while (map[locationToCheck.Item1][locationToCheck.Item2] == 'O')
                {
                    locationToCheck = (locationToCheck.Item1 - 1, locationToCheck.Item2);
                }

                if (map[locationToCheck.Item1][locationToCheck.Item2] == '.')
                {
                    map[locationToCheck.Item1][locationToCheck.Item2] = 'O';
                    map[robotLocation.Item1 - 1][robotLocation.Item2] = '@';
                    map[robotLocation.Item1][robotLocation.Item2] = '.';
                    robotLocation = (robotLocation.Item1 - 1, robotLocation.Item2);
                }
                else if (map[locationToCheck.Item1][locationToCheck.Item2] == '#')
                {
                    return;
                }
            }
            else if (map[robotLocation.Item1 - 1][robotLocation.Item2] == '#')
            {
                return;
            }
        }
        else if (move == 'v')
        {
            if (map[robotLocation.Item1 + 1][robotLocation.Item2] == '.')
            {
                map[robotLocation.Item1 + 1][robotLocation.Item2] = '@';
                map[robotLocation.Item1][robotLocation.Item2] = '.';
                robotLocation = (robotLocation.Item1 + 1, robotLocation.Item2);
            }
            else if (map[robotLocation.Item1 + 1][robotLocation.Item2] == 'O')
            {
                // Check if we can push the box(s)
                var locationToCheck = (robotLocation.Item1 + 1, robotLocation.Item2);
                while (map[locationToCheck.Item1][locationToCheck.Item2] == 'O')
                {
                    locationToCheck = (locationToCheck.Item1 + 1, locationToCheck.Item2);
                }

                if (map[locationToCheck.Item1][locationToCheck.Item2] == '.')
                {
                    map[locationToCheck.Item1][locationToCheck.Item2] = 'O';
                    map[robotLocation.Item1 + 1][robotLocation.Item2] = '@';
                    map[robotLocation.Item1][robotLocation.Item2] = '.';
                    robotLocation = (robotLocation.Item1 + 1, robotLocation.Item2);
                }
                else if (map[locationToCheck.Item1][locationToCheck.Item2] == '#')
                {
                    return;
                }
            }
            else if (map[robotLocation.Item1 + 1][robotLocation.Item2] == '#')
            {
                return;
            }
        }
        else if (move == '<')
        {
            if (map[robotLocation.Item1][robotLocation.Item2 - 1] == '.')
            {
                map[robotLocation.Item1][robotLocation.Item2 - 1] = '@';
                map[robotLocation.Item1][robotLocation.Item2] = '.';
                robotLocation = (robotLocation.Item1, robotLocation.Item2 - 1);
            }
            else if (map[robotLocation.Item1][robotLocation.Item2 - 1] == 'O')
            {
                // Check if we can push the box(s)
                var locationToCheck = (robotLocation.Item1, robotLocation.Item2 - 1);
                while (map[locationToCheck.Item1][locationToCheck.Item2] == 'O')
                {
                    locationToCheck = (locationToCheck.Item1, locationToCheck.Item2 - 1);
                }

                if (map[locationToCheck.Item1][locationToCheck.Item2] == '.')
                {
                    map[locationToCheck.Item1][locationToCheck.Item2] = 'O';
                    map[robotLocation.Item1][robotLocation.Item2 - 1] = '@';
                    map[robotLocation.Item1][robotLocation.Item2] = '.';
                    robotLocation = (robotLocation.Item1, robotLocation.Item2 - 1);
                }
                else if (map[locationToCheck.Item1][locationToCheck.Item2] == '#')
                {
                    return;
                }

            }
            else if (map[robotLocation.Item1][robotLocation.Item2 - 1] == '#')
            {
                return;
            }
        }
        else if (move == '>')
        {
            if (map[robotLocation.Item1][robotLocation.Item2 + 1] == '.')
            {
                map[robotLocation.Item1][robotLocation.Item2 + 1] = '@';
                map[robotLocation.Item1][robotLocation.Item2] = '.';
                robotLocation = (robotLocation.Item1, robotLocation.Item2 + 1);
            }
            else if (map[robotLocation.Item1][robotLocation.Item2 + 1] == 'O')
            {
                // Check if we can push the box(s)
                var locationToCheck = (robotLocation.Item1, robotLocation.Item2 + 1);
                while (map[locationToCheck.Item1][locationToCheck.Item2] == 'O')
                {
                    locationToCheck = (locationToCheck.Item1, locationToCheck.Item2 + 1);
                }

                if (map[locationToCheck.Item1][locationToCheck.Item2] == '.')
                {
                    map[locationToCheck.Item1][locationToCheck.Item2] = 'O';
                    map[robotLocation.Item1][robotLocation.Item2 + 1] = '@';
                    map[robotLocation.Item1][robotLocation.Item2] = '.';
                    robotLocation = (robotLocation.Item1, robotLocation.Item2 + 1);
                }
                else if (map[locationToCheck.Item1][locationToCheck.Item2] == '#')
                {
                    return;
                }

            }
            else if (map[robotLocation.Item1][robotLocation.Item2 + 1] == '#')
            {
                return;
            }
        }
    }

    protected override string SolvePartTwo()
    {
        var input = Input.SplitByParagraph();
        var map = new List<List<char>>();
        var moves = new List<char>();
        var robotLocation = (0, 0);

        foreach (var line in input[0].SplitByNewline())
        {
            map.Add([.. line]);
            if (line.Contains('@'))
            {
                robotLocation = (map.Count - 1, line.IndexOf('@'));
            }
        }

        foreach (var line in input[1].SplitByNewline())
        {
            moves.AddRange([.. line]);
        }

        foreach (var move in moves)
        {
            TryMoveBigBoxes(map, ref robotLocation, move);
        }

        // iterate over the map and get the GPS coordinate sum of all the boxes
        var sum = 0;
        for (int i = 0; i < map.Count; i++)
        {
            for (int j = 0; j < map[i].Count; j++)
            {
                if (map[i][j] == 'O')
                {
                    sum += i * 100 + j;
                }
            }
        }

        // Print the final map
        foreach (var line in map)
        {
            Console.WriteLine(line.JoinAsStrings());
        }

        return sum.ToString();
    }

    private void TryMoveBigBoxes(List<List<char>> map, ref (int, int) robotLocation, char move)
    {
        if (move == '^')
        {
            if (map[robotLocation.Item1 - 1][robotLocation.Item2] == '.')
            {
                map[robotLocation.Item1 - 1][robotLocation.Item2] = '@';
                map[robotLocation.Item1][robotLocation.Item2] = '.';
                robotLocation = (robotLocation.Item1 - 1, robotLocation.Item2);
            }
            else if (map[robotLocation.Item1 - 1][robotLocation.Item2] == 'O')
            {
                // Check if we can push the box(s)
                var locationToCheck = (robotLocation.Item1 - 1, robotLocation.Item2);
                while (map[locationToCheck.Item1][locationToCheck.Item2] == 'O')
                {
                    locationToCheck = (locationToCheck.Item1 - 1, locationToCheck.Item2);
                }

                if (map[locationToCheck.Item1][locationToCheck.Item2] == '.')
                {
                    map[locationToCheck.Item1][locationToCheck.Item2] = 'O';
                    map[robotLocation.Item1 - 1][robotLocation.Item2] = '@';
                    map[robotLocation.Item1][robotLocation.Item2] = '.';
                    robotLocation = (robotLocation.Item1 - 1, robotLocation.Item2);
                }
                else if (map[locationToCheck.Item1][locationToCheck.Item2] == '#')
                {
                    return;
                }
            }
            else if (map[robotLocation.Item1 - 1][robotLocation.Item2] == '#')
            {
                return;
            }
        }
        else if (move == 'v')
        {
            if (map[robotLocation.Item1 + 1][robotLocation.Item2] == '.')
            {
                map[robotLocation.Item1 + 1][robotLocation.Item2] = '@';
                map[robotLocation.Item1][robotLocation.Item2] = '.';
                robotLocation = (robotLocation.Item1 + 1, robotLocation.Item2);
            }
            else if (map[robotLocation.Item1 + 1][robotLocation.Item2] == 'O')
            {
                // Check if we can push the box(s)
                var locationToCheck = (robotLocation.Item1 + 1, robotLocation.Item2);
                while (map[locationToCheck.Item1][locationToCheck.Item2] == 'O')
                {
                    locationToCheck = (locationToCheck.Item1 + 1, locationToCheck.Item2);
                }

                if (map[locationToCheck.Item1][locationToCheck.Item2] == '.')
                {
                    map[locationToCheck.Item1][locationToCheck.Item2] = 'O';
                    map[robotLocation.Item1 + 1][robotLocation.Item2] = '@';
                    map[robotLocation.Item1][robotLocation.Item2] = '.';
                    robotLocation = (robotLocation.Item1 + 1, robotLocation.Item2);
                }
                else if (map[locationToCheck.Item1][locationToCheck.Item2] == '#')
                {
                    return;
                }
            }
            else if (map[robotLocation.Item1 + 1][robotLocation.Item2] == '#')
            {
                return;
            }
        }
        else if (move == '<')
        {
            if (map[robotLocation.Item1][robotLocation.Item2 - 1] == '.')
            {
                map[robotLocation.Item1][robotLocation.Item2 - 1] = '@';
                map[robotLocation.Item1][robotLocation.Item2] = '.';
                robotLocation = (robotLocation.Item1, robotLocation.Item2 - 1);
            }
            else if (map[robotLocation.Item1][robotLocation.Item2 - 1] == ']')
            {
                // Check if we can push the box(s)
                var locationToCheck = (robotLocation.Item1, robotLocation.Item2 - 1);
                while (map[locationToCheck.Item1][locationToCheck.Item2] == '[' || map[locationToCheck.Item1][locationToCheck.Item2] == ']')
                {
                    locationToCheck = (locationToCheck.Item1, locationToCheck.Item2 - 1);
                }

                if (map[locationToCheck.Item1][locationToCheck.Item2] == '.')
                {
                    locationToCheck = (robotLocation.Item1, robotLocation.Item2 - 1); // We are able to push the box
                    while (map[locationToCheck.Item1][locationToCheck.Item2] != '.')
                    {
                        // Flip the box symbol
                        if (map[locationToCheck.Item1][locationToCheck.Item2] == '[')
                        {
                            map[locationToCheck.Item1][locationToCheck.Item2] = ']';
                        }
                        else
                        {
                            map[locationToCheck.Item1][locationToCheck.Item2] = '[';
                        }
                        locationToCheck = (locationToCheck.Item1, locationToCheck.Item2 - 1);
                    }
                    map[robotLocation.Item1][robotLocation.Item2 - 1] = '@';
                    map[robotLocation.Item1][robotLocation.Item2] = '.';
                    robotLocation = (robotLocation.Item1, robotLocation.Item2 - 1);
                }
                else if (map[locationToCheck.Item1][locationToCheck.Item2] == '#')
                {
                    return;
                }
            }
            else if (map[robotLocation.Item1][robotLocation.Item2 - 1] == '#')
            {
                return;
            }
        }
        else if (move == '>')
        {
            if (map[robotLocation.Item1][robotLocation.Item2 + 1] == '.')
            {
                map[robotLocation.Item1][robotLocation.Item2 + 1] = '@';
                map[robotLocation.Item1][robotLocation.Item2] = '.';
                robotLocation = (robotLocation.Item1, robotLocation.Item2 + 1);
            }
            else if (map[robotLocation.Item1][robotLocation.Item2 + 1] == '[')
            {
                // Check if we can push the box(s)
                var locationToCheck = (robotLocation.Item1, robotLocation.Item2 + 1);
                while (map[locationToCheck.Item1][locationToCheck.Item2] == '[' || map[locationToCheck.Item1][locationToCheck.Item2] == ']')
                {
                    locationToCheck = (locationToCheck.Item1, locationToCheck.Item2 + 1);
                }

                if (map[locationToCheck.Item1][locationToCheck.Item2] == '.')
                {
                    locationToCheck = (robotLocation.Item1, robotLocation.Item2 + 1); // We are able to push the box
                    while (map[locationToCheck.Item1][locationToCheck.Item2] != '.')
                    {
                        // Flip the box symbol
                        if (map[locationToCheck.Item1][locationToCheck.Item2] == '[')
                        {
                            map[locationToCheck.Item1][locationToCheck.Item2] = ']';
                        }
                        else
                        {
                            map[locationToCheck.Item1][locationToCheck.Item2] = '[';
                        }
                        locationToCheck = (locationToCheck.Item1, locationToCheck.Item2 + 1);
                    }
                    map[robotLocation.Item1][robotLocation.Item2 + 1] = '@';
                    map[robotLocation.Item1][robotLocation.Item2] = '.';
                    robotLocation = (robotLocation.Item1, robotLocation.Item2 - 1);
                }
                else if (map[locationToCheck.Item1][locationToCheck.Item2] == '#')
                {
                    return;
                }
            }
            else if (map[robotLocation.Item1][robotLocation.Item2 + 1] == '#')
            {
                return;
            }
        }
    }
}
