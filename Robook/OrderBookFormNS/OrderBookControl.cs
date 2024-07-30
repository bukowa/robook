using System.Data;
using System.Diagnostics;
using System.Reflection;
using Robook.Helpers;
using Robook.OrderBookColumns;
using Robook.OrderBookNS;
using Timer = System.Windows.Forms.Timer;

namespace Robook.OrderBookFormNS;

/// <summary>
///     This class exposes methods to create and manage
///     a <see cref="DataGridView"/> control that displays an <see cref="OrderBookNS.OrderBook"/>.
/// </summary>
public class OrderBookDataGridControl {
    public IOrderBook          OrderBook;
    public DataGridView       DataGridView;
    public OrderBookProcessor OrderBookProcessor;

    public OrderBookDataGridControl(
        IOrderBook          orderBook,
        DataGridView       dataGridView,
        OrderBookProcessor orderBookProcessor
    ) {
        OrderBook          = orderBook;
        DataGridView       = dataGridView;
        OrderBookProcessor = orderBookProcessor;

        SetDataGridViewDoubleBuffered(true);

        DataGridView.RowsAdded       += DataGridView_RowsAdded;
        DataGridView.ColumnAdded     += DataGridView_ColumnAdded;
        OrderBook.DataTable.ColumnChanged += OrderBook_OnColumnChanged;

        #region "Auto generate columns in DataGridView"

        // Disable AutoGenerateColumns as custom columns are used in DataGridView
        DataGridView.AutoGenerateColumns = false;

        #endregion

        #region "DataGridView Virtual Mode"

        // Enable virtual mode for optimal performance of the DataGridView
        // Fallback: If virtual mode is disabled for some reason, bind DataSource to maintain reliability
        DataGridView.VirtualMode = true;
        if (DataGridView.VirtualMode) {
            HandleVirtualMode();
        }
        else {
            DataGridView.DataSource = OrderBook.DataTable.AsDataView();
        }
        #endregion

        #region "Additional DataGridView Configuration"

        DataGridView.ReadOnly                = true;
        DataGridView.AllowUserToOrderColumns = true;

        #endregion
    }

    /// <summary>
    /// Event handler that subscribes cells of type <see cref="AbstractOrderBookCell"/> to events triggered
    /// by changes in their parent column properties when a new column is added to the DataGridView.
    /// </summary>
    private void DataGridView_ColumnAdded(object? sender, DataGridViewColumnEventArgs e) {
        try {
            if (e.Column is AbstractOrderBookColumn) {
                foreach (DataGridViewRow row in DataGridView.Rows) {
                    foreach (DataGridViewCell cell in row.Cells) {
                        if (cell is AbstractOrderBookCell abstractDataCell) {
                            abstractDataCell.SubscribeToColumnPropertyChangedEvents();
                        }
                    }
                }
            }
        }
        catch (Exception err) {
            Console.WriteLine(err.Message);
            throw;
        }
    }

    /// <summary>
    /// Event handler that subscribes cells of type <see cref="AbstractOrderBookCell"/> to events triggered
    /// by changes in their parent column properties when a new row is added to the DataGridView.
    /// </summary>
    private void DataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e) {
        try {
            for (var i = e.RowIndex; i < e.RowIndex + e.RowCount; i++) {
                foreach (DataGridViewCell cell in DataGridView.Rows[i].Cells) {
                    if (cell is AbstractOrderBookCell abstractDataCell) {
                        abstractDataCell.SubscribeToColumnPropertyChangedEvents();
                    }
                }
            }
        }
        catch (Exception err) {
            Console.WriteLine(err.Message);
            throw;
        }
    }

    /// <summary>
    /// <para>Event handler that notifies <see cref="AbstractOrderBookColumn"/> and <see cref="AbstractOrderBookCell"/>
    /// instances about a change in the value of the <see cref="OrderBookNS.OrderBook.OBDT"/> column.</para>
    /// <para>This change typically influences the visual representation of the corresponding column in the DataGridView.</para>
    /// </summary>
    private void OrderBook_OnColumnChanged(object sender, DataColumnChangeEventArgs e) {
        try {
            var dataSourceColumnName = e.Column.ColumnName;
            var dataSourceRowIndex   = e.Row.Table.Rows.IndexOf(e.Row);

            // Safety check: Ensure the row index from the DataTable is within the valid range in the DataGridView
            if (dataSourceRowIndex < 0 || dataSourceRowIndex >= DataGridView.Rows.Count) {
                throw new IndexOutOfRangeException("Data Table row index is out of range for the Data Grid View.");
            }

            // Get all columns in the DataGridView bound to the changed OrderBook DataTable column
            var boundedColumns = DataGridViewHelpers.GetColumnsByDataPropertyName(DataGridView, dataSourceColumnName);
            if (boundedColumns.Count > 0) {
                foreach (var columnIndex in boundedColumns) {
                    var col = DataGridView.Columns[columnIndex];
                    if (col is AbstractOrderBookColumn abstractDataColumn) {
                        abstractDataColumn.OnColumnChanged(e, OrderBook);
                    }

                    var cell = DataGridView[columnIndex, dataSourceRowIndex];
                    if (cell is AbstractOrderBookCell abstractDataCell) {
                        abstractDataCell.OnCellValueChanged(e, OrderBook);
                    }

                    // Invalidate cell in Virtual Mode to trigger OnCellValueNeeded and update cell value
                    if (DataGridView.VirtualMode == true) DataGridView.InvalidateCell(cell);
                }
            }
        }
        catch (Exception err) {
            Console.WriteLine(err.Message);
            throw;
        }
    }

    #region "Configure DataGridView Virtual Mode"

    /// <summary>
    /// <para>Prepares the DataGridView for virtual mode by setting the row and column counts.</para>
    /// <para>Subscribes to the CellValueNeeded event for handling virtual mode data retrieval.</para>
    /// </summary>
    private void HandleVirtualMode() {
        // Note:
        // Columns for OrderBookDataGridControl are intended to be added at a later stage;
        // initially, columns count should be 0.
        //
        // Issue workaround: (todo: find the root cause of the unexpected empty column addition)
        // In virtual mode, setting DataGridView.ColumnCount to 0 unexpectedly adds an empty column.
        // To fix this issue, a hidden dummy column is added, and ColumnCount is set to 1.
        DataGridView.Columns.Add(new DataGridViewTextBoxColumn() {
            Name    = "Dummy Column",
            Visible = false,
        });
        // DataGridView.AutoGenerateColumns should be disabled in OrderBookDataGridControl
        // Fallback added for reliability if for some reason AutoGenerateColumns would be enabled
        DataGridView.ColumnCount = DataGridView.AutoGenerateColumns ? OrderBook.DataTable.Columns.Count : 1;
        DataGridView.RowCount    = OrderBook.DataTable.Rows.Count;

        DataGridView.CellValueNeeded += DataGridView_CellValueNeeded;
    }

    private void DataGridView_CellValueNeeded(object? sender, DataGridViewCellValueEventArgs e) {
        var colName = DataGridView.Columns[e.ColumnIndex].DataPropertyName;

        if (!String.IsNullOrEmpty(colName) || OrderBook.DataTable.Columns.Contains(colName)) {
            e.Value = OrderBook.DataTable.Rows[e.RowIndex][colName];
        }
    }

    #endregion

    #region "Adding and removing columns from the DataGridView"

    /// <summary>
    /// <para>Adds a DataGridViewColumn to the DataGridView, ensuring uniqueness and validity
    /// in the associated <see cref="OrderBookNS.OrderBook"/>.</para>
    /// <para>The DataGridViewColumn must have both Name and DataPropertyName attributes specified.</para>
    /// </summary>
    /// <param name="column">The DataGridViewColumn to add.</param>
    public void AddColumn(DataGridViewColumn column) {
        if (string.IsNullOrEmpty(column.Name)) {
            throw new Exception("Please provide column Name.");
        }

        if (DataGridView.Columns.Contains(column.Name)) {
            throw new Exception("Column name already exist. Please provide unique column name.");
        }

        if (string.IsNullOrEmpty(column.DataPropertyName)) {
            throw new Exception("Please provide column DataPropertyName.");
        }

        if (!OrderBook.DataTable.Columns.Contains(column.DataPropertyName)) {
            throw new Exception("DataPropertyName is invalid. Column with provided name does not exist in OrderBook.");
        }

        DataGridView.Columns.Add(column);
    }

    /// <summary>
    /// Removes a DataGridViewColumn by reference.
    /// </summary>
    /// <param name="column">The DataGridViewColumn to remove.</param>
    public void RemoveColumn(DataGridViewColumn column) {
        DataGridView.Columns.Remove(column);
    }

    /// <summary>
    /// Removes a DataGridViewColumn by name.
    /// </summary>
    /// <param name="columnName">The name of the DataGridViewColumn to remove.</param>
    public void RemoveColumn(string columnName) {
        DataGridView.Columns.Remove(columnName);
    }

    #endregion

    /// <summary>
    /// Sets the DoubleBuffered property of the DataGridView to reduce flickering during rendering.
    /// </summary>
    public void SetDataGridViewDoubleBuffered(bool value) {
        typeof(System.Windows.Forms.DataGridView).InvokeMember(
            "DoubleBuffered",
            BindingFlags.NonPublic
            | BindingFlags.Instance
            | BindingFlags.SetProperty,
            null,
            DataGridView,
            new object[] { value });
    }
}