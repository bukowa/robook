using System.Data;
using Robook.Helpers;
using Robook.OrderBookNS;

namespace Robook.OrderBookColumns;

public abstract class BidAskSizeCells : AbstractOrderBookCell {
    public Brush CellBackgroundBrush;
    public Brush EmptyCellBackgroundBrush;

    public BidAskSizeCells() {
    }

    public override void SubscribeToColumnPropertyChangedEvents() {
    }

    public override void OnCellValueChanged(DataColumnChangeEventArgs e, DataTable dataTable) {
    }

    public override void RecalculateProperties(DataTable dataTable) {
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

        // Draw background
        var defaultBackgroundBrush = new SolidBrush(cellStyle.BackColor);
        var backgroundBrush = Convert.IsDBNull(value)
            ? EmptyCellBackgroundBrush ?? defaultBackgroundBrush
            : CellBackgroundBrush ?? defaultBackgroundBrush;

        graphics.FillRectangle(backgroundBrush, cellBounds);

        // Draw text 
        DrawTextWithAlignment(formattedValue.ToString(), cellBounds, cellStyle, graphics);

        // Draw border
        if (paintParts.HasFlag(DataGridViewPaintParts.Border)) {
            PaintBorder(graphics, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
        }
    }
}

public class BidSizeCell : BidAskSizeCells {
    public BidSizeCell() {
        CellBackgroundBrush      = new SolidBrush(Color.FromArgb(255, 106, 168, 98));
        EmptyCellBackgroundBrush = new SolidBrush(Color.FromArgb(255, 207, 230, 204));
    }
}

public class AskSizeCell : BidAskSizeCells {
    public AskSizeCell() {
        CellBackgroundBrush      = new SolidBrush(Color.FromArgb(255, 235, 64,  52));
        EmptyCellBackgroundBrush = new SolidBrush(Color.FromArgb(255, 235, 204, 202));
    }
}