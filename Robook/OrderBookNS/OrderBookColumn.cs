﻿using com.omnesys.rapi;
using Rithmic;

namespace Robook.OrderBookNS;

public interface IOrderBookColumn {
    string                    Name      { get; set; }
    public Type               Type      { get; }
    OrderBookColumnDataType[] DataTypes { get; }
    void                      SetUp(IOrderBook                ob);
    void                      ProcessBid(BidInfo              v, IOrderBook ob);
    void                      ProcessAsk(AskInfo              v, IOrderBook ob);
    void                      ProcessTrade(TradeInfo          v, IOrderBook ob);
    void                      ProcessBestBid(BestBidQuoteInfo v, IOrderBook ob);
    void                      ProcessBestAsk(BestAskQuoteInfo v, IOrderBook ob);
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

    public void SetUp(IOrderBook ob) {
    }

    public void ProcessBid(BidInfo v, IOrderBook ob) {
        if (ob.TryGetPriceIndex(v.Price, out var i)) {
            ob[i, Name] = (v.Size == 0) ? DBNull.Value : (long)v.Size;
        }
    }

    public void ProcessAsk(AskInfo v, IOrderBook ob) {
        if (ob.TryGetPriceIndex(v.Price, out var i)) {
            ob[i, Name] = (v.Size == 0) ? DBNull.Value : (long)v.Size;
        }
    }

    public void ProcessTrade(TradeInfo v, IOrderBook ob) {
        if (ob.TryGetPriceIndex(v.Price, out var i)) {
            ob[i, Name] = (ob[i, Name] as long? ?? 0) + v.Size;
        }
    }

    public void ProcessBestBid(BestBidQuoteInfo v, IOrderBook ob) {
        throw new NotImplementedException();
    }

    public void ProcessBestAsk(BestAskQuoteInfo v, IOrderBook ob) {
        throw new NotImplementedException();
    }
}

public class OrderBookTouchedTradeColumn : OrderBookDefaultColumn, IOrderBookColumn {
    public OrderBookTouchedTradeColumn(string name, OrderBookColumnDataType[] dataTypes, Type type) : base(
        name, dataTypes, type) {
    }

    private int[] bids;
    private int[] asks;

    private int bestBidIndex;
    private int bestAskIndex;

    private (int askIndex, int bidIndex) BestBidAsk() {
        int a = -1;
        int b = -1;

        for (var i = 0; i < asks.Length; i++) {
            if (asks[i] != 0) {
                a = i;
                continue;
            }
            else {
                if (bids[i] != 0) {
                    b = i;
                    break;
                }
            }
        }

        return (a, b);
    }

    private void _setupBidsAsks(IOrderBook ob) {
        bids = new int[ob.PriceArray.Length];
        asks = new int[ob.PriceArray.Length];
    }

    public void SetUp(IOrderBook ob) {
        _setupBidsAsks(ob);
    }

    public void ProcessTrade(TradeInfo v, IOrderBook ob) {
        if (v.CallbackType == CallbackType.History) {
            return;
        }
        
        if (ob.TryGetPriceIndex(v.Price, out var i)) {
            ob[i, Name] = (ob[i, Name] as long? ?? 0) + v.Size;
        }
    }

    public void ProcessBid(BidInfo v, IOrderBook ob) {
        
        if (!ob.TryGetPriceIndex(v.Price, out var i)) {
            return;
        }
        
        bids[i] = (int)v.Size;
        var x = BestBidAsk();

        if (x.bidIndex != bestBidIndex) {
            if (x.bidIndex < bestBidIndex) {
                ob[x.bidIndex, Name] = 0;
            }
        }

        bestAskIndex = x.askIndex;
        bestBidIndex = x.bidIndex;
    }

    public void ProcessAsk(AskInfo v, IOrderBook ob) {
        if (!ob.TryGetPriceIndex(v.Price, out var i)) {
            return;
        }
        asks[i] = (int)v.Size;
        var x = BestBidAsk();
        // if (x.askIndex != bestAskIndex) {
        //     if (x.askIndex > bestAskIndex) {
        //         ob[x.askIndex, Name] = 0;
        //     }
        // }
        bestAskIndex = x.askIndex;
        bestBidIndex = x.bidIndex;
    }
}