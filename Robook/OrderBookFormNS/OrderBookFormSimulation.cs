using System.Collections.Concurrent;
using Robook.OrderBookColumns;
using Robook.OrderBookNS;
using HorizontalAlignment = Robook.Helpers.HorizontalAlignment;

namespace Robook.OrderBookFormNS;

public partial class OrderBookFormSimulation : Form {
    public OrderBook                OrderBook;
    public ConcurrentQueue<object>  ConcurrentQueue;
    public OrderBookProcessor       OrderBookProcessor;
    public DataGridView             OrderBookDataGridView;
    public OrderBookDataGridControl OrderBookDataGridControl;
    public OrderBookSimulator       OrderBookSimulator;

    public OrderBookFormSimulation() {
        InitializeComponent();

        OrderBook                = new OrderBook(0.25m, 100m, 100);
        ConcurrentQueue          = new ConcurrentQueue<object>();
        OrderBookProcessor       = new OrderBookProcessor(OrderBook, ConcurrentQueue);
        OrderBookDataGridView    = new DataGridView();
        OrderBookDataGridControl = new OrderBookDataGridControl(OrderBook, OrderBookDataGridView, OrderBookProcessor);

        OrderBookSimulator = new OrderBookSimulator(OrderBookProcessor);

        orderBookPanel.Controls.Add(OrderBookDataGridView);
        OrderBookDataGridView.Dock = DockStyle.Fill;

        Task.Run(async () => {
            OrderBookProcessor.Start();

            OrderBook.AddColumn(new OrderBookAskColumn());
            OrderBook.AddColumn(new OrderBookBidColumn());
            OrderBook.AddColumn(new OrderBookVolumeColumn());
            OrderBook.AddColumn(new OrderBookBuyVolumeColumn());
            OrderBook.AddColumn(new OrderBookSellVolumeColumn());
        });

        var buyVolumeColumn = new BuyVolumeColumn() {
            DataPropertyName            = "BuyVolume",
            Name                        = "BuyVolume",
            CalculateBasedOnTradeVolume = true,
            HistogramAlignment          = HorizontalAlignment.Center,
        };
        buyVolumeColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

        var sellVolumeColumn = new SellVolumeColumn() {
            DataPropertyName   = "SellVolume",
            Name               = "SellVolume",
            HistogramAlignment = HorizontalAlignment.Right,
        };
        sellVolumeColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

        OrderBookDataGridControl.AddColumn(new PriceColumn() { DataPropertyName   = "Price", Name = "Price" });
        OrderBookDataGridControl.AddColumn(new BidSizeColumn() { DataPropertyName = "Bid", Name   = "Bid" });
        OrderBookDataGridControl.AddColumn(new AskSizeColumn() { DataPropertyName = "Ask", Name   = "Ask" });
        OrderBookDataGridControl.AddColumn(buyVolumeColumn);
        OrderBookDataGridControl.AddColumn(sellVolumeColumn);

        OrderBookSimulator.SetupBidAskOffers();
        OrderBookSimulator.SimulateOrders();
    }

    private OrderBookVolumeColumn _colOb = new() {
        Name = "Volume",
    };

    private AbstractOrderBookColumn _colDgv = new HistogramColumn() {
        DataPropertyName = "Volume",
        Name             = "Volume",
        HeaderText       = "Mój wolumen"
    };

    private async void addButton_Click(object sender, EventArgs e) {
        await OrderBookProcessor.DelayProcessingWith(() => { OrderBook.AddColumn(_colOb); });
        Invoke(() => { OrderBookDataGridControl.AddColumn(_colDgv); });
    }

    private async void removeButton_Click(object sender, EventArgs e) {
        await OrderBookProcessor.DelayProcessingWith(() => { OrderBook.RemoveColumn(_colOb); });
        Invoke(() => { OrderBookDataGridControl.RemoveColumn(_colDgv); });
    }
}