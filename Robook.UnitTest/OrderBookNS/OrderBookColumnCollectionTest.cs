using Robook.OrderBookNS;

namespace Robook.UnitTest.OrderBookUnitTest;

[TestFixture]
[TestOf(typeof(OrderBookColumnCollection))]
public class OrderBookColumnCollectionTest {
    [Test]
    public void TestNew() {
        var obcc = new OrderBookColumnCollection();
    }
}