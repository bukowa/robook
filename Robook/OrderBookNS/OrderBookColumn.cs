using System.Collections.ObjectModel;
using com.omnesys.rapi;

namespace Robook.OrderBookNS;

/// <summary>
///     Represents a column in an <see cref="OrderBook"/>.
///     It's responsibility is processing a value of type <typeparamref name="TOType"/>.
///     When a new value of type <typeparamref name="TOType"/> is produced by the data source,
///     it is passed to the column's <ref name="ProcessAt"/> method, which is responsible
///     for updating the <see cref="OrderBook"/>. This is a generic interface,
///     so it can be implemented for any type of value that the data source produces.
///     The <see cref="IOrderBookColumn{TOType}"/> is also responsible for updating the <see cref="ProcessedCount"/>
///     property, which is used to determine if the OrderBook processed the given value.
/// </summary>
/// <typeparam name="TOType"></typeparam>
public interface IOrderBookColumn<TOType> {
    Type   Type { get; }
    string Name { get; set; }
    void   ProcessHistory(IEnumerable<TOType> v, OrderBook ob);
    void   ProcessRealTimeAt(int              i, TOType    v, OrderBook ob);
    int    ProcessedCount { get; }
}

/// <summary>
///     Implements basic functionality for an OrderBookColumn.
///     <see cref="Name"/> is the name of the column.
///     <see cref="Type"/> is the type of the column.
///     <see cref="ProcessedCount"/> is the number of values processed by the column
///                                 and should be implemented by the derived class.
/// </summary>
public class OrderBookBaseColumn {
    public string Name           { get; set; }
    public Type   Type           { get; set; }
    public int    ProcessedCount { get; set; }

    public OrderBookBaseColumn(string name, Type type) {
        Name = name;
        Type = type;
    }
}

/// <summary>
///     Ask column represents the ask size in the order book.
/// </summary>
public class
    OrderBookAskColumn : OrderBookBaseColumn, IOrderBookColumn<AskInfo> {
    public OrderBookAskColumn(
        string name = "Ask",
        Type?  type = null
    )
        : base(name, type ?? typeof(double)) {
    }

    public void ProcessHistory(IEnumerable<AskInfo> v, OrderBook ob) {
        throw new NotImplementedException();
    }

    public virtual void ProcessRealTimeAt(int i, AskInfo v, OrderBook ob) {
        ProcessedCount++;
        ob[i, Name] = (v.Size == 0) ? DBNull.Value : v.Size;
    }
}

public class
    OrderBookAskColumnLong : OrderBookAskColumn {
    public OrderBookAskColumnLong(
        string name = "Ask"
    ) : base(name, typeof(long)) {
    }

    public override void ProcessRealTimeAt(int i, AskInfo v, OrderBook ob) {
        ProcessedCount++;
        ob[i, Name] = (v.Size == 0) ? DBNull.Value : (int)v.Size;
    }
}

/// <summary>
///     Bid column represents the bid size in the order book.
/// </summary>
public class
    OrderBookBidColumn : OrderBookBaseColumn, IOrderBookColumn<BidInfo> {
    public OrderBookBidColumn(
        string name = "Bid",
        Type?  type = null) : base(name, type ?? typeof(double)) {
    }

    public void ProcessHistory(IEnumerable<BidInfo> v, OrderBook ob) {
        throw new NotImplementedException();
    }

    public virtual void ProcessRealTimeAt(int i, BidInfo v, OrderBook ob) {
        ProcessedCount++;
        ob[i, Name] = (v.Size == 0) ? DBNull.Value : v.Size;
    }
}

public class
    OrderBookBidColumnLong : OrderBookBidColumn {
    public OrderBookBidColumnLong(
        string name = "Bid"
    ) : base(name, typeof(long)) {
    }

    public override void ProcessRealTimeAt(int i, BidInfo v, OrderBook ob) {
        ProcessedCount++;
        ob[i, Name] = (v.Size == 0) ? DBNull.Value : (long)v.Size;
    }
}

/// <summary>
///     Volume column represents the total volume of trades at the given price.
/// </summary>
public class
    OrderBookVolumeColumn : OrderBookBaseColumn, IOrderBookColumn<TradeInfo> {
    public OrderBookVolumeColumn(
        string name = "Volume",
        Type?  type = null) : base(name, type ?? typeof(long)) {
    }

    public void ProcessHistory(IEnumerable<TradeInfo> v, OrderBook ob) {
        throw new NotImplementedException();
    }

    public void ProcessRealTimeAt(int i, TradeInfo v, OrderBook ob) {
        ProcessedCount++;
        ob[i, Name] = (ob[i, Name] as long? ?? 0) + v.Size;
    }
}

/// <summary>
///     SellVolume column represents the total volume of sell trades at the given price.
/// </summary>
public class
    OrderBookSellVolumeColumn : OrderBookBaseColumn, IOrderBookColumn<TradeInfo> {
    public OrderBookSellVolumeColumn(
        string name = "SellVolume",
        Type?  type = null) : base(name, type ?? typeof(long)) {
    }

    public void ProcessHistory(IEnumerable<TradeInfo> v, OrderBook ob) {
        throw new NotImplementedException();
    }

    public void ProcessRealTimeAt(int i, TradeInfo v, OrderBook ob) {
        if (v.AggressorSide != "S") return;
        ProcessedCount++;
        ob[i, Name] = (ob[i, Name] as long? ?? 0) + v.Size;
    }
}

/// <summary>
///    BuyVolume column represents the total volume of buy trades at the given price.
/// </summary>
public class
    OrderBookBuyVolumeColumn : OrderBookBaseColumn, IOrderBookColumn<TradeInfo> {
    public OrderBookBuyVolumeColumn(
        string name = "BuyVolume",
        Type?  type = null) : base(name, type ?? typeof(long)) {
    }

    public void ProcessHistory(IEnumerable<TradeInfo> v, OrderBook ob) {
        throw new NotImplementedException();
    }

    public void ProcessRealTimeAt(int i, TradeInfo v, OrderBook ob) {
        if (v.AggressorSide != "B") return;
        ProcessedCount++;
        ob[i, Name] = (ob[i, Name] as long? ?? 0) + v.Size;
    }
}

/// <summary>
///     VolumeDelta column represents the total volume of
///     buy trades minus (-) the total volume of sell trades at the given price.
/// </summary>
public class
    OrderBookVolumeDeltaColumn : OrderBookBaseColumn, IOrderBookColumn<TradeInfo> {
    public OrderBookVolumeDeltaColumn(
        string name = "VolumeDelta",
        Type?  type = null) : base(name, type ?? typeof(long)) {
    }

    public void ProcessHistory(IEnumerable<TradeInfo> v, OrderBook ob) {
        throw new NotImplementedException();
    }

    public void ProcessRealTimeAt(int i, TradeInfo v, OrderBook ob) {
        switch (v.AggressorSide) {
            case "B":
                ob[i, Name] = (ob[i, Name] as long? ?? 0) + v.Size;
                break;
            case "S":
                ob[i, Name] = (ob[i, Name] as long? ?? 0) - v.Size;
                break;
        }

        ProcessedCount++;
    }
}