using com.omnesys.rapi;

namespace Robook.OrderBookNS;

public interface IOrderBookColumn {
    string                     Name      { get; set; }
    public Type                Type      { get; }
    OrderBookColumnDataType[] DataTypes { get; }
    void                       ProcessBid(int   i, BidInfo   v, OrderBook ob);
    void                       ProcessAsk(int   i, AskInfo   v, OrderBook ob);
    void                       ProcessTrade(int i, TradeInfo v, OrderBook ob);
}

public class OrderBookDefaultColumn : IOrderBookColumn {
    public string                    Name      { get; set; }
    public Type                      Type      { get; set; }
    public OrderBookColumnDataType[] DataTypes { get; set; }

    public OrderBookDefaultColumn(string name, OrderBookColumnDataType[] dataTypes, Type type) {
        Name      = name;
        DataTypes = dataTypes;
        Type      = type;
    }

    public void ProcessBid(int i, BidInfo v, OrderBook ob) {
        ob[i, Name] = (v.Size == 0) ? DBNull.Value : (long)v.Size;
    }

    public void ProcessAsk(int i, AskInfo v, OrderBook ob) {
        ob[i, Name] = (v.Size == 0) ? DBNull.Value : (long)v.Size;
    }

    public void ProcessTrade(int i, TradeInfo v, OrderBook ob) {
        ob[i, Name] = (ob[i, Name] as long? ?? 0) + v.Size;
    }
}