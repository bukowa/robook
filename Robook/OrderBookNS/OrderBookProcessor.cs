﻿using System.Collections.Concurrent;
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
    /// Collection of columns to process.
    /// </summary>
    public readonly IOrderBook _ob;

    /// <summary>
    /// Queue that holds the objects to be processed.
    /// </summary>
    public readonly ConcurrentQueue<object> _qu;

    /// <summary>
    /// Token used to cancel the processing of the queue.
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
        IOrderBook              orderBook,
        ConcurrentQueue<object> objectQueue
    ) {
        _ob = orderBook;
        _qu = objectQueue;
    }

    /// <summary>
    ///     Starts the <see cref="ProcessQueue"/> method in a new thread.
    /// </summary>
    public void StartAsync() {
        Task.Run(() => ProcessQueue(_ct.Token));
    }

    /// <summary>
    ///     Cancels the <see cref="ProcessQueue"/> method.
    /// </summary>
    public void Stop() {
        _ct.Cancel();
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
    /// Processes the queue and passes the objects to the <see cref="OrderBookColumnCollection"/>.
    /// </summary>
    /// <param name="cancellationToken"> The cancellation token used to cancel the method. </param>
    /// <returns> A <see cref="Task"/> that will be completed when the method is cancelled. </returns>
    private Task ProcessQueue(CancellationToken cancellationToken) {
        SpinWait sw = new();

        while (!cancellationToken.IsCancellationRequested) {
            while (_dq.TryDequeue(out var delayTask)) {
                delayTask();
            }

            while (_qu.TryDequeue(out var o)) {
                int i;
                switch (o) {
                    case AskInfo x:
                        _ob.ColumnCollection.ColumnsByDataType[OrderBookColumnDataType.Ask]
                           .ForEach(column => column.ProcessAsk(x, _ob));
                        break;

                    case BidInfo x:
                        _ob.ColumnCollection.ColumnsByDataType[OrderBookColumnDataType.Bid]
                           .ForEach(column => column.ProcessBid(x, _ob));
                        break;

                    case TradeInfo x:
                        _ob.ColumnCollection.ColumnsByDataType[OrderBookColumnDataType.Trade]
                           .ForEach(column => column.ProcessTrade(x, _ob));

                        if (x.AggressorSide == "B")
                            _ob.ColumnCollection.ColumnsByDataType[OrderBookColumnDataType.TradeBuy]
                               .ForEach(column => column.ProcessTrade(x, _ob));
                        else
                            _ob.ColumnCollection.ColumnsByDataType[OrderBookColumnDataType.TradeSell]
                               .ForEach(column => column.ProcessTrade(x, _ob));
                        break;

                    case BestBidQuoteInfo x:
                        _ob.ColumnCollection.ColumnsByDataType[OrderBookColumnDataType.BestBid]
                           .ForEach(column => column.ProcessBestBid(x, _ob));
                        break;

                    case BestAskQuoteInfo x:
                        _ob.ColumnCollection.ColumnsByDataType[OrderBookColumnDataType.BestAsk]
                           .ForEach(column => column.ProcessBestAsk(x, _ob));
                        break;
                }
            }

            sw.SpinOnce();
        }

        return Task.CompletedTask;
    }
}