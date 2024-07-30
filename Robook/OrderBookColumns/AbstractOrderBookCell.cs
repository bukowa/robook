using System.Data;
using Robook.Helpers;
using Robook.OrderBookNS;
using Robook.OrderBookFormNS;

namespace Robook.OrderBookColumns;

/// <summary>
/// Represents a specialized DataGridView cell designed for use in an <see cref="OrderBookDataGridControl"/>.
/// </summary>
public abstract class AbstractOrderBookCell : DataGridViewTextBoxCell {
    public AbstractOrderBookCell() : base() {
    }

    /// <summary>
    /// <para>Subscribes the cell to events triggered by changes in its parent column properties.</para>
    /// <para>These events are invoked in response to a change in the <see cref="OrderBook.OBDT"/> column,
    /// which is bound to the parent column.</para>
    /// </summary>
    public abstract void SubscribeToColumnPropertyChangedEvents();

    /// <summary>
    /// <para>Occurs after a value has been changed for the <see cref="IOrderBook.DataTable"/> cell,
    /// bound to the specified <see cref="OrderBookDataGridControl.DataGridView"/> cell.</para>
    /// <para>Calls <see cref="RecalculateProperties"/> to update visual properties based on the new value.</para>
    /// </summary>
    /// <param name="e">Event arguments providing information about the cell change.</param>
    /// <param name="orderBook">The <see cref="OrderBook"/> associated with the <see cref="OrderBookDataGridControl.DataGridView"/>.</param>
    public abstract void OnCellValueChanged(DataColumnChangeEventArgs e, IOrderBook orderBook);

    /// <summary>
    /// <para>Recalculates properties associated with the cell, which typically influence its visual representation.</para>
    /// <para>The recalculation is triggered when the cell value changes through <see cref="OnCellValueChanged"/>.</para>
    /// </summary>
    /// <param name="orderBook">The <see cref="IOrderBook"/> associated with the <see cref="OrderBookDataGridControl.DataGridView"/>.</param>
    public abstract void RecalculateProperties(IOrderBook orderBook);

    /// <summary>
    /// Draws the specified text within a DataGridView cell, considering the cell's content alignment.
    /// </summary>
    /// <remarks>
    /// This method serves as a wrapper around <see cref="TextRenderer.DrawText"/>.
    /// </remarks>
    public void DrawTextWithAlignment(
        string                text,
        Rectangle             cellBounds,
        DataGridViewCellStyle cellStyle,
        Graphics              graphics,
        Font?                 font  = null,
        Color?                color = null,
        TextFormatFlags?      flags = null
    ) {
        TextRenderer.DrawText(
            graphics,
            text,
            font ?? cellStyle.Font ?? SystemFonts.DefaultFont,
            cellBounds,
            color ?? cellStyle.ForeColor,
            flags ?? DataGridViewHelpers.ComputeTextFormatFlagsForCellStyleAlignment(
                cellStyle.Alignment,
                cellStyle.WrapMode,
                // todo: Get right-to-left settings from control or window
                // for now do not bother with rtl
                false
            )
        );
    }
}