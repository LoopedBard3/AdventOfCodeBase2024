using System.Globalization;

namespace AdventOfCode.Solutions.Year2024.Day16;

class Solution : SolutionBase
{
    public Solution() : base(16, 2024, "", true) { }

    enum Direction
    {
        North,
        East,
        South,
        West
    }

    protected override string SolvePartOne()
    {
        var map = new List<List<char>>();
        var scores = new List<List<int>>();
        var startTile = (0, 0);
        var endTile = (0, 0);
        var rowCount = 0;

        foreach (var line in Input.SplitByNewline())
        {
            map.Add([.. line]);
            scores.Add([]);
            for (int i = 0; i < line.Length; i++)
            {
                scores.Last().Add(int.MaxValue);
            }

            if (line.Contains('S'))
            {
                startTile = (rowCount, line.IndexOf('S'));
            }

            if (line.Contains('E'))
            {
                endTile = (rowCount, line.IndexOf('E'));
            }

            rowCount++;
        }
        var result = DFS(map, scores, startTile, endTile);

        return result.ToString();
    }

    // Depth-first search with scores, we need to add 1 score per tile moved and 1000 for each turn
    // We need to keep track of the direction we are moving in
    public int DFS(List<List<char>> map, List<List<int>> scores, (int, int) startTile, (int, int) endTile)
    {
        var priorityQueue = new SortedSet<(int score, int row, int col, Direction direction)>
        {
            (0, startTile.Item1, startTile.Item2, Direction.East)
        };

        while (priorityQueue.Count > 0)
        {
            var (score, row, col, direction) = priorityQueue.Min;
            priorityQueue.Remove(priorityQueue.Min);

            if (row < 0 || row >= map.Count || col < 0 || col >= map[0].Count || map[row][col] == '#')
            {
                continue;
            }
            if (row == endTile.Item1 && col == endTile.Item2)
            {
                return score;
            }
            if (score >= scores[row][col])
            {
                continue;
            }

            //Console.WriteLine($"Row: {row}, Col: {col}, Score: {score}, Direction: {direction}");
            //foreach (var line in map)
            //{
            //    Console.WriteLine(line.JoinAsStrings());
            //}
            //Console.WriteLine();
            //Console.WriteLine();
            // Sleep for 500 ms
            //Thread.Sleep(500);

            scores[row][col] = score;
            if (direction == Direction.North)
            {
                map[row][col] = '^';
            }
            else if (direction == Direction.East)
            {
                map[row][col] = '>';
            }
            else if (direction == Direction.South)
            {
                map[row][col] = 'v';
            }
            else if (direction == Direction.West)
            {
                map[row][col] = '<';
            }

            priorityQueue.Add((score + ((direction == Direction.South) ? 1 : 1001), row + 1, col, Direction.South));
            priorityQueue.Add((score + ((direction == Direction.North) ? 1 : 1001), row - 1, col, Direction.North));
            priorityQueue.Add((score + ((direction == Direction.East) ? 1 : 1001), row, col + 1, Direction.East));
            priorityQueue.Add((score + ((direction == Direction.West) ? 1 : 1001), row, col - 1, Direction.West));
        }

        return -1;
    }



    protected override string SolvePartTwo()
    {
        var map = new List<List<char>>();
        var scores = new List<List<int>>();
        var startTile = (0, 0);
        var endTile = (0, 0);
        var rowCount = 0;

        foreach (var line in Input.SplitByNewline())
        {
            map.Add([.. line]);
            scores.Add([]);
            for (int i = 0; i < line.Length; i++)
            {
                scores.Last().Add(int.MaxValue);
            }

            if (line.Contains('S'))
            {
                startTile = (rowCount, line.IndexOf('S'));
            }

            if (line.Contains('E'))
            {
                endTile = (rowCount, line.IndexOf('E'));
            }

            rowCount++;
        }
        DFSFull(map, scores, startTile, endTile);
        BacktrackMarkBestPaths(map, scores, startTile, endTile);

        var bestTiles = 0;
        foreach (var line in map)
        {
            foreach (var c in line)
            {
                if (c == 'O') bestTiles++;
            }
        }

        return bestTiles.ToString();
    }

    private void BacktrackMarkBestPaths(List<List<char>> map, List<List<int>> scores, (int, int) startTile, (int, int) endTile)
    {
        var queue = new Queue<(int row, int col)>();
        queue.Enqueue(endTile);

        while (queue.Count > 0)
        {
            var (row, col) = queue.Dequeue();

            if (row == startTile.Item1 && col == startTile.Item2)
            {
                return;
            }

            if (map[row][col] == 'O')
            {
                continue;
            }
            else if (map[row][col] == 'E' || map[row][col] == 'S')
            {
                // Dont mark E or S as O
            }
            else
            {
                map[row][col] = 'O';
            }

            foreach (var line in map)
            {
                Console.WriteLine(line.JoinAsStrings());
            }

            Console.WriteLine();
            Console.WriteLine();
            //Sleep for 200 ms
            Thread.Sleep(100);

            // Check each direction for the lowest cost
            var lowestScore = int.MaxValue;
            if (row + 1 < map.Count && scores[row + 1][col] < lowestScore)
            {
                lowestScore = scores[row + 1][col];
            } 
            if (row - 1 >= 0 && scores[row - 1][col] < lowestScore)
            {
                lowestScore = scores[row - 1][col];
            }
            if (col + 1 < map[0].Count && scores[row][col + 1] < lowestScore)
            {
                lowestScore = scores[row][col + 1];
            }
            if (col - 1 >= 0 && scores[row][col - 1] < lowestScore)
            {
                lowestScore = scores[row][col - 1];
            }

            // For each direction with the lowest score, add it to the queue
            if (row + 1 < map.Count && scores[row + 1][col] == lowestScore)
            {
                queue.Enqueue((row + 1, col));
            }
            if (row - 1 >= 0 && scores[row - 1][col] == lowestScore)
            {
                queue.Enqueue((row - 1, col));
            }
            if (col + 1 < map[0].Count && scores[row][col + 1] == lowestScore)
            {
                queue.Enqueue((row, col + 1));
            }
            if (col - 1 >= 0 && scores[row][col - 1] == lowestScore)
            {
                queue.Enqueue((row, col - 1));
            }
        }
    }

    // Depth-first search with scores, we need to add 1 score per tile moved and 1000 for each turn
    // We need to keep track of the direction we are moving in 
    public void DFSFull(List<List<char>> map, List<List<int>> scores, (int, int) startTile, (int, int) endTile)
    {
        var priorityQueue = new SortedSet<(int score, int row, int col, Direction direction)>
        {
            (0, startTile.Item1, startTile.Item2, Direction.East)
        };

        var bestScore = int.MaxValue;
        while (priorityQueue.Count > 0)
        {
            var (score, row, col, direction) = priorityQueue.Min;
            priorityQueue.Remove(priorityQueue.Min);

            if (row < 0 || row >= map.Count || col < 0 || col >= map[0].Count || map[row][col] == '#')
            {
                continue;
            }
            if (row == endTile.Item1 && col == endTile.Item2)
            {
                bestScore = Math.Min(bestScore, score);
                continue;
            }
            if (score >= scores[row][col] || score > bestScore)
            {
                continue;
            }

           // Console.WriteLine($"Row: {row}, Col: {col}, Score: {score}, Direction: {direction}");
           // foreach (var line in map)
           // {
           //     Console.WriteLine(line.JoinAsStrings());
           // }
           // Console.WriteLine();
           // Console.WriteLine();
           // //Sleep for 500 ms

           //Thread.Sleep(200);

            scores[row][col] = score;
            if (map[row][col] != 'S')
            {
                if (direction == Direction.North)
                {
                    map[row][col] = '^';
                }
                else if (direction == Direction.East)
                {
                    map[row][col] = '>';
                }
                else if (direction == Direction.South)
                {
                    map[row][col] = 'v';
                }
                else if (direction == Direction.West)
                {
                    map[row][col] = '<';
                }
            }

            priorityQueue.Add((score + ((direction == Direction.South) ? 1 : 1001), row + 1, col, Direction.South));
            priorityQueue.Add((score + ((direction == Direction.North) ? 1 : 1001), row - 1, col, Direction.North));
            priorityQueue.Add((score + ((direction == Direction.East) ? 1 : 1001), row, col + 1, Direction.East));
            priorityQueue.Add((score + ((direction == Direction.West) ? 1 : 1001), row, col - 1, Direction.West));
        }
    }
}
