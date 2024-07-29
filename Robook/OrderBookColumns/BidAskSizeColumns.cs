﻿using System.Data;
using Robook.OrderBookNS;

namespace Robook.OrderBookColumns;

public class BidAskSizeColumns : AbstractOrderBookColumn {
    public override void OnColumnChanged(DataColumnChangeEventArgs e, IOrderBook orderBook) {
    }

    public override void RecalculateProperties(IOrderBook orderBook) {
    }
}

public class BidSizeColumn : BidAskSizeColumns {
    public BidSizeColumn() : base() {
        CellTemplate               = new BidSizeCell();
        DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
    }
}

public class AskSizeColumn : BidAskSizeColumns {
    public AskSizeColumn() : base() {
        CellTemplate               = new AskSizeCell();
        DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
    }
}