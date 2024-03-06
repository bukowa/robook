using System.Windows.Forms;
using Robook.Helpers;

namespace Robook.UnitTest.Helpers;

[TestFixture]
public class DataGridViewHelpersTest {
    [Test]
    [Description("Tests GetColumnsByDataPropertyName method with valid DataPropertyName")]
    public void ShouldReturnListOfColumnIndicesForMatchingDataPropertyName() {
        var dgv          = new DataGridView();
        var propertyName = "matched property name";

        DataGridViewColumn[] columns = [
            new DataGridViewTextBoxColumn() { Name = "Col index 0", DataPropertyName = propertyName },
            new DataGridViewTextBoxColumn() { Name = "Col index 1", DataPropertyName = "something" },
            new DataGridViewTextBoxColumn() { Name = "Col index 2", DataPropertyName = propertyName },
            new DataGridViewTextBoxColumn() { Name = "Col index 3", DataPropertyName = "else" },
            new DataGridViewTextBoxColumn() { Name = "Col index 4", DataPropertyName = propertyName },
        ];
        dgv.Columns.AddRange(columns);

        var result         = DataGridViewHelpers.GetColumnsByDataPropertyName(dgv, propertyName);
        var expectedResult = new List<int> { 0, 2, 4 };

        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [Test]
    [Description("Tests GetColumnsByDataPropertyName method with non-existent DataPropertyName")]
    public void ShouldReturnEmptyListForInvalidDataPropertyName() {
        var dgv = new DataGridView();

        DataGridViewColumn[] columns = [
            new DataGridViewTextBoxColumn() { DataPropertyName = "one" },
            new DataGridViewTextBoxColumn() { DataPropertyName = "two" },
            new DataGridViewTextBoxColumn() { DataPropertyName = "three" },
        ];
        dgv.Columns.AddRange(columns);

        var result    = DataGridViewHelpers.GetColumnsByDataPropertyName(dgv, "Property that does not exist");
        var emptyList = new List<int>();

        Assert.That(result, Is.EqualTo(emptyList));
    }
}