using System.Runtime.InteropServices;

namespace quiz
{
    internal static class Helper
    {
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
        public static PointF GetCenter<T>(this T control) where T : Control
        {
            return new PointF(control.Width / 2, control.Height / 2);
        }
        public static PointF GetCenter<T>(this T control, float dx, float dy) where T : Control
        {
            return new PointF((control.Width / 2) + dx, (control.Height / 2) + dy);
        }


    }
}
