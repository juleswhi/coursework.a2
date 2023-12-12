using System.Reflection.Metadata.Ecma335;

namespace quiz;

internal static class Helper
{
    public static Dictionary<int, Font> Fonts = new()
    {
        { 1, new Font(FontFamily.GenericMonospace, 10) },
        { 2, new Font(FontFamily.GenericMonospace, 20) },
        { 3, new Font(FontFamily.GenericMonospace, 30) },
        { 4, new Font(FontFamily.GenericMonospace, 40) },
        { 5, new Font(FontFamily.GenericMonospace, 50) },
    };
    public static Dictionary<int, BrushTypes> Colourscheme = new()
    {
        { 1, new BrushTypes(new SolidBrush(Color.FromArgb(211,189,176)), new SolidBrush(Color.FromArgb(193,174,159))) },
        { 2, new BrushTypes(new SolidBrush(Color.FromArgb(193,174,159)), new SolidBrush(Color.FromArgb(137,147,124))) },
        { 3, new BrushTypes(new SolidBrush(Color.FromArgb(137,147,124)), new SolidBrush(Color.FromArgb(113,91,100))) },
        { 4, new BrushTypes(new SolidBrush(Color.FromArgb(113,91,100)), new SolidBrush(Color.FromArgb(105,56,92))) },
    };


    public static PointF MouseLocation { get; set; } 

    public static bool GetMouseOver(this Rectangle rectangle)
    {
        if (new Rectangle(MouseLocation.Round(), new Size(5, 5)).IntersectsWith(rectangle))
        {
            return true;
        }
        return false;
    }
    public static Rectangle GetRectangle<T>(this T control) where T : ICanvasElement
    {
        return new Rectangle(control.Location(View.Current).Round(), control.Size);
    }
    public static Func<PictureBox, PointF> JustifyCenter(this Func<PictureBox, PointF> func, Size size) {
        return (c) => func(c).AccountForSize(size);
    }
    public static Func<PictureBox, PointF> JustifyCenter(this CanvasButton control)
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
        var loc = control.Location(View.Current);
        return new PointF(loc.X - (int)(0.5 * control.Size.Width), loc.Y - (int)(0.5 * control.Size.Height));
    }
    public static PointF AccountForSize<T>(this T control, string str) where T : ICanvasText
    {
        var size = control.Font.GetTextSize(str);
        var loc = control.Location(View.Current);
        return new PointF(loc.X - (int)(0.5 * size.Width), loc.Y - (int)(0.5 * size.Width));
    } 
    public static PointF AccountForSize(this Rectangle rect)
    {
        return new PointF(rect.X - (int)(0.5 * rect.Size.Width), rect.Y - (int)(0.5 * rect.Size.Height));
    }

    public static PointF GetCenterElement<T>(this T control) where T : ICanvasElement
    {
        var loc = control.Location(View.Current);
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
