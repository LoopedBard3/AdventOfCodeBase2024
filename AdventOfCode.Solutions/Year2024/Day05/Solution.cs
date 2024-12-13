using System.Net;
using System.Reflection.Metadata.Ecma335;

namespace AdventOfCode.Solutions.Year2024.Day05;

class Solution : SolutionBase
{
    public Solution() : base(05, 2024, "") 
    {
        Debug = false;
    }

    protected override string SolvePartOne()
    {
        Dictionary<int, List<int>> rules = new Dictionary<int, List<int>>(); // Rules in the form of (page before, pages after)
        List<List<int>> updates = new List<List<int>>();

        // Read in the rules
        foreach (var line in Input.SplitByNewline())
        {
            if (line.Length == 5 && line[2] == '|') // Read in the rules
            {
                int beforePage = int.Parse(line[0..2]);
                int afterPage = int.Parse(line[3..5]);
                if (rules.TryGetValue(beforePage, out var afterPages))
                {
                    afterPages.Add(afterPage);
                }
                else
                {
                    if(!rules.TryAdd(beforePage, new List<int>() {  afterPage }))
                    {
                        throw new Exception($"Try add failed for {line}");
                    }
                }
                continue;
            }

            if (line.Length > 0) // Read in the updates
            {
                updates.Add(line.Split(',').Select(page => int.Parse(page)).ToList());
            }
        }

        var result = 0;
        foreach (var update in updates)
        {
            if (UpdateIsValid(update, rules)) // Check if the row is valid
            {
                result += update[update.Count / 2];
            }
        }

        return result.ToString();
    }

    private bool UpdateIsValid(List<int> update, Dictionary<int, List<int>> rules)
    {
        // iterate through the update list
        for (int updateIterator = 1; updateIterator < update.Count; updateIterator++)
        {
            // Get the rules for the current update
            if (!rules.TryGetValue(update[updateIterator], out var currentUpdateRules))
            {
                continue;
            }

            // Check against the rest of the updates before the current update
            for (int beforeUpdateIterator = 0; beforeUpdateIterator < updateIterator; beforeUpdateIterator++)
            {
                if (currentUpdateRules.Contains(update[beforeUpdateIterator]))
                {
                    return false;
                }
            }
        }

        return true;
    }

    protected override string SolvePartTwo()
    {
        Dictionary<int, List<int>> rules = new Dictionary<int, List<int>>(); // Rules in the form of (page before, pages after)
        List<List<int>> updates = new List<List<int>>();

        // Read in the rules
        foreach (var line in Input.SplitByNewline())
        {
            if (line.Length == 5 && line[2] == '|') // Read in the rules
            {
                int beforePage = int.Parse(line[0..2]);
                int afterPage = int.Parse(line[3..5]);
                if (rules.TryGetValue(beforePage, out var afterPages))
                {
                    afterPages.Add(afterPage);
                }
                else
                {
                    if (!rules.TryAdd(beforePage, new List<int>() { afterPage }))
                    {
                        throw new Exception($"Try add failed for {line}");
                    }
                }
                continue;
            }

            if (line.Length > 0) // Read in the updates
            {
                updates.Add(line.Split(',').Select(page => int.Parse(page)).ToList());
            }
        }

        var result = 0;
        foreach (var update in updates)
        {
            if (!UpdateIsValid(update, rules)) // Check if the row is valid
            {
                result += UpdateCorrectedMiddleValue(update, rules);
            } 
        }

        return result.ToString();
    }

    private int UpdateCorrectedMiddleValue(List<int> update, Dictionary<int, List<int>> rules)
    {
        // iterate through the update list
        for (int updateIterator = 1; updateIterator < update.Count; updateIterator++)
        {
            // Get the rules for the current update
            if (!rules.TryGetValue(update[updateIterator], out var currentUpdateRules))
            {
                continue;
            }

            // Check against the rest of the updates before the current update
            for (int beforeUpdateIterator = 0; beforeUpdateIterator < updateIterator; beforeUpdateIterator++)
            {
                if (currentUpdateRules.Contains(update[beforeUpdateIterator]))
                {
                    // switch the two values and check again from the first values location (+1)
                    var temp = update[beforeUpdateIterator];
                    update[beforeUpdateIterator] = update[updateIterator];
                    update[updateIterator] = temp;
                }
            }
        }

        return update[update.Count / 2];
    }
}
