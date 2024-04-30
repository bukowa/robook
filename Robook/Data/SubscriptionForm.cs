﻿using System.ComponentModel;
using Robook.Accounts;

namespace Robook.Data;

// its very important to first bind the data source of the columns
// and only then set the data source of the grid view
public partial class SubscriptionForm : BaseForm {
    private BindingList<Subscription> _subscriptions;

    public BindingList<Subscription> Subscriptions {
        get => _subscriptions;
        set {
            _subscriptions           = value;
            dataGridView1.DataSource = Subscriptions;
        }
    }

    private BindingList<Symbol> _symbols;

    public BindingList<Symbol> Symbols {
        get => _symbols;
        set {
            _symbols                 = value;
            _symbolColumn.DataSource = Symbols;
        }
    }

    private BindingList<Connection> _connections;

    public BindingList<Connection> Connections {
        get => _connections;
        set {
            _connections                 = value;
            _connectionColumn.DataSource = Connections;
        }
    }

    private readonly DataGridViewComboBoxColumn _symbolColumn;
    private readonly DataGridViewComboBoxColumn _connectionColumn;

    public SubscriptionForm() {
        InitializeComponent();
        _symbolColumn = new DataGridViewComboBoxColumn {
            DataPropertyName = "Symbol",
            ValueType        = typeof(Symbol),
            DisplayMember    = "DisplayName",
            ValueMember      = "Self",
            AutoSizeMode     = DataGridViewAutoSizeColumnMode.Fill,
        };
        dataGridView1.Columns.Add(_symbolColumn);
        _connectionColumn = new DataGridViewComboBoxColumn {
            DataPropertyName = "Connection",
            ValueType        = typeof(Connection),
            DisplayMember    = "DisplayName",
            ValueMember      = "Self",
            AutoSizeMode     = DataGridViewAutoSizeColumnMode.Fill,
        };
        dataGridView1.Columns.Add(_connectionColumn);
        dataGridView1.Dock      =  DockStyle.Fill;
        dataGridView1.EditMode  =  DataGridViewEditMode.EditOnEnter;
        dataGridView1.CellClick += categoryDataGridView_CellClick;

        dataGridView1.CellValueChanged += (sender, e) => { Console.WriteLine(); };
        dataGridView1.CellParsing      += (sender, e) => { Console.WriteLine(); };
    }

    private void categoryDataGridView_CellClick(object? sender, DataGridViewCellEventArgs e) {
        // You can check for e.ColumnIndex to limit this to your specific column
        if (this.dataGridView1.EditingControl is DataGridViewComboBoxEditingControl editingControl)
            editingControl.DroppedDown = true;
    }
}