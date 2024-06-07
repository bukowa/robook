using com.omnesys.rapi;
using Robook.OrderBookNS;

namespace Robook.UnitTest.OrderBookUnitTest;

[TestFixture]
public class OrderBookColumnTest {
    
    OrderBook orderBook;
    
    [SetUp]
    public void Setup() {
        orderBook = new OrderBook(1, 100, 50);
    }
    
    [Test]
    public void TestOrderBookAskColumnInit() {
        var column = new OrderBookAskColumn();
        Assert.That(column.Name, Is.EqualTo("Ask"));
        Assert.That(column.Type, Is.EqualTo(typeof(double)));
    }
    [Test]
    public void TestOrderBookBidColumnInit() {
        var column = new OrderBookBidColumn();
        Assert.That(column.Name, Is.EqualTo("Bid"));
        Assert.That(column.Type, Is.EqualTo(typeof(double)));
    }
    [Test]
    public void TestOrderBookVolumeColumnInit() {
        var column = new OrderBookVolumeColumn();
        Assert.That(column.Name, Is.EqualTo("Volume"));
        Assert.That(column.Type, Is.EqualTo(typeof(long)));
    }
    [Test]
    public void TestOrderBookBuyVolumeColumnInit() {
        var column = new OrderBookBuyVolumeColumn();
        Assert.That(column.Name, Is.EqualTo("BuyVolume"));
        Assert.That(column.Type, Is.EqualTo(typeof(long)));
    }
    [Test]
    public void TestOrderBookSellVolumeColumnInit() {
        var column = new OrderBookSellVolumeColumn();
        Assert.That(column.Name, Is.EqualTo("SellVolume"));
        Assert.That(column.Type, Is.EqualTo(typeof(long)));
    }
    [Test]
    public void TestOrderBookVolumeDeltaColumnInit() {
        var column = new OrderBookVolumeDeltaColumn();
        Assert.That(column.Name, Is.EqualTo("VolumeDelta"));
        Assert.That(column.Type, Is.EqualTo(typeof(long)));
    }
    
    [Test]
    public void TestOrderBookAskColumnProcessAt() {
        var column = new OrderBookAskColumn();
        orderBook.AddColumn(column);
        column.ProcessRealTimeAt(1, new AskInfo(){Price = 100, Size = 100}, orderBook);
        Assert.That(orderBook[1, "Ask"], Is.EqualTo(100));
    }
    
    [Test]
    public void TestOrderBookBidColumnProcessAt() {
        var column = new OrderBookBidColumn();
        orderBook.AddColumn(column);
        column.ProcessRealTimeAt(1, new BidInfo(){Price = 100, Size = 100}, orderBook);
        Assert.That(orderBook[1, "Bid"], Is.EqualTo(100));
    }
    
    [Test]
    public void TestOrderBookVolumeColumnProcessAt() {
        var column = new OrderBookVolumeColumn();
        orderBook.AddColumn(column);
        column.ProcessRealTimeAt(1, new TradeInfo(){Price = 100, Size = 100}, orderBook);
        Assert.That(orderBook[1, "Volume"], Is.EqualTo(100));
    }
    
    [Test]
    public void TestOrderBookBuyVolumeColumnProcessAt() {
        var column = new OrderBookBuyVolumeColumn();
        orderBook.AddColumn(column);
        column.ProcessRealTimeAt(1, new TradeInfo(){Price = 100, Size = 100, AggressorSide = "B"}, orderBook);
        column.ProcessRealTimeAt(1, new TradeInfo(){Price = 100, Size = 100, AggressorSide = "S"}, orderBook);
        Assert.That(orderBook[1, "BuyVolume"], Is.EqualTo(100));
    }
    
    [Test]
    public void TestOrderBookSellVolumeColumnProcessAt() {
        var column = new OrderBookSellVolumeColumn();
        orderBook.AddColumn(column);
        column.ProcessRealTimeAt(1, new TradeInfo(){Price = 100, Size = 100, AggressorSide = "B"}, orderBook);
        column.ProcessRealTimeAt(1, new TradeInfo(){Price = 100, Size = 100, AggressorSide = "S"}, orderBook);
        Assert.That(orderBook[1, "SellVolume"], Is.EqualTo(100));
    }
    
    [Test]
    public void TestOrderBookVolumeDeltaColumnProcessAt() {
        var column = new OrderBookVolumeDeltaColumn();
        orderBook.AddColumn(column);
        column.ProcessRealTimeAt(1, new TradeInfo(){Price = 100, Size = 300, AggressorSide = "B"}, orderBook);
        column.ProcessRealTimeAt(1, new TradeInfo(){Price = 100, Size = 100, AggressorSide = "S"}, orderBook);
        Assert.That(orderBook[1, "VolumeDelta"], Is.EqualTo(200));
        column.ProcessRealTimeAt(2, new TradeInfo(){Price = 100, Size = 100, AggressorSide = "B"}, orderBook);
        column.ProcessRealTimeAt(2, new TradeInfo(){Price = 100, Size = 300, AggressorSide = "S"}, orderBook);
        Assert.That(orderBook[2, "VolumeDelta"], Is.EqualTo(-200));
    }
}