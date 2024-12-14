
using System.Numerics;

namespace AdventOfCode.Solutions.Year2024.Day07;

class Solution : SolutionBase
{
    public Solution() : base(07, 2024, "", false) { }

    public class Calibration
    {
        public BigInteger total;
        public BigInteger[] values;
    }

    protected override string SolvePartOne()
    {
        // Read in each line in as a calibration
        var calibrations = new List<Calibration>();
        foreach (var line in Input.SplitByNewline())
        {
            var calibration = new Calibration();
            var parts = line.Split(":");
            calibration.total = BigInteger.Parse(parts[0]);
            calibration.values = parts[1][1..].Split(" ").Select(BigInteger.Parse).ToArray();
            calibrations.Add(calibration);
        }

        // Iterate through the possible calculations with * and +, taking the sum of each valid calibration
        BigInteger validSum = 0;
        foreach (var calibration in calibrations)
        {
            if (IsValidCalibration(calibration, 0, 0))
            {
                validSum += calibration.total;
            }
        }
        return validSum.ToString();
    }

    private bool IsValidCalibration(Calibration calibration, int spot, BigInteger curValue)
    {
        // check we are at the end of the calibration and have the correct value
        if (spot == calibration.values.Length)
        {
            return curValue == calibration.total;
        }

        // Check if we are over the total
        if (curValue > calibration.total)
        {
            return false;
        }

        // Check if the multiplication path is valid
        var multCurValue = curValue * calibration.values[spot]; // Ensure we can start with a multiply
        if (spot == 0)
        {
            multCurValue = calibration.values[0];
        }
        if (IsValidCalibration(calibration, spot + 1, multCurValue))
        {
            return true;
        }

        // Check if the addition path is valid
        if (IsValidCalibration(calibration, spot + 1, curValue + calibration.values[spot]))
        {
            return true;
        }

        return false;
    }

    protected override string SolvePartTwo()
    {
        // Read in each line in as a calibration
        var calibrations = new List<Calibration>();
        foreach (var line in Input.SplitByNewline())
        {
            var calibration = new Calibration();
            var parts = line.Split(":");
            calibration.total = BigInteger.Parse(parts[0]);
            calibration.values = parts[1][1..].Split(" ").Select(BigInteger.Parse).ToArray();
            calibrations.Add(calibration);
        }

        // Iterate through the possible calculations with * and +, taking the sum of each valid calibration
        BigInteger validSum = 0;
        bool valid;
        foreach (var calibration in calibrations)
        {
            if ((valid = IsValidCalibrationPt2(calibration, 0, 0)))
            {
                validSum += calibration.total;
            }
        }
        return validSum.ToString();
    }

    private bool IsValidCalibrationPt2(Calibration calibration, int spot, BigInteger curValue)
    {
        // check we are at the end of the calibration and have the correct value
        if (spot == calibration.values.Length)
        {
            return curValue == calibration.total;
        }

        // Check if we are over the total
        if (curValue > calibration.total)
        {
            return false;
        }

        // Check if the multiplication path is valid
        var multCurValue = curValue * calibration.values[spot]; // Ensure we can start with a multiply
        if (spot == 0)
        {
            multCurValue = calibration.values[0];
        }
        if (IsValidCalibrationPt2(calibration, spot + 1, curValue * calibration.values[spot]))
        {
            return true;
        }

        // Check if the addition path is valid
        if (IsValidCalibrationPt2(calibration, spot + 1, curValue + calibration.values[spot]))
        {
            return true;
        }

        // Check if combine path is valid but only if we are not at the end of the calibration
        if (IsValidCalibrationPt2(calibration, spot + 1, BigInteger.Parse(string.Concat(curValue.ToString() + calibration.values[spot].ToString()))))
        {
            return true;
        }

        return false;
    }
}
