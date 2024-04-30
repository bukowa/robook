using System.ComponentModel;

namespace Robook.Data;

public partial class SymbolForm : BaseForm {
    private BindingList<Symbol> symbols;
    public IBindingListDataSaver<Symbol> SymbolsDataSaver;
    
    public BindingList<Symbol> Symbols {
        get => symbols;
        set {
            symbols = value;
            dataGridView1.DataSource = symbols;
            dataGridView1.Columns["Self"]!.Visible = false;
            dataGridView1.Columns["DisplayName"]!.Visible = false;
            dataGridView1.Columns["Id"]!.Visible = false;
        }
    }


    public SymbolForm() {
        InitializeComponent();
        dataGridView1.Dock = DockStyle.Fill;
    }

    private void button1_Click(object sender, EventArgs e) {
        SymbolsDataSaver.Save(Symbols);
    }
}