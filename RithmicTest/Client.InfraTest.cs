using com.omnesys.omne.om;
using com.omnesys.rapi;

namespace RithmicTest;

/// <summary>
/// Tests related to checking how the client infrastructure behaves - 
/// that is not part of this project's business logic but of the underlying
/// Rithmic client infrastructure itself.
/// </summary>
public class ClientInfraTest {
    [Test]
    [Category(Categories.RConnectionTest)]
    [Description("Tested: Engine cannot subscribe before login.")]
    public void TestThatEngineCannotSubscribeBeforeClientLogin() {
        var tc           = new TestServerConnection();
        var clientParams = tc.GetClientParams();
        var client       = tc.GetClient();
        Assert.That(client.LoginAndWait(clientParams), Is.True);
        client.Logout();
        Assert.Throws<OMException>(() => {
            client.Engine.subscribe(tc.Exchange, tc.Symbol, SubscriptionFlags.All, null);
        });
        client.Engine.shutdown();
    }
}