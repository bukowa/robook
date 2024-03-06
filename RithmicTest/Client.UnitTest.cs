using com.omnesys.rapi;

namespace RithmicTest;

[TestFixture, Order(1)]
public class ClientUnitTest {

    [Category(Categories.RConnectionTest)]
    [TestCase(new[] { ConnectionId.History },                                ExpectedResult = true)]
    [TestCase(new[] { ConnectionId.MarketData },                             ExpectedResult = true)]
    [TestCase(new[] { ConnectionId.TradingSystem },                          ExpectedResult = true)]
    [TestCase(new[] { ConnectionId.History, ConnectionId.MarketData },       ExpectedResult = true)]
    [TestCase(new[] { ConnectionId.History, ConnectionId.TradingSystem },    ExpectedResult = true)]
    [TestCase(new[] { ConnectionId.MarketData, ConnectionId.TradingSystem }, ExpectedResult = true)]
    [TestCase(new[] { ConnectionId.History, ConnectionId.MarketData, ConnectionId.TradingSystem },
              ExpectedResult = true)]
    [TestCase(new[] { ConnectionId.History, ConnectionId.MarketData, ConnectionId.TradingSystem, ConnectionId.PnL },
              ExpectedResult = true)]
    public bool TestLoginWaiting(ConnectionId[] connectionIds) {
        var cn = new TestServerConnection();
        var cp = new LoginParams(cn.GetCParams());

        cp.PlugInMode   = false;

        if (connectionIds.Contains(ConnectionId.MarketData))
            cp.MarketDataConnection =
                new Connection(cn.Login, cn.Password, ConnectionId.MarketData);
        if (connectionIds.Contains(ConnectionId.History))
            cp.HistoricalDataConnection =
                new Connection(cn.Login, cn.Password, ConnectionId.History);
        if (connectionIds.Contains(ConnectionId.TradingSystem))
            cp.TradingSystemConnection =
                new Connection(cn.Login, cn.Password, ConnectionId.TradingSystem);
        if (connectionIds.Contains(ConnectionId.PnL))
            cp.PnlConnection =
                new Connection(cn.Login, cn.Password, ConnectionId.PnL);

        var client = new Client();
        var r      = client.LoginAndWait(cp);

        if (connectionIds.Contains(ConnectionId.MarketData))
            Assert.That(client.MarketDataConnection.IsLoggedIn, Is.True);

        if (connectionIds.Contains(ConnectionId.History))
            Assert.That(client.HistoricalDataConnection.IsLoggedIn, Is.True);

        if (connectionIds.Contains(ConnectionId.TradingSystem))
            Assert.That(client.TradingSystemConnection.IsLoggedIn, Is.True);

        if (connectionIds.Contains(ConnectionId.PnL))
            Assert.That(client.PnlConnection.IsLoggedIn, Is.True);

        client.Engine.shutdown();
        return r;
    }

    [Test]
    [TestCase(new[] { ConnectionId.PnL })]
    public void TestLoginThrowsException(ConnectionId[] connectionIds) {
        Assert.Throws<Exception>(() => {
            TestLoginWaiting(connectionIds);
        });
    }

    [Test]
    [Category(Categories.RConnectionTest)]
    public async Task Test() {
        var cn = new TestServerConnection();
        var cp = new LoginParams(cn.GetCParams());
        
        cp.PlugInMode   = false;
        cp.PnlConnection =
            new Connection(cn.Login, cn.Password, ConnectionId.PnL);
        cp.TradingSystemConnection =
            new Connection(cn.Login, cn.Password, ConnectionId.TradingSystem);
        cp.HistoricalDataConnection =
            new Connection(cn.Login, cn.Password, ConnectionId.History);
        cp.MarketDataConnection =
            new Connection(cn.Login, cn.Password, ConnectionId.MarketData);
        
        var client = new Client();
        client.LoginAndWait(cp);
        
        Assert.That(client.MarketDataConnection.IsLoggedIn,     Is.True);
        Assert.That(client.HistoricalDataConnection.IsLoggedIn, Is.True);
        Assert.That(client.TradingSystemConnection.IsLoggedIn,  Is.True);
        Assert.That(client.PnlConnection.IsLoggedIn,            Is.True);

        var start = new DateTime(2023, 10, 10);
        var stop  = start.AddDays(100);
        var unix  = new DateTime(1970, 1, 1).ToUniversalTime();

        // replay some NQ ticks
        var ctx       = new Context();
        var tcs       = new TaskCompletionSource<bool>();
        var barTimers = new List<int>();
        client.RHandler.BarReplayClb.Subscribe(ctx, (bar, symbol) => {
            Console.WriteLine($"[{symbol}] {bar}");
            tcs.SetResult(true);
        });
        client.RHandler.BarClb.Subscribe(ctx, (bar, symbol) => {
            Console.WriteLine($"[{symbol}] {bar}");
            barTimers.Add(symbol.EndSsboe);
        });
        client.Engine.replayBars(new ReplayBarParams() {
            Exchange       = "CME",
            Symbol         = "NQH4",
            Type           = BarType.Tick,
            SpecifiedTicks = 1,
            StartSsboe     = (int)start.Subtract(unix).TotalSeconds,
            EndSsboe       = (int)stop.Subtract(unix).TotalSeconds,
            Context        = ctx,
            StartUsecs     = 0,
            EndUsecs       = 0,
        });
        await tcs.Task;
        Assert.That(barTimers.Count, Is.GreaterThan(0));
        client.Engine.shutdown();
    }
}