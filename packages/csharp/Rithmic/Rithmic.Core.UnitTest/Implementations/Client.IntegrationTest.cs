using NUnit.Framework;

namespace Rithmic.Core.UnitTest.Implementations;

[TestFixture]
[TestOf(typeof(RithmicClient<RCallbacks, AdmCallbacks>))]
public class ClientIntegrationTest {
    [Test]
    public async Task TestLoginLogoutAsync() {
        var testCnt = new TestServerConnection();
        var client  = new RithmicClient<RCallbacks, AdmCallbacks>();
        var cParams = testCnt.GetCParams();
        var lParams = new LParams(
            plugInMode: false,
            pnlConnection: new Connection(testCnt.Login,            testCnt.Password, rapi.ConnectionId.PnL),
            marketDataConnection: new Connection(testCnt.Login,     testCnt.Password, rapi.ConnectionId.MarketData),
            historicalDataConnection: new Connection(testCnt.Login, testCnt.Password, rapi.ConnectionId.History),
            tradingSystemConnection: new Connection(testCnt.Login,  testCnt.Password, rapi.ConnectionId.TradingSystem)
        );
        var authParams = new RithmicAuth(lParams, cParams);
        await client.LoginAsync(authParams);
        Assert.That(lParams.ConnectionsEnumerable.All(c => c.IsLoggedIn));
        await client.LogoutAsync();
        Assert.That(lParams.ConnectionsEnumerable.All(c => !c.IsLoggedIn));
    }
}