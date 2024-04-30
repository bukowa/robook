using System.ComponentModel;
using System.Dynamic;
using Rithmic;
using Robook.Accounts;
using Robook.Data;

namespace Robook.State;

public static class Config {
    public static string StoreDirectory = @"C:\Users\buk\Documents\Robook";
}

public static class Storage {
    public static LocalStorage<Symbol> LocalSymbolsStorage = new (Config.StoreDirectory + "\\symbols.json");
}

public class State {
    public AccountsStore                AccountsStore;
    public CParamsSource                CParamsSource;
    public void Init() {
        AccountsStore = new AccountsStore(@"C:\Users\buk\Documents\Robook\accounts.json");
        CParamsSource = new CParamsSource(@"C:\Users\buk\Documents\RApiConfig");
        AccountsStore.Read();
    }
}