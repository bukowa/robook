﻿using System.Collections.Concurrent;
using System.Data;
using com.omnesys.rapi;
using Rithmic;
using Robook.OrderBookColumns;
using Robook.OrderBookNS;
using Robook.SymbolNS;

namespace Robook.OrderBookFormNS;

public partial class OrderBookForm : BaseForm {

    public OrderBook                OrderBook;
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

    private async void button1_Click(object sender, EventArgs e) {
        // check account
        if (accountsSelectControl1.SelectedAccount.Client?.MarketDataConnection?.LastAlertInfo?.AlertType !=
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

        // create order book
        OrderBook = new OrderBook((decimal)_symbol.TickSize, (decimal)midPrice, 1000);
        OrderBookDataGridView    = new DataGridView();
        ConcurrentQueue          = new ConcurrentQueue<object>();
        OrderBookProcessor       = new OrderBookProcessor(OrderBook, ConcurrentQueue);
        OrderBookDataGridControl = new OrderBookDataGridControl(OrderBook, OrderBookDataGridView, OrderBookProcessor);
        
        OrderBookDataGridControl.AddColumn(new PriceColumn() {
            DataPropertyName = "Price",
            Name             = "Price",
        });
        
        OrderBook.AddColumn(new OrderBookAskColumnLong());
        OrderBookDataGridControl.AddColumn(new HistogramColumn() {
            DataPropertyName = "Ask",
            Name             = "Ask",
        });
        
        OrderBook.AddColumn(new OrderBookBidColumnLong());
        OrderBookDataGridControl.AddColumn(new HistogramColumn() {
            DataPropertyName = "Bid",
            Name             = "Bid",
        });
        
        OrderBook.AddColumn(new OrderBookSellVolumeColumn());
        OrderBookDataGridControl.AddColumn(new HistogramColumn() {
            DataPropertyName = "SellVolume",
            Name             = "SellVolume",
        });
        OrderBook.AddColumn(new OrderBookBuyVolumeColumn());
        OrderBookDataGridControl.AddColumn(new HistogramColumn() {
            DataPropertyName = "BuyVolume",
            Name             = "BuyVolume",
        });
        Invoke(() => {
            panelOrderBook.Controls.Add(OrderBookDataGridView);
            OrderBookDataGridView.Dock = DockStyle.Fill;
        });
        OrderBookProcessor.Start();
        
        // todo orderbook needs to hold bid/ask data in separate container
        // and if the bid/ask columns are added to the orderbook,
        // it needs to be passed as history to the columns
         
        var ctx = new Context();
        _client.RHandler.LimitOrderBookClb.Subscribe(ctx, (_, info) => {
            foreach (var ask in info.Asks) {
                ConcurrentQueue.Enqueue(ask);
            }
            foreach (var bid in info.Bids) {
                ConcurrentQueue.Enqueue(bid);
            }
        });
        
        _client.RHandler.BidQuoteClb.Subscribe(ctx, (_, info) => { ConcurrentQueue.Enqueue(info); });
        _client.RHandler.AskQuoteClb.Subscribe(ctx, (_, info) => { ConcurrentQueue.Enqueue(info); });
        _client.RHandler.TradePrintClb.Subscribe(ctx, (_, info) => { ConcurrentQueue.Enqueue(info); });
        _client.Engine.replayTrades(_symbol.Exchange, _symbol.Name, 0, 0, ctx);
        _client.Engine.subscribe(_symbol.Exchange, _symbol.Name, SubscriptionFlags.All, ctx);
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
        var c = new OrderBookVolumeColumn() {
            Name = "Volume",
        };
        await OrderBookProcessor.DelayProcessingWith(() => {
            OrderBook.AddColumn(c);
        });
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