using com.omnesys.rapi;
using Robook.Accounts;
using Robook.Store;

namespace Robook.UnitTest.Accounts;

[TestFixture]
[TestOf(typeof(AccountsStore))]
public class AccountsStoreTest {
    [Test]
    public void Test() {
        var path = Guid.NewGuid().ToString() + ".json";
        if (File.Exists(path)) File.Delete(path);
        var store = new AccountsStore(path);
        store.Read();
        store.Accounts.Add(new Account() {
            Login         = "login",
            Password      = new ProtectedString("password"),
            Server        = "server",
            Gateway       = "gateway",
            HistoricalDataConnection = true,
            TradingSystemConnection = true
        });
        store.Write();
        store = new AccountsStore(path);
        store.Read();
        Assert.Multiple(() => {
            Assert.That(store.Accounts.Count, Is.EqualTo(1));
            Assert.That(store.Accounts[0].Login, Is.EqualTo("login"));
            Assert.That(store.Accounts[0].Password.Decrypted(), Is.EqualTo("password"));
            Assert.That(store.Accounts[0].Server, Is.EqualTo("server"));
            Assert.That(store.Accounts[0].Gateway, Is.EqualTo("gateway"));
            Assert.That(store.Accounts[0].HistoricalDataConnection, Is.True);
            Assert.That(store.Accounts[0].TradingSystemConnection, Is.True);
            
        });
    }
}