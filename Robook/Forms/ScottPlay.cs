using ScottPlot.WinForms;

namespace Robook.Forms;

public partial class ScottPlay : Form {
    public ScottPlay() {
        InitializeComponent();
        var form = new FormsPlot();
        form.Dock = DockStyle.Fill;
        Controls.Add(form);
        double[] dataX = { 1, 2, 3, 4, 5 };
        double[] dataY = { 1, 4, 9, 16, 25 };

        form.Plot.Add.Scatter(dataX, dataY);
        form.Refresh();
    }
}