namespace Robook.OrderBookColumns;

public class BuyVolumeCell : HistogramCell {
    public BuyVolumeCell() : base() {
        HistogramBrush = Brushes.SeaGreen;
    }

    public override string ToString()
        => $"DataGridViewBuyVolumeCell {{ ColumnIndex={ColumnIndex}, RowIndex={RowIndex} }}";
}

public class SellVolumeCell : HistogramCell {
    public SellVolumeCell() : base() {
        HistogramBrush = Brushes.IndianRed;
    }

    public override string ToString()
        => $"DataGridViewSellVolumeCell {{ ColumnIndex={ColumnIndex}, RowIndex={RowIndex} }}";
}