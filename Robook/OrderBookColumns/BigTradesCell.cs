using System.Data;
using Robook.OrderBookNS;

namespace Robook.OrderBookColumns;

public class BigTradesCell : AbstractOrderBookCell {
    private long _bigTradeSize   = 15;
    private long _previousValue  = 0;
    private int  _bigTradesCount = 0;

    public override void SubscribeToColumnPropertyChangedEvents() {
    }

    public override void OnCellValueChanged(DataColumnChangeEventArgs e, IOrderBook orderBook) {
        RecalculateProperties(orderBook);
    }

    public override void RecalculateProperties(IOrderBook orderBook) {
        // var volume     = orderBook.OBDT.Rows[RowIndex]["Volume"];
        // var buyVolume  = orderBook.OBDT.Rows[RowIndex]["BuyVolume"];
        // var sellVolume = orderBook.OBDT.Rows[RowIndex]["SellVolume"];
        if (Convert.IsDBNull(Value)) return;

        long currentValue = Convert.ToInt64(Value);
        long diff         = currentValue - _previousValue;

        if (diff >= _bigTradeSize) {
            _bigTradesCount++;
            //Console.WriteLine(
            //$"Big trade #{_bigTradesCount} at price {DataGridView["Price", RowIndex].Value}. From {_previousValue} to {proposedValue}. Diff {diff}.");
        }

        _previousValue = currentValue;
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

        if (_bigTradesCount != 0) {
            DrawTextWithAlignment(_bigTradesCount.ToString(), cellBounds, cellStyle, graphics);
        }
    }
}