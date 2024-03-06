using System.Collections.Concurrent;
using com.omnesys.rapi;
using Robook.OrderBookNS;

namespace Robook.UnitTest.OrderBookNS;

[TestFixture]
public class OrderBookSimulatorTest {
    OrderBook               ob;
    OrderBookProcessor      obp;
    OrderBookSimulator      obs;
    ConcurrentQueue<object> cq;

    private int     _obLevels   = 5;
    private decimal _obTickSize = 1m;
    private decimal _obMidPrice = 10m;

    [SetUp]
    public void Setup() {
        ob  = new OrderBook(_obTickSize, _obMidPrice, _obLevels);
        cq  = new ConcurrentQueue<object>();
        obp = new OrderBookProcessor(ob, cq);
        obs = new OrderBookSimulator(obp);

        ob.AddColumn(new OrderBookBidColumn("Bid"));
        ob.AddColumn(new OrderBookAskColumn("Ask"));
    }


    [Test]
    public void TestProcessOrder() {
        // Setup Bid and Ask offers, with offer size = 1 on all price levels
        obs.SetupBidAskOffers("Bid", "Ask", 1, 1);

        var initialPrice     = obs.GetCurrentPrice();
        var initialPriceBids = obs.GetOrderBookOffers("Bid", initialPrice);
        var initialPriceAsks = obs.GetOrderBookOffers("Ask", initialPrice);

        // 1st order: buy, size 1 current price should go up
        obs.ProcessBidAsk(true, 1);
        var firstOrderPrice = obs.GetCurrentPrice();
        Assert.That(firstOrderPrice, Is.Not.EqualTo(initialPrice));
    }

    [Test]
    public void TestGetOrderBookOffers() {
        var bidOffers = 10;
        var askOffers = 5;
        obs.SetupBidAskOffers("Bid", "Ask", bidOffers, askOffers);

        // By index
        var bidsResult1 = obs.GetOrderBookOffers("Bid", ob.OBDT.Rows.Count - 1);
        var asksResult1 = obs.GetOrderBookOffers("Ask", 0);

        Assert.That(bidsResult1, Is.EqualTo(bidOffers));
        Assert.That(asksResult1, Is.EqualTo(askOffers));

        // By price value
        var bidsResult2 = obs.GetOrderBookOffers("Bid", _obMidPrice - _obTickSize);
        var asksResult2 = obs.GetOrderBookOffers("Ask", _obMidPrice + _obTickSize);

        Assert.That(bidsResult2, Is.EqualTo(bidOffers));
        Assert.That(asksResult2, Is.EqualTo(askOffers));
    }

    [Test]
    public void TestGetOrderBookOffers_DBNullReturnsZero() {
        // Data Table is empty so all results should return zero
        Assert.That(obs.GetOrderBookOffers("Bid", 0),           Is.EqualTo(0D));
        Assert.That(obs.GetOrderBookOffers("Ask", 0),           Is.EqualTo(0D));
        Assert.That(obs.GetOrderBookOffers("Bid", _obMidPrice), Is.EqualTo(0D));
        Assert.That(obs.GetOrderBookOffers("Ask", _obMidPrice), Is.EqualTo(0D));
    }
}

[TestFixture]
public class OrderBookSimulatorTestStaticMethods {
    [Test]
    public void TestGetUpdatedPricePosition() {
        int   arrLength = 10;
        int   maxIndex  = arrLength - 1;
        int[] array     = new int[arrLength];

        // Various scenarios testing the behavior of GetUpdatedPricePosition method:
        //
        // - When currentPriceIndex is in the middle, and priceDirectionUp is true, the index should go down by one.
        Assert.That(OrderBookSimulator.GetUpdatedPricePosition(5, array, true), Is.EqualTo(4));

        // - When currentPriceIndex is in the middle, and priceDirectionUp is false, the index should go up by one.
        Assert.That(OrderBookSimulator.GetUpdatedPricePosition(5, array, false), Is.EqualTo(6));

        // - When currentPriceIndex is at index one, and priceDirectionUp is true,
        //   the index should go down, reaching the upper bound of the array.
        Assert.That(OrderBookSimulator.GetUpdatedPricePosition(1, array, true), Is.EqualTo(0));

        // - When currentPriceIndex is at the penultimate position, and priceDirectionUp is false,
        //   the index should go up, reaching the lower bound of the array.
        Assert.That(OrderBookSimulator.GetUpdatedPricePosition(maxIndex - 1, array, false), Is.EqualTo(maxIndex));

        // Scenarios where the updated index is out of bounds:
        //
        // - When currentPriceIndex is at the upper bound, and priceDirectionUp is true,
        //   the index should remain at the upper bound.
        Assert.That(OrderBookSimulator.GetUpdatedPricePosition(0, array, true), Is.EqualTo(0));

        // - When currentPriceIndex is negative, the index should remain at the upper bound.
        Assert.That(OrderBookSimulator.GetUpdatedPricePosition(-100, array, false), Is.EqualTo(0));

        // - When currentPriceIndex is at the lower bound, and priceDirectionUp is false,
        //   the index should remain at the lower bound.
        Assert.That(OrderBookSimulator.GetUpdatedPricePosition(maxIndex, array, false), Is.EqualTo(maxIndex));

        // - When currentPriceIndex is larger than the array size, the index should remain at the lower bound.
        Assert.That(OrderBookSimulator.GetUpdatedPricePosition(100, array, false), Is.EqualTo(maxIndex));
    }

    [Test]
    public void TestGetMiddleIndexOfArray_OddLength() {
        int[] array = new int[7];
        Assert.That(OrderBookSimulator.GetMiddleIndexOfArray(array), Is.EqualTo(3));
    }

    [Test]
    public void TestGetMiddleIndexOfArray_EvenLength() {
        int[] array = new int[10];
        Assert.That(OrderBookSimulator.GetMiddleIndexOfArray(array), Is.EqualTo(5));
    }

    [Test]
    public void TestGetMiddleIndexOfArray_EmptyArrayThrowsException() {
        int[] array = new int[0];
        Assert.Throws<InvalidOperationException>(() => OrderBookSimulator.GetMiddleIndexOfArray(array));
    }
}