using System.Data;
using Robook.OrderBookNS;

namespace Robook.OrderBookColumns;

public class PriceCell : AbstractOrderBookCell {
    public override void SubscribeToColumnPropertyChangedEvents() {
    }

    public override void OnCellValueChanged(DataColumnChangeEventArgs e, DataTable dataTable) {
    }

    public override void RecalculateProperties(DataTable dataTable) {
    }
}