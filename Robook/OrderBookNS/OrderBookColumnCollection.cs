using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using com.omnesys.rapi;

namespace Robook.OrderBookNS;

public class OrderBookColumnCollection {
    public Dictionary<OrderBookColumnDataType, List<IOrderBookColumn>> ColumnsByDataType = new() {
        { OrderBookColumnDataType.Ask, new() },
        { OrderBookColumnDataType.Bid, new() },
        { OrderBookColumnDataType.Trade, new() },
    };
    public OrderBookColumnCollection() {
        foreach (OrderBookColumnDataType dataType in Enum.GetValues(typeof(OrderBookColumnDataType))) {
            ColumnsByDataType[dataType] = new List<IOrderBookColumn>();
        }
    }
    public void Add(IOrderBookColumn col) {
        foreach (var cdt in col.DataTypes) {
            ColumnsByDataType[cdt].Add(col);
        }
    }
    public void Remove(IOrderBookColumn col) {
        foreach (var cdt in col.DataTypes) {
            ColumnsByDataType[cdt].Remove(col);
        }
    }
}