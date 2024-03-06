using System.Data;
using Robook.Helpers;

namespace Robook.UnitTest.Helpers;

public class DataTableHelpersTest {
    DataTable dt;

    [SetUp]
    public void Setup() {
        dt = new DataTable();
    }

    [TearDown]
    public void TearDown() {
        dt.Dispose();
    }

    [Test]
    public void TestFillColumnWithRandomNumber() {
        dt.Columns.Add("Column1");

        for (int i = 1; i <= 3; i++) {
            dt.Rows.Add(dt.NewRow());
        }

        DataTableHelpers.FillColumnWithRandomNumber<double>("Column1", dt, 5D, 55D);

        foreach (DataRow row in dt.Rows) {
            double value = Convert.ToDouble(row["Column1"]);
            Assert.That(value, Is.InRange(5D, 55D));
        }
    }

    [Test]
    [Description("Verifies that attempting to add numeric data to a non-existent column throws an ArgumentException.")]
    public void TestFillColumnWithRandomNumber_ThrowsArgumentException() {
        Assert.Throws<ArgumentException>(() => {
            DataTableHelpers.FillColumnWithRandomNumber("Non existent column", dt, 0, 10);
        });
    }

    [Test]
    public void TestFillColumnWithRandomString() {
        dt.Columns.Add("Column1");

        for (int i = 1; i <= 5; i++) {
            dt.Rows.Add(dt.NewRow());
        }

        DataTableHelpers.FillColumnWithRandomString("Column1", dt, 10);

        foreach (DataRow row in dt.Rows) {
            var value = row["Column1"].ToString();
            Assert.That(value.Length, Is.EqualTo(10));
        }
    }

    [Test]
    [Description("Verifies that attempting to add string data to a non-existent column throws an ArgumentException.")]
    public void TestFillColumnWithRandomString_ThrowsArgumentException() {
        Assert.Throws<ArgumentException>(() => {
            DataTableHelpers.FillColumnWithRandomString("Non existent column", dt, 10);
        });
    }

    [Test]
    public void TestFillRowWithRandomNumber() {
        dt.Columns.Add("Column1", typeof(double));
        dt.Columns.Add("Column2", typeof(double));

        DataRow row1 = dt.NewRow();
        DataRow row2 = dt.NewRow();
        dt.Rows.Add(row1);
        dt.Rows.Add(row2);

        DataTableHelpers.FillRowWithRandomNumber<double>(0, dt, 5D, 55D);

        foreach (DataColumn column in dt.Columns) {
            Assert.That(Convert.ToDouble(row1[column]), Is.InRange(5D, 55D));
            Assert.That(row2[column].ToString(),        Is.Empty);
        }
    }

    [Test]
    [Description(
        "Verifies that attempting to add numeric data to a non-existent row index throws an ArgumentException.")]
    public void TestFillRowWithRandomNumber_ThrowsArgumentException() {
        var exc = Assert.Throws<ArgumentException>(() => {
            DataTableHelpers.FillRowWithRandomNumber<double>(10, dt, 0, 10);
        });
        Assert.That(exc.Message, Does.Contain("Row index 10 is out of bounds for the DataTable."));
    }

    [Test]
    public void TestFillRowWithRandomString() {
        dt.Columns.Add("Column1", typeof(string));
        dt.Columns.Add("Column2", typeof(string));

        DataRow row1 = dt.NewRow();
        DataRow row2 = dt.NewRow();
        dt.Rows.Add(row1);
        dt.Rows.Add(row2);

        DataTableHelpers.FillRowWithRandomString(0, dt, 10);

        foreach (DataColumn column in dt.Columns) {
            Assert.That(row1[column].ToString(),        Is.Not.Empty);
            Assert.That(row1[column].ToString().Length, Is.EqualTo(10));
            Assert.That(row2[column].ToString(),        Is.Empty);
        }
    }

    [Test]
    [Description(
        "Verifies that attempting to add string data to a non-existent row index throws an ArgumentException.")]
    public void TestFillRowWithRandomString_ThrowsArgumentException() {
        var exc = Assert.Throws<ArgumentException>(() => { DataTableHelpers.FillRowWithRandomString(10, dt, 2); });
        Assert.That(exc.Message, Does.Contain("Row index 10 is out of bounds for the DataTable."));
    }
}