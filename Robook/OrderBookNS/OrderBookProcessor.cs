using System.Collections.Concurrent;
using com.omnesys.rapi;
using Rithmic;

namespace Robook.OrderBookNS;

/// <summary>
///     This class is responsible for dequeuing objects from the queue
///     and passing them to the <see cref="OrderBookColumnCollection"/>
///     based on their type. It also exposes a method to delay the processing
///     of the queue by enqueuing an action that will be executed as soon
///     as the queue is empty which is useful for things like updating the
///     column collection or other UI related tasks that should not be
///     executed while the queue is being processed.
/// </summary>
public class OrderBookProcessor {
    /// <summary>
    ///     <see cref="OrderBook"/> instance passed to the <see cref="IOrderBookColumn{T}"/>s.
    /// </summary>
    public readonly OrderBook _ob;

    /// <summary>
    ///     Queue that holds the objects to be processed by the <see cref="IOrderBookColumn{T}"/>s.
    /// </summary>
    public readonly ConcurrentQueue<object> _qu;

    /// <summary>
    ///     Cancellation token source used to cancel the <see cref="ProcessQueue"/> method.
    /// </summary>
    private readonly CancellationTokenSource _ct = new();

    /// <summary>
    ///     Queue that holds the actions to be executed after the queue is empty.
    /// </summary>
    private readonly ConcurrentQueue<Action> _dq = new();

    /// <summary>
    ///     Initializes a new instance of the <see cref="OrderBookProcessor"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    public OrderBookProcessor(
        OrderBook               orderBook,
        ConcurrentQueue<object> objectQueue
    ) {
        _ob = orderBook ?? throw new ArgumentNullException(nameof(orderBook));
        _qu = objectQueue ?? throw new ArgumentNullException(nameof(objectQueue));
    }

    /// <summary>
    ///     Starts the <see cref="ProcessQueue"/> method in a new thread.
    /// </summary>
    public void StartAsync(int sleep = 1) {
        Task.Run(() => ProcessQueue(_ct.Token, sleep));
    }

    /// <summary>
    ///     Cancels the <see cref="ProcessQueue"/> method.
    /// </summary>
    public void Stop() {
        _ct.Cancel();
    }

    /// <summary>
    ///    Enqueues a column to be added to the <see cref="OrderBook"/>.
    /// </summary>
    public void AddColumn(IOrderBookColumn column) {
        DelayProcessingWith(() => _ob.AddColumn(column));
    }

    /// <summary>
    ///     Enqueues a column to be deleted from the <see cref="OrderBook"/>.
    /// </summary>
    public void RemoveColumn(IOrderBookColumn column) {
        DelayProcessingWith(() => _ob.RemoveColumn(column));
    }

    /// <summary>
    ///     Enqueues an action to be executed after the queue is empty.
    /// </summary>
    /// <param name="action"></param>
    /// <returns> A <see cref="Task"/> that will be completed when the action is executed.</returns>
    public Task DelayProcessingWith(Action action) {
        var tcs = new TaskCompletionSource<bool>();
        var taskToEnqueue = new Action(() => {
            try {
                action();
                tcs.SetResult(true);
            }
            catch (Exception ex) {
                tcs.SetException(ex);
            }
        });
        _dq.Enqueue(taskToEnqueue);
        return tcs.Task;
    }

    /// <summary>
    ///     Processes the queue and dequeues the objects to be processed by the <see cref="IOrderBookColumn{T}"/>s.
    /// </summary>
    /// <param name="cancellationToken"> The cancellation token used to cancel the method. </param>
    /// <returns> A <see cref="Task"/> that will be completed when the method is cancelled. </returns>
    private Task ProcessQueue(CancellationToken cancellationToken, int sleep = 1) {
        
        SpinWait sw = new();
        
        while (!cancellationToken.IsCancellationRequested) {
            while (_dq.TryDequeue(out var delayTask)) {
                delayTask();
            }

            while (_qu.TryDequeue(out var o)) {
                int i;
                switch (o) {
                    case AskInfo x when TryGetPriceIndex(x.Price, out i):
                        _ob.OBCC.ColumnsByDataType[OrderBookColumnDataType.Ask]
                           .ForEach(column => column.ProcessAsk(i, x, _ob));
                        break;

                    case BidInfo x when TryGetPriceIndex(x.Price, out i):
                        _ob.OBCC.ColumnsByDataType[OrderBookColumnDataType.Bid]
                           .ForEach(column => column.ProcessBid(i, x, _ob));
                        break;

                    case TradeInfo x when TryGetPriceIndex(x.Price, out i):
                        _ob.OBCC.ColumnsByDataType[OrderBookColumnDataType.Trade]
                           .ForEach(column => column.ProcessTrade(i, x, _ob));

                        if (x.AggressorSide == "B")
                            _ob.OBCC.ColumnsByDataType[OrderBookColumnDataType.TradeBuy]
                               .ForEach(column => column.ProcessTrade(i, x, _ob));
                        else
                            _ob.OBCC.ColumnsByDataType[OrderBookColumnDataType.TradeSell]
                               .ForEach(column => column.ProcessTrade(i, x, _ob));
                        break;

                    case BestBidQuoteInfo x when TryGetPriceIndex(x.BidInfo.Price, out i):
                        _ob.OBCC.ColumnsByDataType[OrderBookColumnDataType.BestBid]
                           .ForEach(column => column.ProcessBestBid(i, x, _ob));
                        break;

                    case BestAskQuoteInfo x when TryGetPriceIndex(x.AskInfo.Price, out i):
                        _ob.OBCC.ColumnsByDataType[OrderBookColumnDataType.BestAsk]
                           .ForEach(column => column.ProcessBestAsk(i, x, _ob));
                        break;
                }
            }
            sw.SpinOnce();
        }

        return Task.CompletedTask;
    }

    /// <summary>
    ///     Tries to get the index of the price in the <see cref="OrderBook.PriceIndexMap"/>.
    /// </summary>
    /// <returns> True if the price was found.</returns>
    private bool TryGetPriceIndex(double price, out int index) {
        return _ob.PriceIndexMap.TryGetValue((decimal)price, out index);
    }
}