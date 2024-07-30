using System.Collections.Concurrent;
using System.Data;
using com.omnesys.rapi;

namespace Robook.OrderBookNS;

public class OrderBookSimulator {
    private IOrderBook              _orderBook;
    private ConcurrentQueue<object> _concurrentQueue;
    private OrderBookProcessor      _orderBookProcessor;
    private DataTable               _orderBookDataTable;
    private decimal[]               _pricesArr;
    private decimal                 _currentPrice;
    private int                     _currentPriceIndex;

    public OrderBookSimulator(OrderBookProcessor orderBookProcessor) {
        _orderBookProcessor = orderBookProcessor;
        _concurrentQueue    = _orderBookProcessor._qu;
        _orderBook          = _orderBookProcessor._ob;
        _orderBookDataTable = _orderBook.DataTable;
        _pricesArr          = _orderBook.PriceArray;

        // Init with current price at middle index
        _currentPriceIndex = GetMiddleIndexOfArray(_pricesArr);
        _currentPrice      = _pricesArr.ElementAt(_currentPriceIndex);
    }

    public void SimulateOrders() {
        var ct = new CancellationTokenSource();

        Task.Run(() => {
            while (!ct.IsCancellationRequested) {
                long orderSize  = 1;
                var  isBuyOrder = Random.Shared.Next(0, 2) == 0;
                var  order      = NewOrder(isBuyOrder, orderSize);

                _concurrentQueue.Enqueue(order);
                ProcessBidAsk(isBuyOrder, orderSize);
                Thread.Sleep(400);
            }
        });
    }

    /// <summary>
    /// Handles order processing for bid and ask columns in the order book, considering the order type (buy or sell).
    /// If there are existing offers at the current price level, it adjusts sizes accordingly.
    /// If no existing offers are present, it simulates order book behavior by adding offers on the opposite side.
    /// </summary>
    /// <param name="isBuyOrder">A boolean indicating whether the order is a buy order (true) or a sell order (false).</param>
    /// <param name="orderSize">The size or quantity of the order to be processed.</param>
    /// <param name="oppositeSideSize">Size of new opposite-side offers in case of price level change, defaults to a random value if not specified.</param>
    public void ProcessBidAsk(bool isBuyOrder, long orderSize, double? oppositeSideSize = null) {
        // todo: should method accept column name?
        var offers = GetOrderBookOffers(isBuyOrder ? "Ask" : "Bid", _currentPriceIndex);

        if (offers > 0) {
            var offerParams = new {
                Price = (double)_currentPrice,
                Size  = (long)(offers - orderSize)
            };
            _concurrentQueue.Enqueue(
                isBuyOrder
                    ? new AskInfo { Price = offerParams.Price, Size = offerParams.Size }
                    : new BidInfo { Price = offerParams.Price, Size = offerParams.Size }
            );
        }
        else {
            // Simulate order book behavior by adding random offers on the opposite side.
            var offerParams = new {
                Price = (double)_currentPrice,
                Size  = oppositeSideSize ?? Random.Shared.Next(1, 10)
            };
            _concurrentQueue.Enqueue(
                isBuyOrder
                    ? new BidInfo { Price = offerParams.Price, Size = (long)offerParams.Size }
                    : new AskInfo { Price = offerParams.Price, Size = (long)offerParams.Size }
            );

            // Update the price level 
            var newPriceIndex = GetUpdatedPricePosition(_currentPriceIndex, _pricesArr, isBuyOrder);
            _currentPriceIndex = newPriceIndex;
            _currentPrice      = _pricesArr[_currentPriceIndex];
        }
    }

    /// <summary>
    /// Creates a new trade order with the specified parameters.
    /// </summary>
    /// <param name="isBuyOrder">A boolean indicating whether the order is a buy order (true) or a sell order (false).</param>
    /// <returns>A new instance of the TradeInfo class representing the trade order.</returns>
    private TradeInfo NewOrder(bool isBuyOrder, long orderSize) {
        string side = isBuyOrder ? "B" : "S";
        return new TradeInfo { Price = (double)_currentPrice, Size = orderSize, AggressorSide = side };
    }

    /// <summary>
    /// Initializes bid and ask offers in the order book.
    /// </summary>
    /// <param name="bidColName">The column name for bid offers in the order book data table.</param>
    /// <param name="askColName">The column name for ask offers in the order book data table.</param>
    /// <param name="bidSize">The size of the bid offers, defaults to a random value if not specified.</param>
    /// <param name="askSize">The size of the ask offers, defaults to a random value if not specified.</param>
    public void SetupBidAskOffers(string? bidColName = null, string? askColName = null, double? bidSize = null,
                                  double? askSize    = null) {
        int minRandSize = 1;
        int maxRandSize = 10;

        for (int i = _currentPriceIndex; i < _pricesArr.Length; i++) {
            _orderBookDataTable.Rows[i][bidColName ?? "Bid"] = bidSize ?? Random.Shared.Next(minRandSize, maxRandSize);
        }

        for (int i = _currentPriceIndex - 1; i >= 0; i--) {
            _orderBookDataTable.Rows[i][askColName ?? "Ask"] = askSize ?? Random.Shared.Next(minRandSize, maxRandSize);
        }
    }

    /// <summary>
    /// Calculates the updated index of the current price after a movement in the specified direction.
    /// </summary>
    /// <param name="currentPriceIndex">Current index of the price in the array.</param>
    /// <param name="priceArray">Array containing the prices.</param>
    /// <param name="priceDirectionUp">Indicates whether the price should move up (true) or down (false).</param>
    /// <returns>
    /// The updated index of the current price, adjusted based on the specified direction.
    /// </returns>
    /// <remarks>
    /// The method adjusts the index based on the specified direction. If the priceDirectionUp is true,
    /// the index decreases by one, indicating a move towards higher prices. If false, the index
    /// increases by one, indicating a move towards lower prices. The method handles cases where the
    /// calculated index is out of range, ensuring it stays within the valid bounds of the array.
    /// </remarks>
    public static int GetUpdatedPricePosition(int currentPriceIndex, Array priceArray, bool priceDirectionUp) {
        // Adjust index based on direction:  price up = decrease, price down = increase
        int updatedPriceIndex = (priceDirectionUp) ? currentPriceIndex - 1 : currentPriceIndex + 1;

        // Handle cases when updatedPriceIndex is out of range
        if (updatedPriceIndex < 0) {
            updatedPriceIndex = 0;
        }
        else if (updatedPriceIndex >= priceArray.Length) {
            updatedPriceIndex = priceArray.Length - 1;
        }

        return updatedPriceIndex;
    }


    /// <summary>
    /// Gets the index of the middle element in the provided array.
    /// </summary>
    /// <returns>
    /// The index of the middle element. For arrays with an odd length, this is the exact middle element.
    /// For arrays with an even length, this is the index of the element closer to the beginning of the array.
    /// </returns>
    public static int GetMiddleIndexOfArray(Array array) {
        if (array.Length > 0) {
            return (int)Math.Floor((double)array.Length / 2);
        }
        else {
            throw new InvalidOperationException("Array is empty.");
        }
    }

    public decimal GetCurrentPrice() {
        return _currentPrice;
    }

    public int GetCurrentPriceIndex() {
        return _currentPriceIndex;
    }

    public double GetOrderBookOffers(string colName, int priceIndex) {
        var offers                         = _orderBook[priceIndex, colName];
        if (offers == DBNull.Value) offers = 0;

        return Convert.ToDouble(offers);
    }

    public double GetOrderBookOffers(string colName, decimal price) {
        var offers                         = _orderBook[price, colName];
        if (offers == DBNull.Value) offers = 0;

        return Convert.ToDouble(offers);
    }
}