﻿using System.Collections.Concurrent;
using Robook.OrderBookColumns;
using Robook.OrderBookNS;
using HorizontalAlignment = Robook.Helpers.HorizontalAlignment;

namespace Robook.OrderBookFormNS;

public partial class OrderBookFormSimulation : Form {
    public IOrderBook               OrderBook;
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
        OrderBookDataGridControl = new OrderBookDataGridControl(OrderBook.DataTable, OrderBookDataGridView);

        OrderBookSimulator = new OrderBookSimulator(OrderBookProcessor);

        orderBookPanel.Controls.Add(OrderBookDataGridView);
        OrderBookDataGridView.Dock = DockStyle.Fill;

        Task.Run(async () => {
            OrderBookProcessor.StartAsync();
            // Ask
            OrderBook.ColumnCollection.Add(
                new OrderBookDefaultColumn("Ask", [OrderBookColumnDataType.Ask], typeof(decimal)));
            OrderBook.DataTable.Columns.Add("Ask", typeof(decimal));
            // Bid
            OrderBook.ColumnCollection.Add(
                new OrderBookDefaultColumn("Bid", [OrderBookColumnDataType.Bid], typeof(decimal)));
            OrderBook.DataTable.Columns.Add("Bid", typeof(decimal));
            // Volume
            OrderBook.ColumnCollection.Add(
                new OrderBookDefaultColumn("Volume", [OrderBookColumnDataType.Trade], typeof(long)));
            OrderBook.DataTable.Columns.Add("Volume", typeof(long));
            // BuyVolume
            OrderBook.ColumnCollection.Add(
                new OrderBookDefaultColumn("BuyVolume", [OrderBookColumnDataType.Trade], typeof(long)));
            OrderBook.DataTable.Columns.Add("BuyVolume", typeof(long));
            // SellVolume
            OrderBook.ColumnCollection.Add(
                new OrderBookDefaultColumn("SellVolume", [OrderBookColumnDataType.Trade], typeof(long)));
            OrderBook.DataTable.Columns.Add("SellVolume", typeof(long));
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

        InitializeFontOptions();
    }

    private AbstractOrderBookColumn _colDgv = new HistogramColumn() {
        DataPropertyName = "Volume",
        Name             = "Volume",
        HeaderText       = "Mój wolumen"
    };

    private async void addButton_Click(object sender, EventArgs e) {
        Invoke(() => { OrderBookDataGridControl.AddColumn(_colDgv); });
    }

    private async void removeButton_Click(object sender, EventArgs e) {
        Invoke(() => { OrderBookDataGridControl.RemoveColumn(_colDgv); });
    }

    private void InitializeFontOptions() {
        fontFamilyComboBox.Items.AddRange(FontFamily.Families.Select(f => f.Name).ToArray());
        fontFamilyComboBox.SelectedIndexChanged += ApplyFontStyle;

        fontSizeNumericUpDown.Value        =  (decimal)DataGridView.DefaultFont.Size;
        fontSizeNumericUpDown.ValueChanged += ApplyFontStyle;

        boldCheckBox.CheckedChanged   += ApplyFontStyle;
        italicCheckBox.CheckedChanged += ApplyFontStyle;
    }

    private void ApplyFontStyle(object sender, EventArgs e) {
        FontStyle fontStyle                   = FontStyle.Regular;
        if (boldCheckBox.Checked) fontStyle   |= FontStyle.Bold;
        if (italicCheckBox.Checked) fontStyle |= FontStyle.Italic;

        string fontFamily = (fontFamilyComboBox.SelectedItem ?? DataGridView.DefaultFont.FontFamily).ToString();
        float  fontSize   = (float)fontSizeNumericUpDown.Value;

        if (OrderBookDataGridView != null) {
            OrderBookDataGridView.DefaultCellStyle.Font = new Font(fontFamily, fontSize, fontStyle);
            OrderBookDataGridView.AutoResizeRows();
        }
    }
}