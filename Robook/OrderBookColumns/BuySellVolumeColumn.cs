using Robook.OrderBookNS;

namespace Robook.OrderBookColumns;

// Common base class for BuyVolumeColumn and SellVolumeColumn
public abstract class BuySellVolumeColumnBase : HistogramColumn {
    public  bool   CalculateBasedOnTradeVolume { get; set; }
    private string _volumeColumnName = "Volume";

    protected BuySellVolumeColumnBase() : base() {
    }

    public override void RecalculateMaxValueProperty(IOrderBook orderBook) {
        // Calculate MaxValue based on trade volume
        if (CalculateBasedOnTradeVolume) {
            if (!orderBook.OBDT.Columns.Contains(_volumeColumnName)) {
                throw new InvalidOperationException(
                    $"The OrderBook does not contain the specified {_volumeColumnName} column required for calculations.");
            }

            var newMaxValue = orderBook.GetColumnValues<long>(_volumeColumnName).Max() ?? 0;
            MaxValue = newMaxValue;
        }
        // Calculate MaxValue based on the column values
        else {
            base.RecalculateMaxValueProperty(orderBook);
        }
    }
}

public class BuyVolumeColumn : BuySellVolumeColumnBase {
    public BuyVolumeColumn() : base() {
        CellTemplate = new BuyVolumeCell();
    }
}

public class SellVolumeColumn : BuySellVolumeColumnBase {
    public SellVolumeColumn() : base() {
        CellTemplate = new SellVolumeCell();
    }
}