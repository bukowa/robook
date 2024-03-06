namespace Robook.Helpers;

/// <summary>
/// Specifies how a rectangle is horizontally aligned relative to a parent rectangle.
/// </summary>
public enum HorizontalAlignment {
    /// <summary>The rectangle is aligned on the left side of the parent rectangle.</summary>
    Left,

    /// <summary>The rectangle is aligned on the right side of the parent rectangle.</summary>
    Right,

    /// <summary>The rectangle is centered horizontally within the parent rectangle.</summary>
    Center,
}

/// <summary>
/// Specifies how a rectangle is vertically aligned relative to a parent rectangle.
/// </summary>
public enum VerticalAlignment {
    /// <summary>The rectangle is aligned at the top of the parent rectangle.</summary>
    Top,

    /// <summary>The rectangle is aligned at the bottom of the parent rectangle.</summary>
    Bottom,

    /// <summary>The rectangle is centered vertically within the parent rectangle.</summary>
    Center,
}

public class DrawingHelpers {
    /// <summary>
    /// Returns the midpoint coordinates of the provided rectangle.
    /// </summary>
    /// <param name="rectangle">The input rectangle.</param>
    /// <returns>A <see cref="Point"/> representing the midpoint of the rectangle.</returns>
    public static Point RectangleMidPoint(Rectangle rectangle) {
        var x = rectangle.X + rectangle.Width / 2;
        var y = rectangle.Y + rectangle.Height / 2;

        return new Point(x, y);
    }

    /// <summary>
    /// Creates a percentage-based relative rectangle with optional alignment.
    /// </summary>
    /// <param name="parent">The parent rectangle.</param>
    /// <param name="widthPercentage">Width percentage of the parent rectangle.</param>
    /// <param name="heightPercentage">Height percentage of the parent rectangle.</param>
    /// <param name="horizontalAlignment">Horizontal alignment of the rectangle. See <see cref="HorizontalAlignment"/></param>
    /// <param name="verticalAlignment">Vertical alignment of the rectangle. See <see cref="VerticalAlignment"/></param>
    public static Rectangle RelativePctRectangle(
        Rectangle           parent,
        double              widthPercentage     = 100.0,
        double              heightPercentage    = 100.0,
        HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left,
        VerticalAlignment   verticalAlignment   = VerticalAlignment.Top
    ) {
        var width  = (int)(widthPercentage * parent.Width / 100);
        var height = (int)(heightPercentage * parent.Height / 100);

        var x = horizontalAlignment switch {
                    HorizontalAlignment.Right  => parent.X + parent.Width - width,
                    HorizontalAlignment.Center => parent.X + (parent.Width - width) / 2,
                    _                          => parent.X
                };

        var y = verticalAlignment switch {
                    VerticalAlignment.Bottom => parent.Y + parent.Height - height,
                    VerticalAlignment.Center => parent.Y + (parent.Height - height) / 2,
                    _                        => parent.Y
                };

        return new Rectangle(x, y, width, height);
    }
}