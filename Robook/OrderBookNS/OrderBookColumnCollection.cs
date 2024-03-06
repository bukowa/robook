using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using com.omnesys.rapi;

namespace Robook.OrderBookNS;

/// <summary>
///     This class exposes methods to add, remove, and manage
///     a collection of <see cref="IOrderBookColumn{T}"/>s.
/// </summary>
public class OrderBookColumnCollection {
    public readonly List<IOrderBookColumn<BidInfo>>        ColBidInfo        = new();
    public readonly List<IOrderBookColumn<AskInfo>>        ColAskInfo        = new();
    public readonly List<IOrderBookColumn<TradeInfo>>      ColTradeInfo      = new();
    public readonly List<IOrderBookColumn<OpenPriceInfo>>  ColOpenPriceInfo  = new();
    public readonly List<IOrderBookColumn<ClosePriceInfo>> ColClosePriceInfo = new();

    public void Add(IOrderBookColumn<BidInfo>        column) => ColBidInfo.Add(column);
    public void Add(IOrderBookColumn<AskInfo>        column) => ColAskInfo.Add(column);
    public void Add(IOrderBookColumn<TradeInfo>      column) => ColTradeInfo.Add(column);
    public void Add(IOrderBookColumn<OpenPriceInfo>  column) => ColOpenPriceInfo.Add(column);
    public void Add(IOrderBookColumn<ClosePriceInfo> column) => ColClosePriceInfo.Add(column);

    public bool Remove(IOrderBookColumn<BidInfo>        column) => ColBidInfo.Remove(column);
    public bool Remove(IOrderBookColumn<AskInfo>        column) => ColAskInfo.Remove(column);
    public bool Remove(IOrderBookColumn<TradeInfo>      column) => ColTradeInfo.Remove(column);
    public bool Remove(IOrderBookColumn<OpenPriceInfo>  column) => ColOpenPriceInfo.Remove(column);
    public bool Remove(IOrderBookColumn<ClosePriceInfo> column) => ColClosePriceInfo.Remove(column);
}