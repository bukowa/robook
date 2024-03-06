using System.ComponentModel;
using com.omnesys.rapi;
using Rithmic;

namespace Robook.Accounts;

public partial class AccountsForm : Form {
    private DataGridView  accountsDataGridView;
    private object        _lock;

    private DataGridView         DGV  => accountsDataGridView;
    private BindingList<Account> ACCS => AccountsStore.Accounts;

    /// <summary>
    /// A convenient method for adding a column with a OnClick event.
    /// </summary>
    public void AddOnClickColumn(DataGridViewColumn c, Action<DataGridViewCellEventArgs, int> e) {
        DGV.CellClick += (_, a) => {
            if (a.ColumnIndex >= 0 && a.RowIndex >= 0) {
                var col = DGV.Columns[a.ColumnIndex];
                if (col == c) {
                    e(a, a.RowIndex);
                    return;
                }
                return;
            }
        };
        DGV.Columns.Add(c);
    }

    /// <summary>
    /// A convenient method for adding a column with a OnClick event and a confirmation dialog.
    /// </summary>
    /// <param name="c"></param>
    /// <param name="e"></param>
    public void AddOnClickConfirmationColumn(DataGridViewColumn c, Action<DataGridViewCellEventArgs, int> e) {
        AddOnClickColumn(c, (a, i) => {
            if (MessageBox.Show("Are you sure?", "?", MessageBoxButtons.YesNo) == DialogResult.No) return;
            e(a, i);
        });
    }

    /// <summary>
    /// Adds a column responsible for deleting the row.
    /// </summary>
    public void AddColumns() {
        var c = new DataGridViewButtonColumn();
        c.Name = "Delete";
        AddOnClickConfirmationColumn(c, (args, i) => { ACCS.RemoveAt(i); });
        var c2 = new DataGridViewButtonColumn();
        c2.Name = "Login";
        AddOnClickColumn(c2, (args, i) => {
            var acc = ACCS[i];

            if (acc.Client == null) {
                acc.Client = new Client();
            }
            
            var cp = _state.CParamsSource.GetCParams(acc.Server, acc.Gateway);
            if (cp == null) {
                MessageBox.Show("No CParams found this gateway and server combination.");
                return;
            }
            var lp = new LoginParams(cp) {
                HistoricalDataConnection = acc.HistoricalDataConnection ? new Connection(acc.Login, acc.Password.Decrypted(), ConnectionId.History) : null,
                MarketDataConnection     = acc.MarketDataConnection     ? new Connection(acc.Login, acc.Password.Decrypted(), ConnectionId.MarketData)  : null,
                PnlConnection            = acc.PnlConnection            ? new Connection(acc.Login, acc.Password.Decrypted(), ConnectionId.PnL)     : null,
                TradingSystemConnection  = acc.TradingSystemConnection  ? new Connection(acc.Login, acc.Password.Decrypted(), ConnectionId.TradingSystem)   : null,
                PlugInMode = acc.PlugInMode,
            };
            
            Task.Run(() => {
                    try {
                        acc.Client.LoginAndWait(lp);
                    } catch (Exception e) {
                        MessageBox.Show(e.Message);
                    }
                });
        });
        var c3 = new DataGridViewButtonColumn();
        c3.Name = "Logout";
        AddOnClickColumn(c3, (args, i) => {
            var acc = ACCS[i];
            try {
                acc.Client?.Engine?.logout();
            } catch (Exception e) {
                MessageBox.Show(e.Message);
            }
        });
    }

    private State.State   _state;
    private AccountsStore AccountsStore => _state.AccountsStore;

    public AccountsForm(State.State state) {
        InitializeComponent();
        _state                             =  state;
        Width                              =  1200;
        AccountsStore.Accounts.ListChanged += (sender, args) => { AccountsStore.Write(); };
        accountsDataGridView               =  new DataGridView();
        accountsDataGridView.Dock          =  DockStyle.Fill;
        accountsDataGridView.DataSource    =  AccountsStore.Accounts;
        AddColumns();
        Controls.Add(accountsDataGridView);
    }
}