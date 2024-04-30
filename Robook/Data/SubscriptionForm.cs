using System.ComponentModel;
using Robook.Accounts;

namespace Robook.Data;

public partial class SubscriptionForm : BaseForm {

    public BindingList<Subscription> Subscriptions;

    public SubscriptionForm(State.State state) {
        InitializeComponent();
        dataGridView1.Dock       = DockStyle.Fill;
        var symbolColumn = new DataGridViewComboBoxColumn {
            // Name             = "Symbol",
            DataSource       = State.Storage.LocalSymbolsStorage.Load(), // Load the list of Symbols
            // DisplayMember    = "Name", // The property of Symbol to display in the dropdown list
            // ValueMember      = "Name", // The property of Symbol to use as the value
            DataPropertyName = "Symbol", // The property of Subscription to bind to
            HeaderText       = "Symbol",
            AutoSizeMode     = DataGridViewAutoSizeColumnMode.Fill
        };
        dataGridView1.DataError += (_, e) => {
            Console.WriteLine();
        };
        dataGridView1.Columns.Add(symbolColumn);

        var accountColumn = new DataGridViewComboBoxColumn {
            DataSource = State.Storage.LocalSymbolsStorage
        };
    }
    
    public SubscriptionForm LoadSubscriptions(IDataHandler<Subscription> dataHandler) {
        Subscriptions = dataHandler.Load();
        Subscriptions.ListChanged += (sender, args) => dataHandler.Save(Subscriptions);
        Subscriptions.AllowNew    = true;
        dataGridView1.DataSource   = Subscriptions;
        return this;
    }
    
    /// <summary>
    /// A convenient method for adding a column with a OnClick event.
    /// </summary>
    public void AddOnClickColumn(DataGridViewColumn c, Action<DataGridViewCellEventArgs, int> e) {
        dataGridView1.CellClick += (_, a) => {
            if (a.ColumnIndex >= 0 && a.RowIndex >= 0) {
                var col = dataGridView1.Columns[a.ColumnIndex];
                if (col == c) {
                    e(a, a.RowIndex);
                    return;
                }
                return;
            }
        };
        dataGridView1.Columns.Add(c);
    }
}