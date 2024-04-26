using com.omnesys.rapi;
using Rithmic;

namespace Robook.OrderBookNS;

public interface IOrderBookColumn {
    string                     Name      { get; set; }
    public Type                Type      { get; }
    OrderBookColumnDataType[] DataTypes { get; }
    void                       ProcessBid(int   i, BidInfo   v, OrderBook ob);
    void                       ProcessAsk(int   i, AskInfo   v, OrderBook ob);
    void                       ProcessTrade(int i, TradeInfo v, OrderBook ob);
    void ProcessBestBid(int i, BestBidQuoteInfo v, OrderBook ob);
    void ProcessBestAsk(int i, BestAskQuoteInfo v, OrderBook ob);
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

    public void ProcessBestBid(int i, BestBidQuoteInfo v, OrderBook ob) {
        throw new NotImplementedException();
    }

    public void ProcessBestAsk(int i, BestAskQuoteInfo v, OrderBook ob) {
        throw new NotImplementedException();
    }
}

public class OrderBookTouchedTradeColumn : OrderBookDefaultColumn, IOrderBookColumn {
    
    public OrderBookTouchedTradeColumn(string name, OrderBookColumnDataType[] dataTypes, Type type) : base(name, dataTypes, type) {

    }

    public void ProcessTrade(int i, TradeInfo v, OrderBook ob) {
        if (v.CallbackType == CallbackType.Image || v.CallbackType == CallbackType.History) {
            return;
        }
        ob[i, Name]            = (ob[i, Name] as long? ?? 0) + v.Size;
    }
   
    public void ProcessBestBid(int i, BestBidQuoteInfo v, OrderBook ob) {
        ob[i, Name] = 0;
    }
    
    public void ProcessBestAsk(int i, BestAskQuoteInfo v, OrderBook ob) {
        ob[i, Name] = 0;
    }
}