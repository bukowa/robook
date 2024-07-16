using NUnit.Framework;

namespace Rithmic.Core.UnitTest.Implementations;

[TestFixture]
[TestOf(typeof(RCallbacks))]
public class RCallbacksUnitTest {
        
        [Test]
        public async Task TestAlert() {
            var rclb = new RCallbacks();
            var tsc  = new TaskCompletionSource<bool>();
            rclb.AlertDispatcher.RegisterHandler(new Context(), (context, info) => {
                Assert.That(context, Is.Not.Null);
                tsc.SetResult(true);
            });
            rclb.Alert(new rapi.AlertInfo());
            Assert.That(await tsc.Task, Is.True);
        }
        
}