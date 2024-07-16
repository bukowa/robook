using NUnit.Framework;

namespace Rithmic.Core.UnitTest.Implementations;

[TestFixture]
[TestOf(typeof(AdmCallbacks))]
public class AdmCallbacksUnitTest {
    [Test]
    public async Task TestAlert() {
        var aclb = new AdmCallbacks();
        var tsc  = new TaskCompletionSource<bool>();
        aclb.AlertDispatcher.RegisterHandler(new Context(), (_, _) => { tsc.SetResult(true); });
        aclb.Alert(new rapi.AlertInfo());
        Assert.That(await tsc.Task, Is.True);
    }
}