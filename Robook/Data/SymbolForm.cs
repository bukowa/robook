using System.ComponentModel;

namespace Robook.Data;

public partial class SymbolForm : BaseForm {
    
    public DataGridView        SymbolDataGridView { get; set; } = new();
    public BindingList<Symbol> Symbols            { get; set; } = new();
    
    public SymbolForm() {
        InitializeComponent();
        SymbolDataGridView.Dock = DockStyle.Fill;
        Controls.Add(SymbolDataGridView);
    }
    
    public SymbolForm LoadSymbols(IDataHandler<Symbol> dataHandler) {
        Symbols                       =  dataHandler.Load();
        Symbols.ListChanged           += (sender, args) => dataHandler.Save(Symbols);
        Symbols.AllowNew              =  true;
        SymbolDataGridView.DataSource =  Symbols;
        return this;
    }
}