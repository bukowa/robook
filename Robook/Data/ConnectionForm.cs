using System.ComponentModel;

namespace Robook.Data;

public partial class ConnectionForm : BaseForm {
    private DataGridView            dataGridView1;
    public  IDataSaver<Connection>  DataSaver;
    private BindingList<Connection> connections;

    public BindingList<Connection> Connections {
        get => connections;
        set {
            connections              = value;
            dataGridView1.DataSource = connections;
        }
    }

    public ConnectionForm() {
        InitializeComponent();
        SetupDataGridView();
    }

    private void SetupDataGridView() {
        dataGridView1 = new DataGridView() {
            Dock = DockStyle.Fill,
        };

        AddOnClickColumn(new DataGridViewButtonColumn() {
            Name = "Save"
        }, (args, i) => {
            var c = Connections[i];
            DataSaver.Save(Connections);
        });

        AddOnClickColumn(new DataGridViewButtonColumn() {
            Name = "Login"
        }, (args, i) => {
            var c = Connections[i];
            Task.Run(async () => {
                try {
                    await c.LoginAsync();
                }
                catch (Exception e) {
                    MessageBox.Show(e.Message);
                }
            });
        });

        AddOnClickColumn(new DataGridViewButtonColumn {
            Name = "Logout"
        }, (args, i) => {
            var c = Connections[i];
            Task.Run(async () => {
                try {
                    await c.LogoutAsync();
                }
                catch (Exception e) {
                    MessageBox.Show(e.Message);
                }
            });
        });

        AddOnClickColumn(new DataGridViewButtonColumn {
            Name = "Delete"
        }, (args, i) => { });

        Controls.Add(dataGridView1);
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