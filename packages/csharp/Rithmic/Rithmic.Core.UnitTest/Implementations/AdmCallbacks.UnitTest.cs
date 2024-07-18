using NUnit.Framework;

namespace Rithmic.Core.UnitTest.Implementations;

[TestFixture]
[TestOf(typeof(AdmCallbacksFacade))]
public class AdmCallbacksUnitTest {
    [Test]
    public async Task TestAlert() {
        var aclb = new AdmCallbacksFacade();
        var tsc  = new TaskCompletionSource<bool>();
        aclb.AlertDispatcher.RegisterHandler(new Context(), (_, _) => { tsc.SetResult(true); });
        aclb.Alert(new rapi.AlertInfo());
        Assert.That(await tsc.Task, Is.True);
    }
}