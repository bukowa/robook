using System.Data;
using Robook.OrderBookNS;

namespace Robook.OrderBookColumns;

public class PriceColumn : AbstractOrderBookColumn {
    public long LastPrice      { get; set; }
    public int  LastPriceIndex { get; set; }

    public long DailyOpenPrice      { get; set; }
    public int  DailyOpenPriceIndex { get; set; }

    public long HighestPrice      { get; set; }
    public int  HighestPriceIndex { get; set; }

    public long LowestPrice      { get; set; }
    public int  LowestPriceIndex { get; set; }

    public PriceColumn() {
        CellTemplate = new PriceCell();

        DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

        // todo: Get info about price: daily open, daily high, daily low
        // DailyOpenPrice = orderBook.GetDailyOpenPrice();
        // ...
    }

    // public override void OnPriceLevelChanged(EventArgs e, OrderBook orderBook) {
    //     // todo: If price level changes, recalculate properties and raise proper events
    //     // RecalculateProperties(orderBook);
    // }

    public override void RecalculateProperties(DataTable dataTable) {
        // todo: Get info about last price and update properties
        // var lastPrice = orderBook.GetLastPrice();
        // var lastPriceIndex = orderBook.GetLastPriceIndex();
        // if (lastPriceIndex != LastPriceIndex) {
        //     LastPriceIndex = lastPriceIndex;
        //     LastPrice      = lastPrice;
        //
        //     if (LastPriceIndex < HighestPriceIndex) {
        //         HighestPrice      = LastPrice;
        //         HighestPriceIndex = LastPriceIndex;
        //     }
        //     else if (LastPriceIndex > LowestPriceIndex) {
        //         LowestPrice      = LastPrice;
        //         LowestPriceIndex = LastPriceIndex;
        //     }
        // OnLastPriceChanged();
        // }
    }

    public override void OnColumnChanged(DataColumnChangeEventArgs e, DataTable dataTable) {
    }

    # region "Events"

    public event EventHandler<EventArgs> LastPriceChanged;

    protected virtual void OnLastPriceChanged(EventArgs args) {
        LastPriceChanged?.Invoke(this, args);
    }

    #endregion
}