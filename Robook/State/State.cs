using System.ComponentModel;
using System.Dynamic;
using Rithmic;
using Robook.Accounts;
using Robook.Data;
using Connection = Robook.Data.Connection;

namespace Robook.State;

public static class Config {
    public static readonly string StoreDirectory
        = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Robook";
}

public static class Storage {
    public static readonly LocalStorage<Symbol> LocalSymbolsStorage =
        new(Config.StoreDirectory + "\\symbols.json");

    public static readonly LocalStorage<Subscription> LocalSubscriptionsStorage =
        new(Config.StoreDirectory + "\\subscriptions.json");

    public static readonly LocalStorage<Connection> LocalConnectionsStorage =
        new(Config.StoreDirectory + "\\connections.json");
}

public static class Data {
    public static readonly BindingList<Symbol>       Symbols;
    public static readonly BindingList<Connection>   Connections;
    public static readonly BindingList<Subscription> Subscriptions;

    static Data() {
        Symbols               = Storage.LocalSymbolsStorage.Load();
        Symbols.AllowEdit     = true;
        Connections           = Storage.LocalConnectionsStorage.Load();
        Connections.AllowEdit = true;
        Subscriptions         = Storage.LocalSubscriptionsStorage.Load();
        Subscriptions.ToList().ForEach(s => {
            s.Connection = Connections.FirstOrDefault(c => c.Id == s.ConnectionId);
            s.Symbol     = Symbols.FirstOrDefault(c => c.Id == s.SymbolId);
        });
        Subscriptions.AllowEdit = true;
    }
}

public class State {
    public AccountsStore AccountsStore;
    public CParamsSource CParamsSource;

    public void Init() {
        AccountsStore = new AccountsStore(Config.StoreDirectory + "\\accounts.json");
        CParamsSource = new CParamsSource(Config.StoreDirectory + "\\RApiConfig");
        AccountsStore.Read();
    }
}