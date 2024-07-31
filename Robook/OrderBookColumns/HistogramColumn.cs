using System.Data;
using System.Diagnostics.Metrics;
using Microsoft.ML;
using Robook.Helpers;
using Robook.OrderBookNS;
using HorizontalAlignment = Robook.Helpers.HorizontalAlignment;

namespace Robook.OrderBookColumns;

public class HistogramColumn : AbstractOrderBookColumn {
    private long _maxValue;

    public long MaxValue {
        get => _maxValue;
        set {
            if (_maxValue != value) {
                _maxValue = value;
                OnMaxValueChanged(_maxValue);
            }
        }
    }

    public HorizontalAlignment HistogramAlignment { set; get; }

    public HistogramColumn() : base() {
        CellTemplate = new HistogramCell();
        MaxValue     = 0;
    }

    public override void OnColumnChanged(DataColumnChangeEventArgs e, DataTable dataTable) {
        RecalculateProperties(dataTable);
    }

    public override void RecalculateProperties(DataTable dataTable) {
        RecalculateMaxValueProperty(dataTable);
    }

    public virtual void RecalculateMaxValueProperty(DataTable dataTable) {
        var newMaxValue = dataTable.AsEnumerable().Select(row => row.Field<long?>(DataPropertyName)).Max() ?? 0;
        MaxValue = newMaxValue;
    }

    # region "Events"

    public event EventHandler<long> MaxValueChanged;

    protected virtual void OnMaxValueChanged(long newMaxValue) {
        MaxValueChanged?.Invoke(this, newMaxValue);
    }

    #endregion
}