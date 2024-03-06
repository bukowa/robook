using Rithmic;
using Robook.Accounts;

namespace Robook.State;

public class State {
    public AccountsStore AccountsStore;
    public CParamsSource CParamsSource;
    public void Init() {
        AccountsStore = new AccountsStore(@"C:\Users\buk\Documents\Robook\accounts.json");
        CParamsSource = new CParamsSource(@"C:\Users\buk\Documents\RApiConfig");
        AccountsStore.Read();
    }
}