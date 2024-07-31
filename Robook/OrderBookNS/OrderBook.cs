using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace Robook.OrderBookNS;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public class ExcOrderBookInvalidTickSize(
    decimal tickSize,
    decimal midPrice,
    int     levels
) : Exception {
    public readonly int     Levels      = levels;
    public readonly decimal MiddlePrice = midPrice;
    public readonly decimal TickSize    = tickSize;

    public override string Message
        => $"Invalid tick size: {TickSize} for price {MiddlePrice}";
}

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public class ExcOrderBookPriceNotFound(decimal price) : Exception {
    public readonly decimal Price = price;

    public override string Message
        => $"Price {Price} not found in OrderBook";
}

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public class ExcOrderBookColumnAlreadyExists(string name) : Exception {
    public readonly string ColumnName = name;

    public override string Message
        => $"Column {ColumnName} already exists in OrderBook";
}

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public class ExcOrderBookColumnNotFound(string name) : Exception {
    public readonly string ColumnName = name;

    public override string Message
        => $"Column {ColumnName} not found in OrderBook";
}

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public class ExcOrderBookUnhandledColumnType : Exception {
    public readonly Type Type;

    public ExcOrderBookUnhandledColumnType(Type type) {
        Type = type;
    }

    public override string Message
        => $"Unhandled column type: {Type}";
}

public interface IOrderBook {
    DataTable                 DataTable        { get; }
    OrderBookColumnCollection ColumnCollection { get; }
    Dictionary<decimal, int>  PriceIndexMap    { get; }
    object this[int     index, string columnName] { get; set; }
    object this[double  price, string columnName] { get; set; }
    object this[decimal price, string columnName] { get; set; }
    decimal[] PriceArray { get; }
}

/// <summary>
///     <see cref="OrderBook"/> represents the order book of a single <see cref="Symbol"/>.
///     This is a wrapper around the <see cref="System.Data.DataTable"/> and <see cref="DataColumnCollection"/>.
/// </summary>
// [SuppressMessage("ReSharper", "UnusedMember.Global")]
public class OrderBook : IOrderBook {
    public readonly int     Levels;
    public readonly decimal TickSize;
    public readonly decimal MidPrice;

    /// <summary>
    /// Creates a new <see cref="IOrderBook"/> with the specified tick size, mid price and levels.
    /// </summary>
    /// <param name="tickSize">Tick size</param>
    /// <param name="midPrice">Middle price at which the order book is centered</param>
    /// <param name="levels">Number of levels on each side of the middle price</param>
    public OrderBook(
        decimal tickSize,
        decimal midPrice,
        int     levels
    ) {
        TickSize = tickSize;
        MidPrice = midPrice;
        Levels   = levels;

        DataTable.Columns.Add("Price", typeof(decimal));
        PriceArray = NewPriceLevels(TickSize, MidPrice, Levels);
        for (var i = 0; i < PriceArray.Length; i++) {
            PriceIndexMap[PriceArray[i]] = i;
            DataTable.Rows.Add(PriceArray[i]);
        }
    }

    /// <summary>
    /// Creates a new <see cref="IOrderBook"/> with the specified tick size, mid price and levels.
    /// </summary>
    public OrderBook(
        decimal tickSize,
        decimal lowPrice,
        decimal highPrice
    ) {
        TickSize = tickSize;

        // create new price levels from low to high based on tick size
        var prices = new List<decimal>();

        for (var i = lowPrice; i <= highPrice; i += tickSize) {
            prices.Add(i);
        }

        prices.Reverse();
        DataTable.Columns.Add("Price", typeof(decimal));
        PriceArray = prices.ToArray();

        for (var i = 0; i < PriceArray.Length; i++) {
            PriceIndexMap[PriceArray[i]] = i;
            DataTable.Rows.Add(PriceArray[i]);
        }
    }

    /// <summary>
    /// This table holds the displayed order book data (in the actual form).
    /// </summary>
    public DataTable DataTable { get; } = new();

    /// <summary>
    /// This collection holds the columns of the order book.
    /// They modify the data in the <see cref="DataTable"/>.
    /// </summary>
    public OrderBookColumnCollection ColumnCollection { get; } = new();

    /// <summary>
    /// Returns the row at the specified index.
    /// </summary>
    /// <param name="index"></param>
    public DataRow this[int index] => DataTable.Rows[index];

    /// <summary>
    /// Returns the row at the specified price.
    /// </summary>
    /// <param name="price"></param>
    public DataRow this[double price] => DataTable.Rows[GetIndexOfPrice(price)];

    /// <summary>
    /// Returns the row at the specified price.
    /// </summary>
    /// <param name="price"></param>
    public DataRow this[decimal price] => DataTable.Rows[GetIndexOfPrice(price)];

    public Dictionary<decimal, int> PriceIndexMap { get; } = new();

    /// <summary>
    /// Returns or sets the value of the cell at the specified index and column.
    /// </summary>
    /// <param name="index"> Row index </param>
    /// <param name="columnName"> Column name </param>
    public object this[int index, string columnName] {
        get => DataTable.Rows[index][columnName];
        set => DataTable.Rows[index][columnName] = value;
    }

    /// <summary>
    /// Returns or sets the value of the cell at the specified price and column. 
    /// </summary>
    /// <param name="price"> Price </param>
    /// <param name="columnName"> Column name </param>
    public object this[double price, string columnName] {
        get => DataTable.Rows[GetIndexOfPrice(price)][columnName];
        set => DataTable.Rows[GetIndexOfPrice(price)][columnName] = value;
    }

    /// <summary>
    /// Returns or sets the value of the cell at the specified price and column.
    /// </summary>
    /// <param name="price"> Price </param>
    /// <param name="columnName"> Column name </param>
    public object this[decimal price, string columnName] {
        get => DataTable.Rows[GetIndexOfPrice(price)][columnName];
        set => DataTable.Rows[GetIndexOfPrice(price)][columnName] = value;
    }

    /// <summary>
    /// Original price levels that should also be represented in the <see cref="DataTable"/>.
    /// </summary>
    public decimal[] PriceArray { get; }

    /// <summary>
    ///     Creates an array of prices centered around the middle price.
    /// </summary>
    /// <param name="tickSize">Tick size</param>
    /// <param name="midPrice">Middle price from which the levels are centered</param>
    /// <param name="levels">Number of levels on each side of the middle price</param>
    /// <returns>Array of prices</returns>
    /// <exception cref="ExcOrderBookInvalidTickSize"></exception>
    private static decimal[] NewPriceLevels(
        decimal tickSize,
        decimal midPrice,
        int     levels
    ) {
        if (midPrice % tickSize != 0)
            throw new ExcOrderBookInvalidTickSize(tickSize, midPrice, levels);

        var priceIndexSequence = new decimal[levels * 2 + 1];
        priceIndexSequence[levels] = midPrice;

        for (var i = 1; i <= levels; i++) {
            priceIndexSequence[levels - i] = midPrice - i * tickSize;
            priceIndexSequence[levels + i] = midPrice + i * tickSize;
        }

        Array.Reverse(priceIndexSequence);
        return priceIndexSequence;
    }

    /// <summary>
    ///     Returns the index of the specified price.
    /// </summary>
    /// <param name="price">Price.</param>
    /// <returns>Index of the price.</returns>
    /// <exception cref="ExcOrderBookPriceNotFound"></exception>
    private int GetIndexOfPrice(decimal price) {
        if (PriceIndexMap.TryGetValue(price, out var index))
            return index;
        throw new ExcOrderBookPriceNotFound(price);
    }

    /// <summary>
    ///     Returns the index of the specified price.
    /// </summary>
    /// <param name="price">Price.</param>
    /// <returns>Index of the price.</returns>
    /// <exception cref="ExcOrderBookPriceNotFound"></exception>
    private int GetIndexOfPrice(double price) {
        return GetIndexOfPrice((decimal)price);
    }
}