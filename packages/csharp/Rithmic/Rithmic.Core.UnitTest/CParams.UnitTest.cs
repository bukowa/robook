using NUnit.Framework;

namespace Rithmic.Core.UnitTest;

[TestFixture, Order(0)]
[TestOf(typeof(CParamsSource))]
public class CParamsSourceUnitTest {

    [Test]
    public void TestCParamsDict() {
        var source = new CParamsSource(TestConfig.RApiConfigPath);
        Assert.That(source.CParamsDict.Count,                            Is.GreaterThan(10));
        Assert.That(source.CParamsDict.Select(v => v.Value.Count).Sum(), Is.GreaterThan(100));

        var cParams = source.CParamsDict["Rithmic Paper Trading"]["Europe"];
        Assert.That(cParams.SystemName,  Is.EqualTo("Rithmic Paper Trading"));
        Assert.That(cParams.GatewayName, Is.EqualTo("Europe"));
        Assert.Multiple(() => {
            Assert.That(cParams.AdmCnnctPt,  !Is.EqualTo(""));
            Assert.That(cParams.SCnnctPt,    !Is.EqualTo(""));
            Assert.That(cParams.SMdCnnctPt,  !Is.EqualTo(""));
            Assert.That(cParams.SIhCnnctPt,  !Is.EqualTo(""));
            Assert.That(cParams.STsCnnctPt,  !Is.EqualTo(""));
            Assert.That(cParams.SPnLCnnctPt, !Is.EqualTo(""));
            Assert.That(cParams.DomainName,  !Is.EqualTo(""));
            Assert.That(cParams.LoggerAddr,  !Is.EqualTo(""));
            Assert.That(cParams.DmnSrvrAddr, !Is.EqualTo(""));
            Assert.That(cParams.LicSrvrAddr, !Is.EqualTo(""));
            Assert.That(cParams.LocBrokAddr, !Is.EqualTo(""));
        });
    }

    [Test]
    public void TestGetCParams() {
        var source = new CParamsSource(TestConfig.RApiConfigPath);
        var cParams = source.GetCParams("Rithmic Paper Trading", "Europe");
        Assert.That(cParams, Is.Not.Null);
        Assert.That(cParams.SystemName,  Is.EqualTo("Rithmic Paper Trading"));
        Assert.That(cParams.GatewayName, Is.EqualTo("Europe"));
    }
}