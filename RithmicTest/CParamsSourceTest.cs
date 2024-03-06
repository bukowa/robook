namespace RithmicTest;

[TestFixture, Order(0)]
[TestOf(typeof(CParamsSource))]
public class CParamsSourceTest {
    public static string ConnectionFilesPath = Config.RApiConfigPath;

    [Test]
    [Category("CParamsTest")]
    public void TestCParamsSource() {
        var source = new CParamsSource(ConnectionFilesPath);
        Assert.That(source.CParamsBySystemName.Count,                            Is.GreaterThan(10));
        Assert.That(source.CParamsBySystemName.Select(v => v.Value.Count).Sum(), Is.GreaterThan(100));

        var cParams = source.CParamsBySystemName["Rithmic Paper Trading"]["Europe"];
        Assert.That(cParams.SystemName,  Is.EqualTo("Rithmic Paper Trading"));
        Assert.That(cParams.GatewayName, Is.EqualTo("Europe"));
        Assert.Multiple(() => {
            Assert.That(cParams.AdmCnnctPt,  !Is.EqualTo(""));
            Assert.That(cParams.sCnnctPt,    !Is.EqualTo(""));
            Assert.That(cParams.sMdCnnctPt,  !Is.EqualTo(""));
            Assert.That(cParams.sIhCnnctPt,  !Is.EqualTo(""));
            Assert.That(cParams.sTsCnnctPt,  !Is.EqualTo(""));
            Assert.That(cParams.sPnLCnnctPt, !Is.EqualTo(""));
            Assert.That(cParams.DomainName,  !Is.EqualTo(""));
            Assert.That(cParams.LoggerAddr,  !Is.EqualTo(""));
            Assert.That(cParams.DmnSrvrAddr, !Is.EqualTo(""));
            Assert.That(cParams.LicSrvrAddr, !Is.EqualTo(""));
            Assert.That(cParams.LocBrokAddr, !Is.EqualTo(""));
        });
    }

    [Test]
    public void GetCParams() {
        var source = new CParamsSource(ConnectionFilesPath);
        var cParams = source.GetCParams("Rithmic Paper Trading", "Europe");
        Assert.That(cParams, Is.Not.Null);
        Assert.That(cParams.SystemName,  Is.EqualTo("Rithmic Paper Trading"));
        Assert.That(cParams.GatewayName, Is.EqualTo("Europe"));
    }
}