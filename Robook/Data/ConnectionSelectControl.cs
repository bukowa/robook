using System.ComponentModel;

namespace Robook.Data;

/// <summary>
/// ConnectionSelectControl is a user control that allows the user to select a connection.
/// </summary>
public partial class ConnectionSelectControl : UserControl {
    private BindingList<Connection> _connections;

    public BindingList<Connection> Connections {
        get => _connections;
        set {
            _connections = value;
            comboBox1.DataSource = _connections;
        }
    }

    public ConnectionSelectControl() {
        InitializeComponent();
        comboBox1.Dock = DockStyle.Fill;
    }

}