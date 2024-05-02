using System.ComponentModel;
using Robook.Accounts;

namespace Robook.Data;

// its very important to first bind the data source of the columns
// and only then set the data source of the grid view
public partial class SubscriptionForm : BaseForm {
    private BindingList<Subscription> _subscriptions;
    public IBindingListDataSaver<Subscription> SubscriptionsDataSaver { get; set; }
    
    public BindingList<Subscription> Subscriptions {
        get => _subscriptions;
        set {
            _subscriptions = value;
            dataGridView1.DataSource = Subscriptions;
        }
    }

    private BindingList<Symbol> _symbols;

    public BindingList<Symbol> Symbols {
        get => _symbols;
        set {
            _symbols = value;
            _symbolColumn.DataSource = Symbols;
        }
    }

    private BindingList<Connection> _connections;

    public BindingList<Connection> Connections {
        get => _connections;
        set {
            _connections = value;
            _connectionColumn.DataSource = Connections;
        }
    }

    private readonly DataGridViewComboBoxColumn _symbolColumn;
    private readonly DataGridViewComboBoxColumn _connectionColumn;

    public SubscriptionForm() {
        InitializeComponent();
        _symbolColumn = new DataGridViewComboBoxColumn {
            DataPropertyName = "Symbol",
            ValueType = typeof(Symbol),
            DisplayMember = "DisplayName",
            ValueMember = "Self",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
        };
        dataGridView1.Columns.Add(_symbolColumn);
        _connectionColumn = new DataGridViewComboBoxColumn {
            DataPropertyName = "Connection",
            ValueType = typeof(Connection),
            DisplayMember = "DisplayName",
            ValueMember = "Self",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
        };
        dataGridView1.Columns.Add(_connectionColumn);
        AddOnClickColumn(new DataGridViewButtonColumn {
            Name = "Subscribe"
        }, (args, i) => {
            var c = Subscriptions[i];
            Task.Run(async () => {
                try {
                    if (c.TickSize == null && c.PointValue == null) {
                        await c.PriceIncrInfoTask();
                        await c.RefDataTask();   
                    }
                    c.StartStream();
                    Task.Run(() => { c.Start(); });
                }
                catch (Exception e) {
                    MessageBox.Show(e.Message);
                }
            });
        });
        AddOnClickColumn(new DataGridViewButtonColumn {
            Name = "Unsubscribe"
        }, (args, i) => {
            var c = Subscriptions[i];
            Task.Run(() => {
                c.StopStream();
                c.Cts.Cancel();
            });
        });
        dataGridView1.Dock = DockStyle.Fill;
        dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
        dataGridView1.CellClick += categoryDataGridView_CellClick;
    }

    
    private void button1_Click(object sender, EventArgs e) {
        SubscriptionsDataSaver.Save(Subscriptions);
    }
    
    private void categoryDataGridView_CellClick(object? sender, DataGridViewCellEventArgs e) {
        // You can check for e.ColumnIndex to limit this to your specific column
        if (dataGridView1.EditingControl is DataGridViewComboBoxEditingControl editingControl)
            editingControl.DroppedDown = true;
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
                }
            }
        };
        dataGridView1.Columns.Add(c);
    }
    
}