using System.ComponentModel;
using Robook.Accounts;

namespace Robook.Data;

public partial class SubscriptionForm : BaseForm {
    
    
    public BindingList<Subscription> Subscriptions = new();
    
    private State.State State { get; }
    public SubscriptionForm(State.State state) {
        State = state;
        InitializeComponent();
        // dataGridView1            = new DataGridView();
        dataGridView1.Dock       = DockStyle.Fill;
        dataGridView1.DataSource = Subscriptions;
        Subscriptions.AllowNew   = true;
        Subscriptions.ListChanged += (_, _) => {
            
        };
        // accountsSelectControl1.Init(state.AccountsStore.Accounts);
        Subscriptions.Add(new Subscription());
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