using System.Data;
using Robook.OrderBookNS;

namespace Robook.OrderBookColumns;

// Common base class for BuyVolumeColumn and SellVolumeColumn
public abstract class BuySellVolumeColumnBase : HistogramColumn {
    public  bool   CalculateBasedOnTradeVolume { get; set; }
    private string _volumeColumnName = "Volume";

    protected BuySellVolumeColumnBase() : base() {
    }

    public override void RecalculateMaxValueProperty(DataTable dataTable) {
        // Calculate MaxValue based on trade volume
        if (CalculateBasedOnTradeVolume) {
            if (!dataTable.Columns.Contains(_volumeColumnName)) {
                throw new InvalidOperationException(
                    $"The OrderBook does not contain the specified {_volumeColumnName} column required for calculations.");
            }
            MaxValue = dataTable.AsEnumerable().Select(row => row.Field<long?>(_volumeColumnName)).Max() ?? 0;
        }
        // Calculate MaxValue based on the column values
        else {
            base.RecalculateMaxValueProperty(dataTable);
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