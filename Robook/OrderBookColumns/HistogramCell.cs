using System.Data;
using System.Diagnostics.Metrics;
using Microsoft.ML;
using Robook.Helpers;
using Robook.OrderBookNS;
using HorizontalAlignment = Robook.Helpers.HorizontalAlignment;

namespace Robook.OrderBookColumns;

public class HistogramCell : AbstractOrderBookCell {
    private long  MaxValue       { set; get; }
    public  Brush TextBrush      { set; get; }
    public  Brush HistogramBrush { set; get; }

    private HashSet<int> _changedValueRows = new HashSet<int>();

    public HistogramCell() : base() {
        // todo: global brushes
        TextBrush      = Brushes.Black;
        HistogramBrush = Brushes.CornflowerBlue;
    }

    public override void SubscribeToColumnPropertyChangedEvents() {
        if (OwningColumn is HistogramColumn histogramColumn) {
            histogramColumn.MaxValueChanged += HistogramColumn_MaxValueChanged;
        }
    }

    public override void OnCellValueChanged(DataColumnChangeEventArgs e, OrderBook orderBook) {
    }

    public override void RecalculateProperties(OrderBook orderBook) {
    }

    protected override void Paint(
        Graphics                        graphics,
        Rectangle                       clipBounds,
        Rectangle                       cellBounds,
        int                             rowIndex,
        DataGridViewElementStates       cellState,
        object                          value,
        object                          formattedValue,
        string                          errorText,
        DataGridViewCellStyle           cellStyle,
        DataGridViewAdvancedBorderStyle advancedBorderStyle,
        DataGridViewPaintParts          paintParts) {
        // Call base without without specifying 'formattedValue', because text will be drawn at a later stage
        base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value,
                   "", errorText, cellStyle, advancedBorderStyle, paintParts);

        var cellValue = Convert.IsDBNull(value) ? 0D : Convert.ToDouble(value);

        if (cellValue != 0) {
            // Record the row for later invalidation
            _changedValueRows.Add(RowIndex);

            DrawHistogram(graphics, cellValue, cellBounds);

            DrawTextWithAlignment(cellValue.ToString(), cellBounds, cellStyle, graphics);

            if (paintParts.HasFlag(DataGridViewPaintParts.Border)) {
                PaintBorder(graphics, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
            }
        }
    }

    private void DrawHistogram(Graphics graphics, double cellValue, Rectangle cellBounds) {
        if (MaxValue != 0) {
            var valuePercentage = MathHelpers.RelativePercentOfMaxValue(cellValue, new double[] { MaxValue });
            var alignment       = (OwningColumn as HistogramColumn)?.HistogramAlignment ?? HorizontalAlignment.Left;

            var histogramBounds = DrawingHelpers.RelativePctRectangle(
                cellBounds,
                valuePercentage,
                100D,
                alignment
            );

            graphics.FillRectangle(HistogramBrush, histogramBounds);
        }
    }

    private void HistogramColumn_MaxValueChanged(object sender, long newMaxValue) {
        MaxValue = newMaxValue;

        // Optimize: Invalidate only the cells whose value have changed.
        foreach (var rowIndex in _changedValueRows) {
            DataGridView.InvalidateCell(ColumnIndex, rowIndex);
        }
    }

    // Override the Clone method to copy the new properties during cloning operations
    public override object Clone() {
        var clone = base.Clone() as HistogramCell;

        if (clone != null) {
            clone.TextBrush      = this.TextBrush;
            clone.HistogramBrush = this.HistogramBrush;
        }

        return clone;
    }

    public override string ToString()
        => $"DataGridViewHistogramCell {{ ColumnIndex={ColumnIndex}, RowIndex={RowIndex} }}";
}