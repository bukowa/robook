using com.omnesys.rapi;
using Rithmic;
using Robook.SymbolNS;

namespace Robook.UnitTest.SymbolNS;

[TestFixture]
[TestOf(typeof(Symbol))]
public class SymbolTest {

    private static Client client;
    
    [SetUpFixture]
    public class Setup {
        [OneTimeSetUp]
        public void OneTimeSetUp() {
            client = new TestConnection().NewReadyClient();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown() {
            client.Engine.shutdown();
        }
    }

    [Test]
    [Category(Categories.RithmicConnectionTestServer)]
    public async Task TestGetPriceIncrInfo() {
        var s = new Symbol("CME", "ES");
        s.SetClient(client);
        var pii = await s.GetPriceIncrInfoAsync();
        Assert.That(pii, Is.Not.Null);
        Assert.That(pii.Rows[0].PriceIncr, Is.EqualTo(0.25));
    }
    
    [Test]
    [Category(Categories.RithmicConnectionTestServer)]
    public async Task TestGetRefData() {
        var s = new Symbol("CME", "ES");
        s.SetClient(client);
        var rd = await s.GetRefDataAsync();
        Assert.That(rd, Is.Not.Null);
        Assert.That(rd.RpCode, Is.EqualTo(0));
    }

    [Test]
    [Category(Categories.RithmicConnectionTestServer)]
    public async Task TestRebuildBook() {
        var s = new Symbol("CME", "ES");
        s.SetClient(client);
        var rb = await s.RebuildBookAsync();
        Assert.That(rb, Is.Not.Null);
        Assert.That(rb.RpCode, Is.EqualTo(0));
    }
    
    [Test]
    [Category(Categories.RithmicConnectionTestServer)]
    public async Task TestGetInformation() {
        var s = new Symbol("CME", "ES");
        s.SetClient(client);
        await s.GetInformationAsync();
        Assert.That(s.TickSize,   Is.EqualTo(0.25));
        Assert.That(s.TickValue,  Is.EqualTo(12.5));
        Assert.That(s.PointValue, Is.EqualTo(50));
    }

    [Test]
    [Category(Categories.RithmicConnectionTestServer)]
    public async Task TestSubscribeToUpdates() {
        var paperClient = new TestConnection().NewPaperReadyClient();
        var s           = new Symbol("CME", "ES");
        s.SetClient(paperClient);
        s.SubscribeToUpdates();
        await Task.Delay(10000);
        paperClient.Engine.shutdown();
    }
}