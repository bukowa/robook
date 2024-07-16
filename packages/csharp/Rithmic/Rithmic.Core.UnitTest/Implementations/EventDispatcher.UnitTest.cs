using NUnit.Framework;

namespace Rithmic.Core.UnitTest.Implementations;

[TestFixture]
[TestOf(typeof(EventDispatcher<rapi.AskInfo>))]
public class EventDispatcherUnitTest {
    
    [Test]
    public async Task TestRegisterUnregisterHandler() {
        var dsp = new EventDispatcher<rapi.AskInfo>();
        var ctx = new Context();
        var ask = new rapi.AskInfo() { Size = 25 };
        var tcs = new TaskCompletionSource();
        dsp.RegisterHandler(ctx, (context, info) => {
            Assert.That(context, Is.EqualTo(ctx));
            Assert.That(info, Is.EqualTo(ask));
            tcs.SetResult();
        });
        dsp.Dispatch(ctx, ask);
        await tcs.Task;
        
        tcs = new TaskCompletionSource();
        dsp.UnregisterHandler(ctx);
        dsp.Dispatch(ctx, ask);
            
        var tcs2 = new TaskCompletionSource();
        var ctx2 = new Context();
        dsp.RegisterHandler(ctx2, (context, info) => {
            Assert.That(ctx2, Is.EqualTo(context));
            Assert.That(ask, Is.EqualTo(info));
            tcs2.SetResult();
        });
        dsp.Dispatch(ctx2, ask);
        await tcs2.Task;
        Assert.That(tcs.Task.IsCompleted, Is.False);
    }
}