using System.Collections.Concurrent;
using com.omnesys.rapi;
using Robook.OrderBookNS;

namespace Robook.UnitTest.OrderBookNS;

[TestFixture]
[TestOf(typeof(OrderBookProcessor))]
public class OrderBookProcessorTest {
    [Test]
    [Description("Class can be constructed.")]
    public void TestNew() {
        var orb = new OrderBook(0.25m, 100m, 10);
        var cqu = new ConcurrentQueue<object>();
        var obp = new OrderBookProcessor(orb, cqu);
    }

    [Test]
    [Description("Columns are processed with queue.")]
    public void TestCanProcessAllColumnsWithQueue() {
        var orb = new OrderBook(0.25m, 100m, 10);
        var cqu = new ConcurrentQueue<object>();
        var obp = new OrderBookProcessor(orb, cqu);

        orb.AddColumn(new OrderBookAskColumn());
        orb.AddColumn(new OrderBookBidColumn());
        orb.AddColumn(new OrderBookVolumeColumn());
        orb.AddColumn(new OrderBookBuyVolumeColumn());
        orb.AddColumn(new OrderBookSellVolumeColumn());

        cqu.Enqueue(new AskInfo { Price   = 100, Size = 1 });
        cqu.Enqueue(new BidInfo { Price   = 100, Size = 1 });
        cqu.Enqueue(new TradeInfo { Price = 100, Size = 1 });
        cqu.Enqueue(new TradeInfo { Price = 100, Size = 1, AggressorSide = "B" });
        cqu.Enqueue(new TradeInfo { Price = 100, Size = 1, AggressorSide = "S" });

        obp.Start();
        Thread.Sleep(100);
        var invoked =
            orb.OBCC.ColAskInfo.Sum(col => col.ProcessedCount)
            + orb.OBCC.ColBidInfo.Sum(col => col.ProcessedCount)
            + orb.OBCC.ColTradeInfo.Sum(col => col.ProcessedCount);

        Assert.That(invoked, Is.EqualTo(7));
        obp.Stop();
    }

    [Test]
    public void TestAddAndRemoveColumns() {
        var orb = new OrderBook(0.25m, 100m, 10);
        var cqu = new ConcurrentQueue<object>();
        var obp = new OrderBookProcessor(orb, cqu);
        
        orb.AddColumn(new OrderBookAskColumn());
        orb.AddColumn(new OrderBookBidColumn());
        
        // spawn a thread that will enqueue randomly
        // bid or ask info objects until canceled
        var ct = new CancellationTokenSource();
        Task.Run(() => {
            while (!ct.IsCancellationRequested) {
                var rnd = new Random();
                var obj = rnd.Next(0, 2) == 0
                    ? (object) new AskInfo { Price = 100, Size = 1 }
                    : new BidInfo { Price = 100, Size = 1 };
                cqu.Enqueue(obj);
            }
        });
        
        // spawn a thread that will dequeue the objects
        obp.Start(0);
        Thread.Sleep(100);
        
        var ask = new OrderBookAskColumn();
        var bid = new OrderBookBidColumn();
        obp.AddColumn(ask);
        obp.AddColumn(bid);
        // add and remove columns
        for (var i = 0; i < 100000; i++) {
            obp.RemoveColumn(ask);
            obp.AddColumn(ask);
            obp.RemoveColumn(bid);
            obp.AddColumn(bid);
        }
        ct.Cancel();
        
        while (cqu.Count != 0) {
            Thread.Sleep(100);
        }
    }
}