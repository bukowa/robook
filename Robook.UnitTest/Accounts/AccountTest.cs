using com.omnesys.rapi;
using Rithmic;
using Robook.Accounts;
using Robook.Store;

namespace Robook.UnitTest.Accounts;

[TestFixture]
[TestOf(typeof(Account))]
public class AccountTest {
    [Test]
    public void TestClientNotify() {
        var account = new Account();
        var client  = new Client();
        var @params = new LoginParams(new CParams());
        @params.HistoricalDataConnection = new Connection("login", "pass", ConnectionId.History);
        @params.MarketDataConnection     = new Connection("login", "pass", ConnectionId.MarketData);
        // set the client
        account.Client = client;
        // set the params on the client
        client.Params  = @params;
        Assert.That(account.Client, Is.EqualTo(client));
        // modify client properties
        var x = 0;
        account.PropertyChanged                              += (sender, args) => { x += 1; };
        client.Params.HistoricalDataConnection.LastConnectionAlert =  new ConnectionAlert(){AlertInfo = new AlertInfo() { Message = "test" }};
        Assert.That(x, Is.EqualTo(1));
    }
}