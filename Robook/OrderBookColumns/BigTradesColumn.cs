﻿using System.Data;
using Robook.OrderBookNS;

namespace Robook.OrderBookColumns;

public class BigTradesColumn : AbstractOrderBookColumn {
    public BigTradesColumn() : base() {
        CellTemplate = new BigTradesCell();
    }

    public override void OnColumnChanged(DataColumnChangeEventArgs e, OrderBook orderBook) {
    }

    public override void RecalculateProperties(OrderBook orderBook) {
    }
}