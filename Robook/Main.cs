using System.Collections.ObjectModel;
using Robook.Accounts;
using Robook.Data;
using Robook.DataServiceFormNS;
using Robook.Forms;
using Robook.OrderBookFormNS;
using Robook.State;

namespace Robook;

public partial class Main : Form {
    private State.State _state;

    public Main() {
        InitializeComponent();
        _state = new State.State();
        _state.Init();

        accountsFormInit();
    }

    #region AccountsForm

    private AccountsForm? _accountsForm;

    private void accountsFormInit() {
        _accountsForm = new AccountsForm(_state);
        _accountsForm.FormClosing += (sender, args) => {
            args.Cancel = true;
            _accountsForm.Hide();
        };
    }

    private void accountsToolStripMenuItem_Click(object sender, EventArgs e) {
        _accountsForm.Show();
        _accountsForm.Focus();
    }

    #endregion

    #region OrderBookForms

    private Collection<OrderBookForm> _orderBookForms = new();

    private void orderBookToolStripMenuItem_Click(object sender, EventArgs e) {
        var orderBookForm = new OrderBookForm(_state);
        _orderBookForms.Add(orderBookForm);
        orderBookForm.Show();
        orderBookForm.FormClosed += (o, args) => { _orderBookForms.Remove(orderBookForm); };
    }

    #endregion

    #region OrderBookFormSimulation

    private void orderBookSimulationToolStripMenuItem_Click(object sender, EventArgs e) {
        var orderBookSimulation = new OrderBookFormSimulation();
        orderBookSimulation.Show();
    }

    #endregion

    #region DataServiceForm

    private DataServiceForm? _dataServiceForm;

    private void dataToolStripMenuItem_Click(object sender, EventArgs e) {
        (_dataServiceForm ??= (DataServiceForm)
                new FormBuilder(() => {
                        var form = new DataServiceForm();
                        form.Init(_state);
                        return form;
                    })
                    .SetHiddenOnClose()
                    .Build())
            .Show()
            .Focus();
    }

    #endregion

    #region SubscriptionsForm

    private SubscriptionForm _subscriptionsForm;

    private void subscriptionsToolStripMenuItem_Click(object sender, EventArgs e) {
        _subscriptionsForm ??= (SubscriptionForm)
            new FormBuilder(new SubscriptionForm(_state))
                .SetHiddenOnClose()
                .Build();
        _subscriptionsForm
            .LoadSubscriptions(State.Storage.LocalSubscriptionsStorage)
            .Show()
            .Focus();
    }

    #endregion

    #region SymbolForm

    private SymbolForm _symbolForm;

    private void symbolsToolStripMenuItem_Click(object sender, EventArgs e) {
        _symbolForm ??= (SymbolForm)
            new FormBuilder(new SymbolForm())
                .SetHiddenOnClose()
                .Build();
        _symbolForm
            .LoadSymbols(State.Storage.LocalSymbolsStorage)
            .Show()
            .Focus();
    }

    #endregion

    #region ConnectionForm

    private ConnectionForm? _connectionForm;

    private void connectionsToolStripMenuItem_Click(object sender, EventArgs e) {
        _connectionForm ??= (ConnectionForm)
            new FormBuilder(new ConnectionForm())
                .SetHiddenOnClose()
                .Build();
        _connectionForm.Connections = State.Data.Connections;
        _connectionForm.DataSaver   = Storage.LocalConnectionsStorage;
        _connectionForm
            .Show()
            .Focus();
    }

    #endregion
}