using NUnit.Framework;

namespace Rithmic.Core.UnitTest.Implementations;

[TestFixture]
[TestOf(typeof(Connection))]
public class ConnectionUnitTest {
    [Test]
    public async Task TestRegisterRCallbackHandler() {
        var cnt  = new Connection("login", "password", rapi.ConnectionId.History);
        var rclb = new RCallbacks();
        var ctx  = new Context();
        cnt.RegisterRCallbackAlertHandler(ctx, rclb);

        var tcs = new TaskCompletionSource<IConnectionAlert>();
        cnt.RegisterHandler(rapi.AlertType.ConnectionOpened, (connection, alert) => {
            Assert.That(connection, Is.EqualTo(cnt));
            tcs.SetResult(alert);
        });
        rclb.Alert(new rapi.AlertInfo() {
            ConnectionId = rapi.ConnectionId.History,
            AlertType    = rapi.AlertType.ConnectionOpened
        });
        await tcs.Task;
    }
}