using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using com.omnesys.rapi;

namespace Robook.OrderBookNS;

public class OrderBookColumnCollection {

    public Dictionary<OrderBookColumnDataType, List<IOrderBookColumn>> ColumnsByDataType = 
        Enum.GetValues(typeof(OrderBookColumnDataType))
            .Cast<OrderBookColumnDataType>()
            .ToDictionary(dataType => dataType, dataType => new List<IOrderBookColumn>());

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