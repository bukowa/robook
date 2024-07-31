using System.Data;
using Robook.OrderBookNS;

namespace Robook.OrderBookColumns;

public class BigTradesColumn : AbstractOrderBookColumn {
    public BigTradesColumn() : base() {
        CellTemplate = new BigTradesCell();
    }

    public override void OnColumnChanged(DataColumnChangeEventArgs e, DataTable dataTable) {
    }

    public override void RecalculateProperties(DataTable dataTable) {
    }
}