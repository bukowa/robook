namespace Robook.Helpers;
// todo: change to better name
public class MathHelpers { 

    /// <summary>
    /// Calculates the relative percentage of each element compared to the maximum value in the provided array.
    /// </summary>
    /// <param name="array">The array of double values.</param>
    /// <returns>An array of relative percentages corresponding to each element in the input array.</returns>
    public static double[] RelativePercentOfMax(double[] array) {
        return array.Select(x => array.Max() <= 0 ? 0 : (x * 100) / array.Max()).ToArray();
    }

    /// <summary>
    /// Calculates the relative percentage of a given value compared to the maximum value in an array.
    /// </summary>
    /// <param name="value">The value to calculate the percentage for.</param>
    /// <param name="array">The array of values.</param>
    /// <returns>The relative percentage of the value in comparison to the maximum value in the array.</returns>
    public static double RelativePercentOfMaxValue(double value, double[] array) {
        return array.Max() <= 0 ? 0 : array.Select(x => (value * 100) / array.Max()).FirstOrDefault();
    }
}