using Rithmic;

namespace Robook.SymbolNS;

public partial class SymbolTextBoxControl : UserControl {
    public (string symbol, string exchange) GetSymbolData() {
        var symbol = textBox1.Text.Split('.');
        if (symbol.Length < 2) {
            throw new Exception("Symbol must be in the format: SYMBOL.EXCHANGE");
        }

        return (symbol[0], symbol[1]);
    }

    public Symbol GetSymbol(Client client) {
        var sdata = GetSymbolData();
        var symbol = new Symbol(sdata.exchange, sdata.symbol);
        symbol.SetClient(client);
        return symbol;
    }

    public SymbolTextBoxControl() {
        InitializeComponent();
        textBox1.Dock = DockStyle.Fill;
        textBox1.Text = "NQU4.CME";
    }
}