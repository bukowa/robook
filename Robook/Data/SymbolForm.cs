using System.ComponentModel;

namespace Robook.Data;

public partial class SymbolForm : BaseForm {
    private BindingList<Symbol> symbols;
    
    public BindingList<Symbol> Symbols {
        get => symbols;
        set {
            symbols                  = value;
            SymbolDataGridView.DataSource = symbols;
        }
    }
    
    public DataGridView        SymbolDataGridView { get; set; } = new();
    
    public SymbolForm() {
        InitializeComponent();
        SymbolDataGridView.Dock = DockStyle.Fill;
        Controls.Add(SymbolDataGridView);
    }
    
}