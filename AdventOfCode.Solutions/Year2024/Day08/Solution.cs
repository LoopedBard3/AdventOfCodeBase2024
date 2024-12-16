namespace AdventOfCode.Solutions.Year2024.Day08;

class Solution : SolutionBase
{
    public Solution() : base(08, 2024, "", false) { }

    protected override string SolvePartOne()
    {
        var grid = new List<List<char>>();
        foreach (var line in Input.SplitByNewline())
        {
            grid.Add([.. line]);
        }

        // Generate the list of locations for each antenna frequency (frequency is based on character (any char not '.'))
        var antennaLocationsByFrequency = new Dictionary<char, List<(int row, int col)>>(); // Frequency: List<locations>
        for (int row = 0; row < grid.Count; row++)
        {
            for (int col = 0; col < grid[row].Count; col++)
            {
                if (grid[row][col] != '.')
                {
                    if (!antennaLocationsByFrequency.ContainsKey(grid[row][col]))
                    {
                        antennaLocationsByFrequency[grid[row][col]] = new List<(int row, int col)>();
                    }
                    antennaLocationsByFrequency[grid[row][col]].Add((row, col));
                }
            }
        }

        // For each antenna frequency, calculate the antinodes (spot the same distance between pair of antenna in the opposite direction of the pair per antenna)
        var antinodeList = new List<(int row, int col)>();
        foreach (var antennaLocations in antennaLocationsByFrequency)
        {
            var antinodeFrequencyList = new List<(int row, int col)>();
            for (int i = 0; i < antennaLocations.Value.Count; i++)
            {
                for (int j = i + 1; j < antennaLocations.Value.Count; j++)
                {
                    // Calculate the two points that are the same distance from the two antenna
                    (int row, int col) distanceVector = (antennaLocations.Value[j].row - antennaLocations.Value[i].row, antennaLocations.Value[j].col - antennaLocations.Value[i].col);
                    (int row, int col) antinodeMain = (antennaLocations.Value[i].row - distanceVector.row, antennaLocations.Value[i].col - distanceVector.col);
                    (int row, int col) antinodeOpposite = (antennaLocations.Value[j].row + distanceVector.row, antennaLocations.Value[j].col + distanceVector.col);
                    if(antinodeMain.row >= 0 && antinodeMain.row < grid.Count && antinodeMain.col >= 0 && antinodeMain.col < grid[0].Count)
                    {
                        antinodeFrequencyList.Add(antinodeMain);
                    }
                    if (antinodeOpposite.row >= 0 && antinodeOpposite.row < grid.Count && antinodeOpposite.col >= 0 && antinodeOpposite.col < grid[0].Count)
                    {
                        antinodeFrequencyList.Add(antinodeOpposite);
                    }
                }
            }
            antinodeList.AddRange(antinodeFrequencyList.Distinct());
        }
        return antinodeList.Distinct().Count().ToString();
    }

    protected override string SolvePartTwo()
    {
        var grid = new List<List<char>>();
        foreach (var line in Input.SplitByNewline())
        {
            grid.Add([.. line]);
        }

        // Generate the list of locations for each antenna frequency (frequency is based on character (any char not '.'))
        var antennaLocationsByFrequency = new Dictionary<char, List<(int row, int col)>>(); // Frequency: List<locations>
        for (int row = 0; row < grid.Count; row++)
        {
            for (int col = 0; col < grid[row].Count; col++)
            {
                if (grid[row][col] != '.')
                {
                    if (!antennaLocationsByFrequency.ContainsKey(grid[row][col]))
                    {
                        antennaLocationsByFrequency[grid[row][col]] = new List<(int row, int col)>();
                    }
                    antennaLocationsByFrequency[grid[row][col]].Add((row, col));
                }
            }
        }

        // For each antenna frequency, calculate the antinodes (spot the same distance between pair of antenna in the opposite direction of the pair per antenna)
        var antinodeList = new List<(int row, int col)>();
        foreach (var antennaLocations in antennaLocationsByFrequency)
        {
            var antinodeFrequencyList = new List<(int row, int col)>();
            for (int i = 0; i < antennaLocations.Value.Count; i++)
            {
                for (int j = i + 1; j < antennaLocations.Value.Count; j++)
                {
                    // Calculate the two points that are the same distance from the two antenna
                    (int row, int col) distanceVector = (antennaLocations.Value[j].row - antennaLocations.Value[i].row, antennaLocations.Value[j].col - antennaLocations.Value[i].col);

                    (int row, int col) currentLocation = antennaLocations.Value[i];
                    while (currentLocation.row >= 0 && currentLocation.row < grid.Count && currentLocation.col >= 0 && currentLocation.col < grid[0].Count)
                    {
                        // Get every distance away
                        antinodeFrequencyList.Add(currentLocation);
                        //grid[currentLocation.row][currentLocation.col] = '#';
                        currentLocation = (currentLocation.row - distanceVector.row, currentLocation.col - distanceVector.col); // Move to the next location
                    }

                    currentLocation = antennaLocations.Value[j];
                    while (currentLocation.row >= 0 && currentLocation.row < grid.Count && currentLocation.col >= 0 && currentLocation.col < grid[0].Count)
                    {
                        // Get every distance away
                        antinodeFrequencyList.Add(currentLocation);
                        //grid[currentLocation.row][currentLocation.col] = '#';
                        currentLocation = (currentLocation.row + distanceVector.row, currentLocation.col + distanceVector.col); // Move to the next location
                    }
                }
            }
            //Console.WriteLine($"Found {antinodeFrequencyList.Count} locations for {antennaLocations.Key}: {antinodeFrequencyList.JoinAsStrings(", ")}");
            antinodeList.AddRange(antinodeFrequencyList.Distinct());
        }

        //foreach(var row in grid)
        //{
        //    Console.WriteLine(row.ToArray());
        //}
        return antinodeList.Distinct().Count().ToString();
    }
}
