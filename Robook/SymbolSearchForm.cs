using System.Collections.ObjectModel;
using System.ComponentModel;
using com.omnesys.rapi;
using Rithmic;

namespace Robook;

public partial class SymbolSearchForm : Form {
    public SymbolSearchForm() {
        InitializeComponent();
        SetupComboBox<SearchField>(searchFieldComboBox);
        SetupComboBox<SearchOperator>(searchOperatorComboBox);
    }

    private void searchButton_Click(object sender, EventArgs e) {
        var ctx = new Context();
        var st  = GetSearchTerm();
        // State.RHandler.InstrumentSearchClb.Subscribe(ctx, (context, info) => {
        //     Console.WriteLine(info.Instruments.ToString());
        //     Invoke(() => {
        //         searchGridView.DataSource = new BindingList<RefDataInfo>(info.Instruments.Select(x => x).ToList());
        //     });
        // });
        // State.Client.Engine.searchInstrument
        // (
        //     exchangeTextBox.Text,
        //     new ReadOnlyCollection<SearchTerm>(new List<SearchTerm> { GetSearchTerm() }),
        //     ctx
        // );
    }

    public SearchTerm GetSearchTerm() {
        return new SearchTerm
        (
            (SearchField)searchFieldComboBox.SelectedValue,
            (SearchOperator)searchFieldComboBox.SelectedValue,
            searchTermTextBox.Text,
            caseSensitiveCheckBox.Checked
        );
    }

    private static void SetupComboBox<TEnum>(ComboBox comboBox)
        where TEnum : struct, Enum {
        comboBox.DisplayMember = "DisplayText";
        comboBox.ValueMember   = "EnumValue";
        comboBox.DataSource =
            Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Select(x => new ComboItem<TEnum>(x, x.ToString())).ToArray();
    }

    public record ComboItem<TEnum>(TEnum EnumValue, string DisplayText);
}