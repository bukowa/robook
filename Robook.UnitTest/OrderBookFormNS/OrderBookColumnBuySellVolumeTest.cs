using com.omnesys.rapi;
using Robook.OrderBookColumns;
using Robook.OrderBookNS;

namespace Robook.UnitTest.OrderBookFormNS;

[TestFixture]
public class OrderBookColumnBuySellVolumeTest {
    OrderBook ob;

    [SetUp]
    public void SetUp() {
        ob = new OrderBook(10, 100, 10);
    }

    [Test]
    public void ShouldCalculateMaxValueBasedOnTradeVolume() {
        var orderBookColumn = new OrderBookVolumeColumn();
        var buyVolumeColumn = new BuyVolumeColumn() {
            Name                        = "BuyVolume",
            DataPropertyName            = "BuyVolume",
            CalculateBasedOnTradeVolume = true
        };

        ob.AddColumn(orderBookColumn);

        orderBookColumn.ProcessRealTimeAt(ob.GetIndexOfPrice(100d), new TradeInfo() { Price = 100d, Size = 50 }, ob);
        orderBookColumn.ProcessRealTimeAt(ob.GetIndexOfPrice(100d), new TradeInfo() { Price = 100d, Size = 50 }, ob);
        orderBookColumn.ProcessRealTimeAt(ob.GetIndexOfPrice(120d), new TradeInfo() { Price = 120d, Size = 20 }, ob);
        orderBookColumn.ProcessRealTimeAt(ob.GetIndexOfPrice(130d), new TradeInfo() { Price = 130d, Size = 10 }, ob);

        buyVolumeColumn.RecalculateMaxValueProperty(ob);

        Assert.That(buyVolumeColumn.MaxValue, Is.EqualTo(100));
    }

    [Test]
    public void CalculateMaxValueBasedOnTradeVolume_ShouldThrowErrorIfOrderBookColumnDoesNotExist() {
        var buyVolumeColumn = new BuyVolumeColumn() {
            Name                        = "BuyVolume",
            DataPropertyName            = "BuyVolume",
            CalculateBasedOnTradeVolume = true
        };

        var exc = Assert.Throws<InvalidOperationException>(() => buyVolumeColumn.RecalculateMaxValueProperty(ob));
        Assert.That(
            exc.Message, Does.Contain(
                $"The OrderBook does not contain the specified {new OrderBookVolumeColumn().Name} column required for calculations."
            ));
    }
}