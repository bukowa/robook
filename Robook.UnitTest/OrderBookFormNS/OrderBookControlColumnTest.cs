using System.Collections.Concurrent;
using System.Data;
using System.Windows.Forms;
using Robook.OrderBookColumns;
using Robook.OrderBookFormNS;
using Robook.OrderBookNS;

namespace Robook.UnitTest.OrderBookFormNS;

[TestFixture]
public class OrderBookControlColumnTest {
    OrderBook                ob;
    DataGridView             dgv;
    OrderBookProcessor       obp;
    OrderBookDataGridControl obc;
    int                      initColCount;

    [SetUp]
    public void Setup() {
        dgv = new DataGridView();
        ob  = new OrderBook(0.25m, 100m, 10);
        obp = new OrderBookProcessor(ob, new ConcurrentQueue<object>());
        obc = new OrderBookDataGridControl(ob, dgv, obp);

        ob.AddColumn(new OrderBookDefaultColumn("Volume",     new[] { OrderBookColumnDataType.Trade }, typeof(int)));
        ob.AddColumn(new OrderBookDefaultColumn("SellVolume", new[] { OrderBookColumnDataType.Trade }, typeof(int)));

        initColCount = dgv.Columns.Count;
    }

    [TearDown]
    public void TearDown() {
        dgv.Dispose();
    }

    [Test]
    public void ShouldAddSingleColumn() {
        var colName = "My column";
        var col     = new DataGridViewTextBoxColumn() { DataPropertyName = "Volume", Name = colName };

        obc.AddColumn(col);

        Assert.That(dgv.Columns.Contains(colName), Is.True);
        Assert.That(dgv.Columns.Count,             Is.EqualTo(initColCount + 1));
    }

    [Test]
    public void ShouldAddMultipleColumns() {
        var colOneName = "Column 1";
        var colTwoName = "Column 2";
        var colOne     = new DataGridViewTextBoxColumn() { Name = colOneName, DataPropertyName = "Volume" };
        var colTwo     = new DataGridViewTextBoxColumn() { Name = colTwoName, DataPropertyName = "SellVolume" };

        obc.AddColumn(colOne);
        obc.AddColumn(colTwo);

        Assert.That(dgv.Columns.Contains(colOneName), Is.True);
        Assert.That(dgv.Columns.Contains(colTwoName), Is.True);
        Assert.That(dgv.Columns.Count,                Is.EqualTo(initColCount + 2));
    }

    [Test]
    public void ShouldAddColumnsWithSameDataPropertyName() {
        var colOneName = "Column 1";
        var colTwoName = "Column 2";
        var colOne     = new DataGridViewTextBoxColumn() { DataPropertyName = "Volume", Name = colOneName };
        var colTwo     = new DataGridViewTextBoxColumn() { DataPropertyName = "Volume", Name = colTwoName };

        obc.AddColumn(colOne);
        obc.AddColumn(colTwo);

        Assert.That(dgv.Columns.Contains(colOneName), Is.True);
        Assert.That(dgv.Columns.Contains(colTwoName), Is.True);
        Assert.That(dgv.Columns.Count,                Is.EqualTo(initColCount + 2));
    }

    [Test]
    public void ShouldNotAddTwoColumnsWithSameName() {
        var col = new DataGridViewTextBoxColumn() { DataPropertyName = "Volume", Name = "Name" };

        var exc = Assert.Throws<Exception>(() => {
            obc.AddColumn(col);
            obc.AddColumn(col);
        });

        Assert.That(exc.Message,       Does.Contain("Column name already exist. Please provide unique column name."));
        Assert.That(dgv.Columns.Count, Is.EqualTo(initColCount + 1));
    }

    [Test]
    public void ShouldNotAddColumnWithoutSettingName() {
        var col = new DataGridViewTextBoxColumn() { DataPropertyName = "Volume" };

        var exc = Assert.Throws<Exception>(() => { obc.AddColumn(col); });

        Assert.That(exc.Message,       Does.Contain("Please provide column Name."));
        Assert.That(dgv.Columns.Count, Is.EqualTo(initColCount));
    }

    [Test]
    public void ShouldNotAddColumnWithNullOrEmptyName() {
        var colOne = new DataGridViewTextBoxColumn() { DataPropertyName = "Volume", Name = null };
        var colTwo = new DataGridViewTextBoxColumn() { DataPropertyName = "Volume", Name = "" };

        var excOne = Assert.Throws<Exception>(() => { obc.AddColumn(colOne); });
        var excTwo = Assert.Throws<Exception>(() => { obc.AddColumn(colTwo); });

        Assert.That(excOne.Message,    Does.Contain("Please provide column Name."));
        Assert.That(excTwo.Message,    Does.Contain("Please provide column Name."));
        Assert.That(dgv.Columns.Count, Is.EqualTo(initColCount));
    }

    [Test]
    public void ShouldNotAddColumnWithoutSettingDataPropertyName() {
        var col = new DataGridViewTextBoxColumn() { Name = "Name" };

        var exc = Assert.Throws<Exception>(() => { obc.AddColumn(col); });

        Assert.That(exc.Message,       Does.Contain("Please provide column DataPropertyName."));
        Assert.That(dgv.Columns.Count, Is.EqualTo(initColCount));
    }

    [Test]
    public void ShouldNotAddColumnWithNullOrEmptyDataPropertyName() {
        var colOne = new DataGridViewTextBoxColumn() { Name = "Name", DataPropertyName = null };
        var colTwo = new DataGridViewTextBoxColumn() { Name = "Name", DataPropertyName = "" };

        var excOne = Assert.Throws<Exception>(() => { obc.AddColumn(colOne); });
        var excTwo = Assert.Throws<Exception>(() => { obc.AddColumn(colTwo); });

        Assert.That(excOne.Message,    Does.Contain("Please provide column DataPropertyName."));
        Assert.That(excTwo.Message,    Does.Contain("Please provide column DataPropertyName."));
        Assert.That(dgv.Columns.Count, Is.EqualTo(initColCount));
    }

    [Test]
    public void ShouldNotAddColumnWithNonExistentDataPropertyName() {
        var col = new DataGridViewTextBoxColumn() { Name = "Name", DataPropertyName = "Do not exist!" };

        var exc = Assert.Throws<Exception>(() => { obc.AddColumn(col); });

        Assert.That(exc.Message,
                    Does.Contain(
                        "DataPropertyName is invalid. Column with provided name does not exist in OrderBook."));
        Assert.That(dgv.Columns.Count, Is.EqualTo(initColCount));
    }

    [Test]
    public void ShouldRemoveColumnByName() {
        var colName = "My column";
        var col     = new DataGridViewTextBoxColumn() { DataPropertyName = "Volume", Name = colName };

        obc.AddColumn(col);
        obc.RemoveColumn(colName);

        Assert.That(dgv.Columns.Contains(colName), Is.False);
        Assert.That(dgv.Columns.Count,             Is.EqualTo(initColCount));
    }

    [Test]
    public void ShouldRemoveColumnByInstance() {
        var colName = "My column";
        var col     = new DataGridViewTextBoxColumn() { DataPropertyName = "Volume", Name = colName };

        obc.AddColumn(col);
        obc.RemoveColumn(col);

        Assert.That(dgv.Columns.Contains(colName), Is.False);
        Assert.That(dgv.Columns.Count,             Is.EqualTo(initColCount));
    }

    [Test]
    public void ShouldRaiseErrorWhenRemovedColumnDoesNotExist() {
        Assert.Throws<ArgumentException>(() => { obc.RemoveColumn("I do not exist"); });
    }
}