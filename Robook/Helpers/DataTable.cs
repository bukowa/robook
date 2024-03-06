using System.Data;

namespace Robook.Helpers;

/// <summary>
/// Provides helper methods for working with DataTables.
/// </summary>
public static class DataTableHelpers {
    /// <summary>
    /// Fills a specified column in the DataTable with random numeric values.
    /// </summary>
    /// <typeparam name="T">The type of numeric values to generate.</typeparam>
    /// <param name="columnName">The name of the column to be filled.</param>
    /// <param name="dataTable">The DataTable to fill.</param>
    /// <param name="minValue">The minimum value for the generated numbers.</param>
    /// <param name="maxValue">The maximum value for the generated numbers.</param>
    /// <exception cref="ArgumentException">Thrown when the specified column does not belong to the DataTable.</exception>
    public static void FillColumnWithRandomNumber<T>(string columnName, DataTable dataTable, T minValue, T maxValue)
        where T : struct, IComparable<T> {
        if (!dataTable.Columns.Contains(columnName)) {
            throw new ArgumentException($"Column {columnName} does not belong to Data Table.");
        }

        T[] randomNumbers = RandomDataGenerator.NumericArray<T>(dataTable.Rows.Count, minValue, maxValue);

        for (var i = 0; i < dataTable.Rows.Count; i++) {
            T randValue = randomNumbers[i];
            dataTable.Rows[i][columnName] = randValue;
        }
    }

    /// <summary>
    /// Fills a specified column in the DataTable with random strings.
    /// </summary>
    /// <param name="columnName">The name of the column to be filled.</param>
    /// <param name="dataTable">The DataTable to fill.</param>
    /// <param name="stringLength">The length of the generated strings (default is 8).</param>
    /// <exception cref="ArgumentException">Thrown when the specified column does not belong to the DataTable.</exception>
    public static void FillColumnWithRandomString(string columnName, DataTable dataTable, int stringLength = 8) {
        if (!dataTable.Columns.Contains(columnName)) {
            throw new ArgumentException($"Column {columnName} does not belong to Data Table.");
        }

        string[] randomStrings = RandomDataGenerator.StringArray(dataTable.Rows.Count, stringLength);

        for (var i = 0; i < dataTable.Rows.Count; i++) {
            string randValue = randomStrings[i];
            dataTable.Rows[i][columnName] = randValue;
        }
    }

    /// <summary>
    /// Fills a specified row in the DataTable with random numeric values.
    /// </summary>
    /// <typeparam name="T">The type of numeric values to generate.</typeparam>
    /// <param name="rowIndex">The index of the row to be filled.</param>
    /// <param name="dataTable">The DataTable to fill.</param>
    /// <param name="minValue">The minimum value for the generated numbers.</param>
    /// <param name="maxValue">The maximum value for the generated numbers.</param>
    /// <exception cref="ArgumentException">Thrown when the specified row index is out of bounds for the DataTable.</exception>
    public static void FillRowWithRandomNumber<T>(int rowIndex, DataTable dataTable, T minValue, T maxValue)
        where T : struct, IComparable<T> {
        if (rowIndex < 0 || rowIndex >= dataTable.Rows.Count) {
            throw new ArgumentException($"Row index {rowIndex} is out of bounds for the DataTable.");
        }

        T[] randomNumbers = RandomDataGenerator.NumericArray<T>(dataTable.Columns.Count, minValue, maxValue);

        for (var i = 0; i < dataTable.Columns.Count; i++) {
            T randValue = randomNumbers[i];
            dataTable.Rows[rowIndex][i] = randValue;
        }
    }

    /// <summary>
    /// Fills a specified row in the DataTable with random strings.
    /// </summary>
    /// <param name="rowIndex">The index of the row to be filled.</param>
    /// <param name="dataTable">The DataTable to fill.</param>
    /// <param name="stringLength">The length of the generated strings.</param>
    /// <exception cref="ArgumentException">Thrown when the specified row index is out of bounds for the DataTable.</exception>
    public static void FillRowWithRandomString(int rowIndex, DataTable dataTable, int stringLength = 8) {
        if (rowIndex < 0 || rowIndex >= dataTable.Rows.Count) {
            throw new ArgumentException($"Row index {rowIndex} is out of bounds for the DataTable.");
        }

        string[] randomStrings = RandomDataGenerator.StringArray(dataTable.Columns.Count, stringLength);

        for (var i = 0; i < dataTable.Columns.Count; i++) {
            string randValue = randomStrings[i];
            dataTable.Rows[rowIndex][i] = randValue;
        }
    }
}