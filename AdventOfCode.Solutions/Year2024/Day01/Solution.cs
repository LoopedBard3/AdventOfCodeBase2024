namespace AdventOfCode.Solutions.Year2024.Day01;

class Solution : SolutionBase
{
    public List<int> inputCol1;
    public List<int> inputCol2;

    public Solution() : base(01, 2024, "") 
    {
        Debug = false;
        inputCol1 = new List<int>();
        inputCol2 = new List<int>();
        foreach (string line in Input.SplitByNewline()){
            var hold = line.Split("   ");
            inputCol1.Add(int.Parse(hold[0]));
            inputCol2.Add(int.Parse(hold[1]));
        }
        if (inputCol1.Count != inputCol2.Count)
        {
            throw new Exception("Column sizes don't match");
        }
    }

    protected override string SolvePartOne()
    {
        inputCol1.Sort();
        inputCol2.Sort();
        var distance = 0;
        for(int i = 0; i < inputCol1.Count; i++){
            distance += Math.Abs(inputCol1[i] - inputCol2[i]);
        }
        return distance.ToString();
    }

    protected override string SolvePartTwo()
    {
        var col1Iter = inputCol1.GetEnumerator();
        var col2Iter = inputCol2.GetEnumerator();
        col1Iter.MoveNext();
        col2Iter.MoveNext();
        var col1Value = col1Iter.Current;
        var col1Count = 0;
        var result = 0;
        var validIterationCol1 = true;
        var validIterationCol2 = true;
        do{
            while(validIterationCol1 && col1Iter.Current == col1Value){
                validIterationCol1 = col1Iter.MoveNext();
                col1Count += 1;
            }

            while(validIterationCol2 && col2Iter.Current < col1Value){
                validIterationCol2 = col2Iter.MoveNext();
            }

            var col2Count = 0;
            while (validIterationCol2 && col2Iter.Current == col1Value){
                validIterationCol2 = col2Iter.MoveNext();
                col2Count += 1;
            }
            result += col1Value * col1Count * col2Count;
            col1Count = 0;
            col1Value = col1Iter.Current;
        } while (validIterationCol1 && validIterationCol2);
        return result.ToString();
    }
}
