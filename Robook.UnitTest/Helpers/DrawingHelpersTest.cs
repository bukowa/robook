using System.Drawing;
using Robook.Helpers;

namespace Robook.UnitTest.Helpers;

public class DrawingHelpersTest {
    [Test]
    public void TestRelativePctRectangle() {
        var parent = new Rectangle(10, 5, 6, 2);
        var exp    = new Rectangle(10, 5, 3, 1);

        Assert.That(DrawingHelpers.RelativePctRectangle(parent, 50, 50), Is.EqualTo(exp));
    }

    [Test]
    public void TestRelativePctRectangle_RightBottomAlignment() {
        var parent = new Rectangle(10,  10, 200, 100);
        var exp    = new Rectangle(110, 60, 100, 50);

        Assert.That(
            DrawingHelpers.RelativePctRectangle(parent, 50, 50, HorizontalAlignment.Right, VerticalAlignment.Bottom),
            Is.EqualTo(exp));
    }

    [Test]
    public void TestRelativePctRectangle_LeftCenterAlignment() {
        var parent = new Rectangle(10, 10, 4, 8);
        var exp    = new Rectangle(10, 12, 2, 4);

        Assert.That(
            DrawingHelpers.RelativePctRectangle(parent, 50, 50, HorizontalAlignment.Left, VerticalAlignment.Center),
            Is.EqualTo(exp));
    }

    [Test]
    public void TestRelativePctRectangle_CenterTopAlignment() {
        var parent = new Rectangle(10, 10, 4, 8);
        var exp    = new Rectangle(11, 10, 2, 4);

        Assert.That(
            DrawingHelpers.RelativePctRectangle(parent, 50, 50, HorizontalAlignment.Center, VerticalAlignment.Top),
            Is.EqualTo(exp));
    }

    [Test]
    public void RectangleMidPointTest() {
        var parent = new Rectangle(0, 0, 100, 200);
        var exp    = new Point(50, 100);

        Assert.That(DrawingHelpers.RectangleMidPoint(parent), Is.EqualTo(exp));
    }
}