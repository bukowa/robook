﻿using System.Collections.Concurrent;
using com.omnesys.rapi;

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
    ///     Cancellation token source used to cancel the <see cref="ProcessQueueAsync(CancellationToken)"/> method.
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
        _qu = objectQueue ?? throw new ArgumentNullException(nameof(objectQueue));
        _ob = orderBook ?? throw new ArgumentNullException(nameof(orderBook));
    }

    /// <summary>
    ///     Starts the <see cref="ProcessQueueAsync(CancellationToken)"/> method in a new thread.
    /// </summary>
    public void Start(int sleep=1) {
        Task.Run(() => ProcessQueueAsync(_ct.Token, sleep));
    }

    /// <summary>
    ///     Cancels the <see cref="ProcessQueueAsync(CancellationToken)"/> method.
    /// </summary>
    public void Stop() {
        _ct.Cancel();
    }
    
    /// <summary>
    ///    Enqueues a column to be added to the <see cref="OrderBook"/>.
    /// </summary>
    public void AddColumn<T>(IOrderBookColumn<T> column) {
        DelayProcessingWith(() => _ob.AddColumn(column));
    }
    
    /// <summary>
    ///     Enqueues a column to be deleted from the <see cref="OrderBook"/>.
    /// </summary>
    public void RemoveColumn<T>(IOrderBookColumn<T> column) {
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
    ///     Processes the columns of the <see cref="OrderBookColumnCollection"/> based on the type of the item.
    /// </summary>
    private void ProcessColumns<T>(List<IOrderBookColumn<T>> columns, int index, T item) {
        foreach (var column in columns) {
            column.ProcessRealTimeAt(index, item, _ob);
        }
    }

    /// <summary>
    ///     Processes the queue and dequeues the objects to be processed by the <see cref="IOrderBookColumn{T}"/>s.
    /// </summary>
    /// <param name="cancellationToken"> The cancellation token used to cancel the method. </param>
    /// <returns> A <see cref="Task"/> that will be completed when the method is cancelled. </returns>
    private Task ProcessQueueAsync(CancellationToken cancellationToken, int sleep=1) {
        while (!cancellationToken.IsCancellationRequested) {
            while (_dq.TryDequeue(out var delayTask)) {
                delayTask();
            }
            while (_qu.TryDequeue(out var o)) {
                int i;
                switch (o) {
                    case AskInfo x when TryGetPriceIndex(x.Price, out i):
                        ProcessColumns(_ob.OBCC.ColAskInfo, i, x);
                        break;

                    case BidInfo x when TryGetPriceIndex(x.Price, out i):
                        ProcessColumns(_ob.OBCC.ColBidInfo, i, x);
                        break;

                    case TradeInfo x when TryGetPriceIndex(x.Price, out i):
                        ProcessColumns(_ob.OBCC.ColTradeInfo, i, x);
                        break;
                }
            }
            // keeps the CPU usage low
            Thread.Sleep(1);
            //Task.Delay(sleep, cancellationToken);
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