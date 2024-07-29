using System.Data;
using Robook.OrderBookNS;

namespace Robook.OrderBookColumns;

public class PriceCell : AbstractOrderBookCell {
    public override void SubscribeToColumnPropertyChangedEvents() {
    }

    public override void OnCellValueChanged(DataColumnChangeEventArgs e, IOrderBook orderBook) {
    }

    public override void RecalculateProperties(IOrderBook orderBook) {
    }
}