using Robook.Helpers;

namespace Robook.UnitTest.Helpers;

public class MathHelpersTest {
    [Test]
    public void TestRelativePercentOfMax_PositiveValues() {
        var arr = new double[] { 10, 25, 50 };
        var exp = new double[] { 20, 50, 100 };
        Assert.That(MathHelpers.RelativePercentOfMax(arr), Is.EqualTo(exp));
    }

    [Test]
    public void TestRelativePercentOfMax_MaxZeroValues() {
        var arr = new double[] { -1, -2, 0 };
        var exp = new double[] { 0, 0, 0 };
        Assert.That(MathHelpers.RelativePercentOfMax(arr), Is.EqualTo(exp));
    }

    [Test]
    public void TestRelativePercentOfMaxValue_PositiveValues() {
        double[]     arr   = { 10, 25, 50 };
        const double value = 10;
        const double exp   = 20;

        Assert.That(MathHelpers.RelativePercentOfMaxValue(value, arr), Is.EqualTo(exp));
    }

    [Test]
    public void TestRelativePercentOfMaxValue_MaxZeroValues() {
        double[]     arr   = { -1, -0.25, 0 };
        const double value = 10;
        const double exp   = 0;

        Assert.That(MathHelpers.RelativePercentOfMaxValue(value, arr), Is.EqualTo(exp));
    }
}