using System.Reflection.Metadata.Ecma335;

namespace quiz;

internal static class Helper
{
    public static Rectangle GetRectangle<T>(this T control) where T : ICanvasElement
    {
        return new Rectangle(control.Location().Round(), control.Size);
    }
    public static Func<PointF> JustifyCenter(this Func<PointF> func, Size size) {
        return () => func().AccountForSize(size);
    }
    public static Func<PointF> JustifyCenter(this CanvasButton control)
    {
        return control.Location.JustifyCenter(control.Str.GetSize(control.Font));
    }
    public static Size GetSize(this string str, Font font)
    {
        return TextRenderer.MeasureText(str, font);
    }
    public static Point Round(this PointF point)
    {
        return Point.Round(point);
    }
    public static Brush GetDefaultBrush<T>(this T control) where T : CanvasBox
    {
        return Brushes.Gray;
    }
    public static Size GetTextSize(this Font font, string str) 
    {
        return TextRenderer.MeasureText(str, font);
    }
    public static Size GetDefaultBoxSize<T>(this T control) where T : PictureBox
    {
        return new Size(250, 100);
    }
    public static PointF AccountForSize(this PointF point, Size size) 
    {
        return new PointF(point.X - (int)(size.Width * 0.5), point.Y - (int)(size.Height * 0.5));
    }
    public static PointF AccountForTextSize(this PointF point, string str, Font font)
    {
        Size s = str.GetSize(font);
        return new PointF(point.X + (int)(s.Width * 0.5), point.Y + (int)(s.Height * 0.5));
    }
    public static PointF AccountForSize<T>(this T control) where T : ICanvasElement
    {
        var loc = control.Location();
        return new PointF(loc.X - (int)(0.5 * control.Size.Width), loc.Y - (int)(0.5 * control.Size.Height));
    }
    public static PointF AccountForSize<T>(this T control, string str) where T : ICanvasText
    {
        var size = control.Font.GetTextSize(str);
        var loc = control.Location();
        return new PointF(loc.X - (int)(0.5 * size.Width), loc.Y - (int)(0.5 * size.Width));
    } 
    public static PointF AccountForSize(this Rectangle rect)
    {
        return new PointF(rect.X - (int)(0.5 * rect.Size.Width), rect.Y - (int)(0.5 * rect.Size.Height));
    }

    public static PointF GetCenterElement<T>(this T control) where T : ICanvasElement
    {
        var loc = control.Location();
        return new PointF(loc.X / 2, loc.Y / 2);
    }
    public static PointF GetCenterControl<T>(this T control) where T : Control
    {
        return new PointF(control.Width / 2, control.Height / 2);
    }
    public static PointF GetCenterRectangle(this Rectangle control)
    {
        return new PointF(control.Width / 2, control.Height / 2);
    }
    public static PointF GetCenter<T>(this T control, float dx, float dy) where T : Control
    {
        return new PointF((control.Width / 2) + dx, (control.Height / 2) + dy);
    }


}
