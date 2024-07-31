﻿using System.Collections.Concurrent;
using com.omnesys.rapi;
using Rithmic;
using Robook.OrderBookColumns;
using Robook.OrderBookNS;
using Robook.SymbolNS;

namespace Robook.OrderBookFormNS;

public partial class OrderBookForm : BaseForm {
    public IOrderBook               OrderBook;
    public ConcurrentQueue<object>  ConcurrentQueue;
    public OrderBookProcessor       OrderBookProcessor;
    public DataGridView             OrderBookDataGridView;
    public OrderBookDataGridControl OrderBookDataGridControl;

    private bool        _simulationMode = false;
    private State.State _state;

    public OrderBookForm(State.State state) {
        _state = state;
        InitializeComponent();
        SetDoubleBuffered(true);
        accountsSelectControl1.Init(_state.AccountsStore.Accounts);
    }

    private Client _client;
    private Symbol _symbol;

    private void createOrderBook(double lowPrice, double highPrice) {
        OrderBook = new OrderBook((decimal)_symbol.TickSize, (decimal)lowPrice, (decimal)highPrice);

        OrderBookDataGridView    = new DataGridView();
        OrderBookProcessor       = new OrderBookProcessor(OrderBook, ConcurrentQueue);
        OrderBookDataGridControl = new OrderBookDataGridControl(OrderBook.DataTable, OrderBookDataGridView, OrderBookProcessor);
        
        OrderBookDataGridControl.AddColumn(new PriceColumn() {
            DataPropertyName = "Price",
            Name             = "Price",
        });

        OrderBook.AddColumn(new OrderBookDefaultColumn("Ask", new[] { OrderBookColumnDataType.Ask }, typeof(long)));
        OrderBookDataGridControl.AddColumn(new HistogramColumn() {
            DataPropertyName = "Ask",
            Name             = "Ask",
        });

        OrderBook.AddColumn(new OrderBookDefaultColumn("Bid", new[] { OrderBookColumnDataType.Bid }, typeof(long)));
        OrderBookDataGridControl.AddColumn(new HistogramColumn() {
            DataPropertyName = "Bid",
            Name             = "Bid",
        });

        OrderBook.AddColumn(
            new OrderBookDefaultColumn("SellVolume", new[] { OrderBookColumnDataType.Trade }, typeof(long)));
        OrderBookDataGridControl.AddColumn(new HistogramColumn() {
            DataPropertyName = "SellVolume",
            Name             = "SellVolume",
        });

        OrderBook.AddColumn(
            new OrderBookDefaultColumn("BuyVolume", new[] { OrderBookColumnDataType.Trade }, typeof(long)));
        OrderBookDataGridControl.AddColumn(new HistogramColumn() {
            DataPropertyName = "BuyVolume",
            Name             = "BuyVolume",
        });
        //
        // OrderBook.AddColumn(new OrderBookTouchedTradeColumn("TouchedSell",
        //                                                     new[] {
        //                                                         OrderBookColumnDataType.TradeSell,
        //                                                     }, typeof(long)));
        // OrderBookDataGridControl.AddColumn(new HistogramColumn() {
        //     DataPropertyName = "TouchedSell",
        //     Name             = "TouchedSell",
        // });

        var c = new OrderBookTouchedTradeColumn(
            "TouchedBuy",
            new[] {
                OrderBookColumnDataType.TradeBuy,
                OrderBookColumnDataType.Bid,
                OrderBookColumnDataType.Ask
            }, typeof(long)
        );

        c.SetUp(OrderBook);
        OrderBook.AddColumn(c);
        OrderBookDataGridControl.AddColumn(new HistogramColumn() {
            DataPropertyName = "TouchedBuy",
            Name             = "TouchedBuy",
        });

        Invoke(() => {
            panelOrderBook.Controls.Add(OrderBookDataGridView);
            OrderBookDataGridView.Dock = DockStyle.Fill;
        });
        // todo orderbook needs to hold bid/ask data in separate container
        // and if the bid/ask columns are added to the orderbook,
        // it needs to be passed as history to the columns
    }

    private async void button1_Click(object sender, EventArgs e) {
        // check account
        if (accountsSelectControl1.SelectedAccount.Client?.MarketDataConnection?.LastConnectionAlert?.AlertInfo
                                  .AlertType !=
            AlertType.LoginComplete) {
            MessageBox.Show("Please login first");
            return;
        }

        _client = accountsSelectControl1.SelectedAccount.Client;

        // if valid get symbol
        _symbol = symbolTextBoxControl1.GetSymbol(_client);
        var midPrice = 0.0;
        try {
            await _symbol.GetPriceIncrInfoAsync();
            await _symbol.GetRefDataAsync();
            var ob = await _symbol.RebuildBookAsync();
            midPrice = ob.Asks.First().Price;
        }
        catch (Exception exception) {
            MessageBox.Show(exception.Message);
            return;
        }


        ConcurrentQueue = new ConcurrentQueue<object>();

        var ctx = new Context();
        _client.RHandler.LimitOrderBookClb.Subscribe(ctx, (_, info) => {
            foreach (var ask in info.Asks) {
                ConcurrentQueue.Enqueue(ask);
            }

            foreach (var bid in info.Bids) {
                ConcurrentQueue.Enqueue(bid);
            }
        });

        _client.RHandler.BidQuoteClb.Subscribe(ctx, (_,     info) => { ConcurrentQueue.Enqueue(info); });
        _client.RHandler.AskQuoteClb.Subscribe(ctx, (_,     info) => { ConcurrentQueue.Enqueue(info); });
        _client.RHandler.TradePrintClb.Subscribe(ctx, (_,   info) => { ConcurrentQueue.Enqueue(info); });
        _client.RHandler.BestAskQuoteClb.Subscribe(ctx, (_, info) => { ConcurrentQueue.Enqueue(info); });
        _client.RHandler.BestBidQuoteClb.Subscribe(ctx, (_, info) => { ConcurrentQueue.Enqueue(info); });
        _client.RHandler.BestBidAskQuoteClb.Subscribe(ctx, (_, info) => {
            ConcurrentQueue.Enqueue(info);
            // Console.WriteLine(info.AskInfo.Price);
        });
        _client.RHandler.HighBidPriceClb.Subscribe(ctx, (_, info) => { ConcurrentQueue.Enqueue(info); });
        _client.RHandler.LowAskPriceClb.Subscribe(ctx, (_,  info) => { ConcurrentQueue.Enqueue(info); });

        var lowPriceTask  = new TaskCompletionSource<double>();
        var highPriceTask = new TaskCompletionSource<double>();
        
        _client.RHandler.HighPriceLimitClb.Subscribe(ctx, (_, info) => {
            highPriceTask.SetResult(info.Price);
        });
        _client.RHandler.LowPriceLimitClb.Subscribe(ctx, (_, info) => {
            lowPriceTask.SetResult(info.Price);
        });

        // _client.Engine.replayTrades(_symbol.Exchange, _symbol.Name, 0, 0, ctx);
        _client.Engine.subscribe(_symbol.Exchange, _symbol.Name, SubscriptionFlags.All, ctx);

        await Task.Run(() => {
            var completed = Task.WaitAll([lowPriceTask.Task, highPriceTask.Task], 5000);
            if (!completed) {
                MessageBox.Show("Failed to get price limits");
                return;
            }
            
            createOrderBook(lowPriceTask.Task.Result, highPriceTask.Task.Result);
            OrderBookProcessor.StartAsync();
        });
    }

    private void columnsToolStripMenuItem_Click(object sender, EventArgs e) {
    }

    private void toolStripSeparator1_Click(object sender, EventArgs e) {
    }

    public enum VisualMode {
        Histogram,
    }

    public void SetVisualMode(VisualMode mode, string dataPropertyName, string Name) {
        switch (mode) {
            case VisualMode.Histogram:
                OrderBookDataGridControl.AddColumn(new HistogramColumn() {
                    DataPropertyName = dataPropertyName,
                    Name             = Name
                });
                break;
        }
    }

    private async void addToolStripMenuItem_Click(object sender, EventArgs e) {
        var c = new OrderBookDefaultColumn("Volume", new[] { OrderBookColumnDataType.Trade }, typeof(int));
        await OrderBookProcessor.DelayProcessingWith(() => { OrderBook.AddColumn(c); });
        OrderBookDataGridControl.AddColumn(new HistogramColumn() {
            DataPropertyName = "Volume",
            Name             = "Volume"
        });
        Invoke(() => {
            OrderBookDataGridControl.AddColumn(new HistogramColumn() {
                DataPropertyName = "Volume",
                Name             = "Volume"
            });
        });
    }
}