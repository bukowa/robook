using System.Collections.Concurrent;
using com.omnesys.rapi;
using Robook.DataServiceNS;
using Robook.State;
using Robook.SymbolNS;

namespace Robook.DataServiceFormNS;

public partial class DataServiceForm : Form {

    public DataServiceForm() {
        InitializeComponent();
        comboBox1.DataSource         = Enum.GetValues(typeof(BarType));
        var dt = dateTimePicker1.Value;
        
        dateTimePicker1.Format       = DateTimePickerFormat.Custom;
        dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss";
        dateTimePicker1.Value        = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
        
        dateTimePicker2.Format       = DateTimePickerFormat.Custom;
        dateTimePicker2.CustomFormat = "yyyy-MM-dd HH:mm:ss";
        dateTimePicker2.Value        = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
    }

    private State.State State { get; set; }

    public void Init(State.State state) {
        State = state;
        accountsSelectControl1.Init(State.AccountsStore.Accounts);
        comboBox1.SelectedIndex = 1;
    }

    private void textBox4_TextChanged(object sender, EventArgs e) {
        textBox4.Text = (folderBrowserDialog1.ShowDialog() == DialogResult.OK) ? folderBrowserDialog1.SelectedPath : textBox4.Text;
    }

    private async void button1_Click(object sender, EventArgs e) {
        // check if folder exists
        if (!Directory.Exists(textBox4.Text)) {
            MessageBox.Show("Please select a valid folder");
            return;
        }
        var exchange  = textBox2.Text;
        var symbol    = textBox1.Text;
        var filePath  = Path.Join(textBox4.Text, $"{exchange}_{symbol}.parquet");
        
        var barType   = (BarType)comboBox1.SelectedItem;
        var barPeriod = Int32.Parse(textBox3.Text);
        var start     = dateTimePicker1.Value;
        start = DateTime.SpecifyKind(start, DateTimeKind.Utc);
        var end       = dateTimePicker2.Value;
        end = DateTime.SpecifyKind(end, DateTimeKind.Utc);
        var acc       = accountsSelectControl1.SelectedAccount;
        
        if (acc.Client?.HistoricalDataConnection?.LastAlertInfo?.AlertType != AlertType.LoginComplete) {
            MessageBox.Show("Please login first");
            return;
        }
        
        var ds = new DataService(new Symbol(exchange, symbol), textBox4.Text);
        ds.Symbol.SetClient(acc.Client);
        await ds.GetHistory(new DataService.History.Opts() {
            Start = start,
            End = end,
            BarPeriod = barPeriod,
            BarType = barType,
        });
        // await ds.ReplayBars(start, end, barPeriod, barType, filePath, info => {
        //     // Invoke(() => {richTextBox1.AppendText(info.ToString());});
        // });
        MessageBox.Show("ReplayBars done");
    }
}