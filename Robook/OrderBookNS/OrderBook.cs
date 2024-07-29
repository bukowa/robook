using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace Robook.OrderBookNS;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public class ExcOrderBookInvalidTickSize(
    decimal tickSize,
    decimal middPrice,
    int     levels
) : Exception {
    public readonly int     Levels      = levels;
    public readonly decimal MiddlePrice = middPrice;
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
    DataTable                 OBDT          { get; }
    OrderBookColumnCollection OBCC          { get; }
    Dictionary<decimal, int>  PriceIndexMap { get; }
    object this[int     i, string c] { get; set; }
    object this[double  i, string c] { get; set; }
    object this[decimal i, string c] { get; set; }

    decimal[] Prices { get; }

    int GetIndexOfPrice(decimal price);
    int GetIndexOfPrice(double  price);

    void AddColumn(IOrderBookColumn    column);
    void RemoveColumn(IOrderBookColumn column);

    public IEnumerable<T?> GetColumnValues<T>(string columnName) where T : struct;
}

/// <summary>
///     <see cref="OrderBook"/> represents the order book of a single <see cref="Symbol"/>.
///     This is a wrapper around the <see cref="DataTable"/> and <see cref="DataColumnCollection"/>.
/// </summary>
public class OrderBookSimple : IOrderBook {
    public readonly int     Levels;
    public readonly decimal TickSize;
    public readonly decimal MidPrice;

    public DataTable                 OBDT { get; } = new();
    public OrderBookColumnCollection OBCC { get; } = new();

    public DataRow this[int     i] => OBDT.Rows[i];
    public DataRow this[decimal p] => OBDT.Rows[GetIndexOfPrice(p)];
    public DataRow this[double  p] => OBDT.Rows[GetIndexOfPrice(p)];

    public decimal[] Prices { get; }

    public Dictionary<decimal, int> PriceIndexMap { get; } = new();

    /// <summary>
    ///     Returns or sets the value of the cell at the specified index and column.
    /// </summary>
    /// <param name="i"> Row index </param>
    /// <param name="c"> Column name </param>
    public object this[int i, string c] {
        get => OBDT.Rows[i][c];
        set => OBDT.Rows[i][c] = value;
    }

    /// <summary>
    ///     Returns or sets the value of the cell at the specified price and column. 
    /// </summary>
    /// <param name="p"> Price </param>
    /// <param name="c"> Column name </param>
    public object this[double p, string c] {
        get => OBDT.Rows[GetIndexOfPrice(p)][c];
        set => OBDT.Rows[GetIndexOfPrice(p)][c] = value;
    }

    /// <summary>
    ///     Returns or sets the value of the cell at the specified price and column.
    /// </summary>
    /// <param name="p"> Price </param>
    /// <param name="c"> Column name </param>
    public object this[decimal p, string c] {
        get => OBDT.Rows[GetIndexOfPrice(p)][c];
        set => OBDT.Rows[GetIndexOfPrice(p)][c] = value;
    }

    /// <summary>
    ///     Creates a new <see cref="OrderBook"/> with the specified tick size, mid price and levels.
    /// </summary>
    /// <param name="tickSize">Tick size</param>
    /// <param name="midPrice">Middle price at which the order book is centered</param>
    /// <param name="levels">Number of levels on each side of the middle price</param>
    public OrderBookSimple(
        decimal tickSize,
        decimal midPrice,
        int     levels
    ) {
        TickSize = tickSize;
        MidPrice = midPrice;
        Levels   = levels;

        OBDT.Columns.Add("Price", typeof(decimal));
        Prices = NewPriceLevels(TickSize, MidPrice, Levels);
        for (var i = 0; i < Prices.Length; i++) {
            PriceIndexMap[Prices[i]] = i;
            OBDT.Rows.Add(Prices[i]);
        }
    }

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
    ///     Returns enumerable of all values in the specified column.
    /// </summary>
    /// <param name="columnName">Name of the column.</param>
    /// <typeparam name="T">Type to which the values are casted.</typeparam>
    /// <returns>Enumerable of values.</returns>
    public IEnumerable<T?> GetColumnValues<T>(string columnName) where T : struct {
        return OBDT.AsEnumerable().Select(row => row.Field<T?>(columnName));
    }

    /// <summary>
    ///     Adds a column to the order book.
    /// </summary>
    /// <param name="column">Column to add.</param>
    /// <exception cref="ExcOrderBookColumnAlreadyExists"></exception>
    /// <exception cref="ExcOrderBookUnhandledColumnType"></exception>
    public void AddColumn(
        IOrderBookColumn column
    ) {
        OBCC.Add(column);
        OBDT.Columns.Add(column.Name, column.Type);
    }

    /// <summary>
    ///     Removes a column from the order book.
    /// </summary>
    /// <param name="column">Column to remove.</param>
    /// <exception cref="ExcOrderBookUnhandledColumnType"></exception>
    /// <exception cref="ExcOrderBookColumnNotFound"></exception>
    public void RemoveColumn(
        IOrderBookColumn column
    ) {
        OBCC.Remove(column);
        OBDT.Columns.Remove(column.Name);
    }

    /// <summary>
    ///     Returns the index of the specified price.
    /// </summary>
    /// <param name="price">Price.</param>
    /// <returns>Index of the price.</returns>
    /// <exception cref="ExcOrderBookPriceNotFound"></exception>
    public int GetIndexOfPrice(decimal price) {
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
    public int GetIndexOfPrice(double price) {
        return GetIndexOfPrice((decimal)price);
    }
}