namespace Robook.Helpers;

public static class DataGridViewHelpers {
    /// <summary>
    /// Retrieves a list of column indices in a DataGridView that correspond to a specific DataPropertyName.
    /// </summary>
    /// <param name="dataGridView">The DataGridView instance to search for columns.</param>
    /// <param name="dataPropertyName">The DataPropertyName to match against column DataPropertyNames.</param>
    /// <returns>A list of integers representing column indices with the specified DataPropertyName.</returns>
    public static List<int> GetColumnsByDataPropertyName(DataGridView dataGridView, string dataPropertyName) {
        List<int> columnsIds = new List<int>();

        foreach (DataGridViewColumn column in dataGridView.Columns) {
            if (column.DataPropertyName == dataPropertyName) {
                columnsIds.Add(column.Index);
            }
        }

        return columnsIds;
    }


    /// <summary>
    /// Computes the TextFormatFlags for a DataGridView cell's content alignment based on the specified alignment,
    /// wrap mode, and right-to-left settings.
    /// </summary>
    /// <remarks>
    /// This method is adapted from <see cref="DataGridViewUtilities.ComputeTextFormatFlagsForCellStyleAlignment"/>
    /// </remarks>
    public static TextFormatFlags ComputeTextFormatFlagsForCellStyleAlignment(
        DataGridViewContentAlignment alignment,
        DataGridViewTriState         wrapMode,
        bool                         rightToLeft) {
        TextFormatFlags tff;

        switch (alignment) {
            case DataGridViewContentAlignment.TopLeft:
                tff =  TextFormatFlags.Top;
                tff |= rightToLeft ? TextFormatFlags.Right : TextFormatFlags.Left;
                break;
            case DataGridViewContentAlignment.TopCenter:
                tff = TextFormatFlags.Top | TextFormatFlags.HorizontalCenter;
                break;
            case DataGridViewContentAlignment.TopRight:
                tff =  TextFormatFlags.Top;
                tff |= rightToLeft ? TextFormatFlags.Left : TextFormatFlags.Right;
                break;
            case DataGridViewContentAlignment.MiddleLeft:
                tff =  TextFormatFlags.VerticalCenter;
                tff |= rightToLeft ? TextFormatFlags.Right : TextFormatFlags.Left;
                break;
            case DataGridViewContentAlignment.MiddleCenter:
                tff = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
                break;
            case DataGridViewContentAlignment.MiddleRight:
                tff =  TextFormatFlags.VerticalCenter;
                tff |= rightToLeft ? TextFormatFlags.Left : TextFormatFlags.Right;
                break;
            case DataGridViewContentAlignment.BottomLeft:
                tff =  TextFormatFlags.Bottom;
                tff |= rightToLeft ? TextFormatFlags.Right : TextFormatFlags.Left;
                break;
            case DataGridViewContentAlignment.BottomCenter:
                tff = TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter;
                break;
            case DataGridViewContentAlignment.BottomRight:
                tff =  TextFormatFlags.Bottom;
                tff |= rightToLeft ? TextFormatFlags.Left : TextFormatFlags.Right;
                break;
            default:
                tff = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
                break;
        }

        if (wrapMode == DataGridViewTriState.False) {
            tff |= TextFormatFlags.SingleLine;
        }
        else {
            tff |= TextFormatFlags.WordBreak;
        }

        tff |= TextFormatFlags.NoPrefix;
        tff |= TextFormatFlags.PreserveGraphicsClipping;
        if (rightToLeft) {
            tff |= TextFormatFlags.RightToLeft;
        }

        return tff;
    }
}