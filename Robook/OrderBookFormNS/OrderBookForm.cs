using System.Collections.Concurrent;
using com.omnesys.rapi;
using Rithmic;
using Robook.OrderBookColumns;
using Robook.OrderBookNS;
using Robook.SymbolNS;

namespace Robook.OrderBookFormNS;

public partial class OrderBookForm : BaseForm {
    public IOrderBook              OrderBook;
    public ConcurrentQueue<object> ConcurrentQueue;
    public OrderBookProcessor      OrderBookProcessor;
    public DataGridView            OrderBookDataGridView;
    // public OrderBookDataGridControl OrderBookDataGridControl;

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
        OrderBook             = new OrderBook((decimal)_symbol.TickSize, (decimal)lowPrice, (decimal)highPrice);
        OrderBookDataGridView = new DataGridView();
        OrderBookProcessor    = new OrderBookProcessor(OrderBook, ConcurrentQueue);

        List<DataGridViewColumn> columns = new();

        columns.Add(new PriceColumn {
            DataPropertyName = "Price",
            Name             = "Price"
        });

        OrderBook.ColumnCollection.Add(
            new OrderBookDefaultColumn("Ask", [OrderBookColumnDataType.Ask], typeof(long)));
        OrderBook.DataTable.Columns.Add("Ask", typeof(long));
        columns.Add(new HistogramColumn {
            DataPropertyName = "Ask",
            Name             = "Ask"
        });

        OrderBook.ColumnCollection.Add(
            new OrderBookDefaultColumn("Bid", [OrderBookColumnDataType.Bid], typeof(long)));
        OrderBook.DataTable.Columns.Add("Bid", typeof(long));
        columns.Add(new HistogramColumn {
            DataPropertyName = "Bid",
            Name             = "Bid"
        });

        OrderBook.ColumnCollection.Add(
            new OrderBookDefaultColumn("SellVolume", [OrderBookColumnDataType.TradeSell], typeof(long)));
        OrderBook.DataTable.Columns.Add("SellVolume", typeof(long));
        columns.Add(new HistogramColumn {
            DataPropertyName = "SellVolume",
            Name             = "SellVolume"
        });

        OrderBook.ColumnCollection.Add(
            new OrderBookDefaultColumn("BuyVolume", [OrderBookColumnDataType.TradeBuy], typeof(long)));
        OrderBook.DataTable.Columns.Add("BuyVolume", typeof(long));
        columns.Add(new HistogramColumn {
            DataPropertyName = "BuyVolume",
            Name             = "BuyVolume"
        });

        var c = new OrderBookTouchedTradeColumn(
            "TouchedBuy",
            [
                OrderBookColumnDataType.TradeBuy,
                OrderBookColumnDataType.Bid,
                OrderBookColumnDataType.Ask
            ], typeof(long)
        );

        c.SetUp(OrderBook);
        OrderBook.ColumnCollection.Add(c);
        OrderBook.DataTable.Columns.Add("TouchedBuy", typeof(long));
        columns.Add(new HistogramColumn {
            DataPropertyName = "TouchedBuy",
            Name             = "TouchedBuy",
        });
        
        var c2 = new OrderBookTouchedTradeColumn(
            "TouchedSell",
            [
                OrderBookColumnDataType.TradeSell,
                OrderBookColumnDataType.Bid,
                OrderBookColumnDataType.Ask
            ], typeof(long)
        );
        
        c2.SetUp(OrderBook);
        OrderBook.ColumnCollection.Add(c2);
        OrderBook.DataTable.Columns.Add("TouchedSell", typeof(long));
        columns.Add(new HistogramColumn {
            DataPropertyName = "TouchedSell",
            Name             = "TouchedSell",
        });
        
        var OrderBookDataGridControl = new OrderBookDataGridControl(OrderBook.DataTable, OrderBookDataGridView);
        
        columns.ForEach(column => OrderBookDataGridControl.AddColumn(column));
        
        Invoke(() => {
            panelOrderBook.Controls.Add(OrderBookDataGridView);
            OrderBookDataGridView.Dock = DockStyle.Fill;
        });
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

        _client.RHandler.HighPriceLimitClb.Subscribe(ctx, (_, info) => { highPriceTask.SetResult(info.Price); });
        _client.RHandler.LowPriceLimitClb.Subscribe(ctx, (_,  info) => { lowPriceTask.SetResult(info.Price); });

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

}