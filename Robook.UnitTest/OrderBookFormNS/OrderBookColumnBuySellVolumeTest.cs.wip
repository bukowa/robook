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
        var orderBookColumn = new OrderBookDefaultColumn("Volume", new []{OrderBookColumnDataType.Trade}, typeof(int));
        var buyVolumeColumn = new BuyVolumeColumn() {
            Name                        = "Volume",
            DataPropertyName            = "Volume",
            CalculateBasedOnTradeVolume = true
        };

        ob.AddColumn(orderBookColumn);

        orderBookColumn.ProcessTrade(ob.GetIndexOfPrice(100d), new TradeInfo() { Price = 100d, Size = 50 }, ob);
        orderBookColumn.ProcessTrade(ob.GetIndexOfPrice(100d), new TradeInfo() { Price = 100d, Size = 50 }, ob);
        orderBookColumn.ProcessTrade(ob.GetIndexOfPrice(120d), new TradeInfo() { Price = 120d, Size = 20 }, ob);
        orderBookColumn.ProcessTrade(ob.GetIndexOfPrice(130d), new TradeInfo() { Price = 130d, Size = 10 }, ob);

        buyVolumeColumn.RecalculateMaxValueProperty(ob.DataTable);

        Assert.That(buyVolumeColumn.MaxValue, Is.EqualTo(100));
    }

    [Test]
    public void CalculateMaxValueBasedOnTradeVolume_ShouldThrowErrorIfOrderBookColumnDoesNotExist() {
        var buyVolumeColumn = new BuyVolumeColumn() {
            Name                        = "BuyVolume",
            DataPropertyName            = "BuyVolume",
            CalculateBasedOnTradeVolume = true
        };

        var exc = Assert.Throws<InvalidOperationException>(() => buyVolumeColumn.RecalculateMaxValueProperty(ob.DataTable));
        Assert.That(
            exc.Message, Does.Contain(
                $"The OrderBook does not contain the specified {new OrderBookDefaultColumn("1", new [] { OrderBookColumnDataType.Trade }, typeof(int)).Name} column required for calculations."
            ));
    }
}