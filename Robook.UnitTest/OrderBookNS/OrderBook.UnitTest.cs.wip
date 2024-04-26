using System.Collections.ObjectModel;
using System.Data;
using Robook.OrderBookNS;

namespace Robook.UnitTest.OrderBookNS;

[TestFixture]
[TestOf(typeof(OrderBook))]
public class OrderBookTest {
    
    [Test]
    public void Test_CreatePriceLevels() {
        var expected = new[] { 0.5m, 0.75m, 1.0m, 1.25m, 1.5m, 1.75m, 2.0m }.Reverse();
        var actual   = OrderBook.NewPriceLevels(0.25m, 1.25m, 3);
        Assert.That(actual, Is.EqualTo(expected));
        Assert.Throws<ExcOrderBookInvalidTickSize>(() => OrderBook.NewPriceLevels(0.25m, 1.26m, 3));
    }
    
    [Test]
    public void TestGetColumnValues() {
        var ob               = new OrderBook(0.25m, 100m, 10);
        var pricesEnumerable = ob.GetColumnValues<decimal>("Price");
        Assert.That(pricesEnumerable.Sum(), Is.EqualTo(ob.PricesArray.Sum()));
    }

    [Test]
    [Description("Tests all of the this[] indexers.")]
    public void TestIndexers() {
        var ob = new OrderBook(0.25m, 100m, 10);
        Assert.Multiple(() => {
            
            // public DataRow this[int     i]
            Assert.That(ob[10]["Price"], Is.EqualTo(100m));
            
            // public DataRow this[decimal p]
            Assert.That(ob[100m]["Price"], Is.EqualTo(100m));
            
            // public DataRow this[double  p]
            Assert.That(ob[100d]["Price"], Is.EqualTo(100m));
            
            // public object this[int i, string c] {
            Assert.That(ob[0, "Price"], Is.EqualTo(102.5m));
            ob[0, "Price"] = 102m;
            Assert.That(ob[0, "Price"], Is.EqualTo(102m));
            
            // public object this[double p, string c] {
            Assert.That(ob[101.25d, "Price"], Is.EqualTo(101.25m));
            ob[101.25d, "Price"] = 102.25m;
            Assert.That(ob[101.25d, "Price"], Is.EqualTo(102.25m));
            
            // public object this[decimal p, string c] {
            Assert.That(ob[101.5m, "Price"], Is.EqualTo(101.5m));
            ob[101.5m, "Price"] = 102.5m;
            Assert.That(ob[101.5m, "Price"], Is.EqualTo(102.5m));
        });
    }

    [Test]
    public void TestAddAndRemoveColumn() {
        var ob = new OrderBook(0.25m, 100m, 10);

        var ask = new OrderBookAskColumn("Ask", typeof(double));
        ob.AddColumn(ask);
        
        Assert.That(ob.OBDT.Columns.Contains("Ask"), Is.True);
        Assert.That(ob.OBDT.Columns["Ask"].DataType, Is.EqualTo(typeof(double)));
        Assert.That(ob.OBCC.ColAskInfo[0], Is.EqualTo(ask));
        
        var bid = new OrderBookBidColumn("Bid", typeof(double));
        ob.AddColumn(bid);
        Assert.That(ob.OBDT.Columns.Contains("Bid"), Is.True);
        Assert.That(ob.OBDT.Columns["Bid"].DataType, Is.EqualTo(typeof(double)));
        Assert.That(ob.OBCC.ColBidInfo[0], Is.EqualTo(bid));
        
        var trade = new OrderBookVolumeColumn("Volume", typeof(double));
        ob.AddColumn(trade);
        Assert.That(ob.OBDT.Columns.Contains("Volume"), Is.True);
        Assert.That(ob.OBDT.Columns["Volume"].DataType, Is.EqualTo(typeof(double)));
        Assert.That(ob.OBCC.ColTradeInfo[0], Is.EqualTo(trade));
        
        var tradeBuy = new OrderBookBuyVolumeColumn("BuyVolume", typeof(long));
        ob.AddColumn(tradeBuy);
        Assert.That(ob.OBDT.Columns.Contains("BuyVolume"), Is.True);
        Assert.That(ob.OBDT.Columns["BuyVolume"].DataType, Is.EqualTo(typeof(long)));
        Assert.That(ob.OBCC.ColTradeInfo[1], Is.EqualTo(tradeBuy));
        
        var tradeSell = new OrderBookSellVolumeColumn("SellVolume", typeof(long));
        ob.AddColumn(tradeSell);
        Assert.That(ob.OBDT.Columns.Contains("SellVolume"), Is.True);
        Assert.That(ob.OBDT.Columns["SellVolume"].DataType, Is.EqualTo(typeof(long)));
        Assert.That(ob.OBCC.ColTradeInfo[2], Is.EqualTo(tradeSell));
        
        ob.RemoveColumn(ask);
        Assert.That(ob.OBDT.Columns.Contains("Ask"), Is.False);
        Assert.That(ob.OBCC.ColAskInfo.Count, Is.EqualTo(0));
        
        ob.RemoveColumn(bid);
        Assert.That(ob.OBDT.Columns.Contains("Bid"), Is.False);
        Assert.That(ob.OBCC.ColBidInfo.Count, Is.EqualTo(0));
        
        ob.RemoveColumn(trade);
        Assert.That(ob.OBDT.Columns.Contains("Volume"), Is.False);
        Assert.That(ob.OBCC.ColTradeInfo.Count, Is.EqualTo(2));
        
        ob.RemoveColumn(tradeBuy);
        Assert.That(ob.OBDT.Columns.Contains("BuyVolume"), Is.False);
        Assert.That(ob.OBCC.ColTradeInfo.Count, Is.EqualTo(1));
        
        ob.RemoveColumn(tradeSell);
        Assert.That(ob.OBDT.Columns.Contains("SellVolume"), Is.False);
        Assert.That(ob.OBCC.ColTradeInfo.Count, Is.EqualTo(0));
        
    }
    
    [Test]
    public void TestAddColumn_ThrowsExcOrderBookColumnAlreadyExists() {
        var ob = new OrderBook(0.25m, 100m, 10);
        var ask = new OrderBookAskColumn("Ask", typeof(double));
        ob.AddColumn(ask);
        Assert.Throws<ExcOrderBookColumnAlreadyExists>(() => ob.AddColumn(ask));
        var ask2 = new OrderBookAskColumn("Ask2", typeof(double));
        ob.AddColumn(ask2);
    }
    
    [Test]
    public void TestRemoveColumn_ThrowsExcOrderBookColumnDoesNotExist() {
        var ob = new OrderBook(0.25m, 100m, 10);
        var ask = new OrderBookAskColumn("Ask", typeof(double));
        Assert.Throws<ExcOrderBookColumnNotFound>(() => ob.RemoveColumn(ask));
    }

    public class UnhandledColumn : OrderBookBaseColumn, IOrderBookColumn<bool> {
        public UnhandledColumn(string name, Type type) : base(name, type) {
        }

        public void ProcessHistory(IEnumerable<bool> v, OrderBook ob) {
            throw new NotImplementedException();
        }

        public void ProcessRealTimeAt(int i, bool v, OrderBook ob) {
            throw new NotImplementedException();
        }
    }
    
    [Test]
    public void TestAddColumn_ThrowsExcOrderBookUnhandledColumnType() {
        var ob  = new OrderBook(0.25m, 100m, 10);
        var col = new UnhandledColumn("", typeof(bool));
        Assert.Throws<ExcOrderBookUnhandledColumnType>(() => ob.AddColumn(col));
    }

    [Test]
    public void TestRemoveColumn_ThrowsExcOrderBookUnhandledColumnType() {
        var ob = new OrderBook(0.25m, 100m, 10);
        var col = new UnhandledColumn("", typeof(bool));
        Assert.Throws<ExcOrderBookUnhandledColumnType>(() => ob.RemoveColumn(col));
    }
    
    [Test]
    public void TestGetIndexOfPrice() {
        var ob = new OrderBook(0.25m, 100m, 10);
        Assert.That(ob.GetIndexOfPrice(100m), Is.EqualTo(10));
        Assert.That(ob.GetIndexOfPrice(100d), Is.EqualTo(10));
        Assert.That(ob.GetIndexOfPrice(100.25m), Is.EqualTo(9));
        Assert.That(ob.GetIndexOfPrice(100.25d), Is.EqualTo(9));
        Assert.Throws<ExcOrderBookPriceNotFound>(() => ob.GetIndexOfPrice(92.75m));
    }
}