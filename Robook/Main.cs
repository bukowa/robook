using System.Collections.ObjectModel;
using Robook.Accounts;
using Robook.DataServiceFormNS;
using Robook.OrderBookFormNS;

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
        if (_dataServiceForm == null) {
            _dataServiceForm = new DataServiceForm();
            _dataServiceForm.Init(_state);
            _dataServiceForm.FormClosing += (o, args) => {
                args.Cancel = true;
                _dataServiceForm.Hide();
            };
        }
        _dataServiceForm.Show();
        _dataServiceForm.Focus();
    }
    #endregion
    
}