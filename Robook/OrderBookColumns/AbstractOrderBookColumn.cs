using System.Data;
using Robook.OrderBookNS;
using Robook.OrderBookFormNS;

namespace Robook.OrderBookColumns;

/// <summary>
/// Represents a specialized DataGridView column designed for use in an <see cref="OrderBookDataGridControl"/>.
/// </summary>
public abstract class AbstractOrderBookColumn : DataGridViewTextBoxColumn {
    // todo: Handle CellTemplate to allow only templates of type AbstractOrderBookCell to be assigned
    public AbstractOrderBookColumn() : base() {
    }

    /// <summary>
    /// <para>Occurs after a value has been changed for the <see cref="IOrderBook.DataTable"/> column,
    /// bound to the specified <see cref="OrderBookDataGridControl.DataGridView"/> column.</para>
    /// <para>Calls <see cref="RecalculateProperties"/> to update visual properties based on the new values.</para>
    /// </summary>
    /// <param name="e">Event arguments providing information about the cell change.</param>
    /// <param name="orderBook">The <see cref="IOrderBook"/> associated with the <see cref="OrderBookDataGridControl.DataGridView"/>.</param>
    public abstract void OnColumnChanged(DataColumnChangeEventArgs e, IOrderBook orderBook);

    /// <summary>
    /// <para>Recalculates properties associated with the column, influencing the visual representation of its cells.</para>
    /// <para>After recalculating column properties, changes trigger events that subsequently update the cells.</para>
    /// <para>The recalculation is initiated when the column cell value changes through <see cref="OnColumnChanged"/>.</para>
    /// </summary>
    /// <param name="orderBook">The <see cref="IOrderBook"/> associated with the <see cref="OrderBookDataGridControl.DataGridView"/>.</param>
    public abstract void RecalculateProperties(IOrderBook orderBook);
}