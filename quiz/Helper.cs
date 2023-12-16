using Newtonsoft.Json;
using static quiz.FontSize;
using static quiz.Colourscheme.Colours;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using OneOf;
using System.Drawing.Drawing2D;

namespace quiz;

public enum FontSize
{
    HeadingOne,
    HeadingTwo,
    HeadingThree,
    HeadingFour,
    HeadingFive
}

internal static class Helper
{
    public static CanvasTextBox? CurrentTextBox;
    public static formMaster Form = new();
    /// <summary>
    /// Stopwatch stores time since last time the mouse was clicked.
    /// </summary>
    public static Stopwatch TimeSinceLastClick = new();

    public static (BrushTypes, BrushTypes?) DefaultBrush<T>(this T element) where T : ICanvasElement
    {
        if (element is CanvasButton)
        {
            return (
                new BrushTypes(Colourscheme.BrushDictionary[PALE_DOGWOOD], Colourscheme.BrushDictionary[KHAKI]),
                new BrushTypes(Colourscheme.BrushDictionary[RESEDA_GREEN], Colourscheme.BrushDictionary[WENGE])
                );
        }
        else if (element is CanvasText)
        {
            return (
                new BrushTypes(Colourscheme.BrushDictionary[WENGE], Colourscheme.BrushDictionary[VIOLET]),
                null
            );
        }

        return (
            new BrushTypes(Colourscheme.BrushDictionary[KHAKI], Colourscheme.BrushDictionary[RESEDA_GREEN]),
            null
        );
    }
    public static BrushTypes GetPopupColours()
    {
        return new(Colourscheme.BrushDictionary[KHAKI], Brushes.White);
    }
    public static BrushTypes GetButtonColours()
    {
        return new(Colourscheme.BrushDictionary[PALE_DOGWOOD], Colourscheme.BrushDictionary[KHAKI]);
    }
    public static BrushTypes GetTextColours()
    {
        return new(Colourscheme.BrushDictionary[RESEDA_GREEN], Colourscheme.BrushDictionary[RESEDA_GREEN]);
    }

    /// <summary>
    /// Dictionary of fonts where key is x, and font is size 10x
    /// </summary>
    public static Dictionary<FontSize, Font> Fonts = new()
    {
        { HeadingOne, new Font(FontFamily.GenericMonospace, 10) },
        { HeadingTwo, new Font(FontFamily.GenericMonospace, 20) },
        { HeadingThree, new Font(FontFamily.GenericMonospace, 30) },
        { HeadingFour, new Font(FontFamily.GenericMonospace, 40) },
        { HeadingFive, new Font(FontFamily.GenericMonospace, 50) },
    };
   
    /// <summary>
    /// Stores Mouse Location in a PointF
    /// </summary>
    public static PointF MouseLocation { get; set; }
    public static Size GetDefaultPopupSize()
    {
        return new Size(350, 350);
    }

    public static Size DefaultCanvasBoxSize()
    {
        return new Size(200, 100);
    }
    public static Size DefaultTextBoxSize() {
        return new Size(500, 100);
    }

    /// <summary>
    /// Checks if Mouse is hovering over rectangle
    /// </summary>
    /// <param name="rectangle">Rectangle to check mouse over</param>
    /// <returns>True if mouse intersects with rectangle</returns>
    public static bool GetMouseOver(this Rectangle rectangle)
    {
        if (new Rectangle(MouseLocation.Round(), new Size(5, 5)).IntersectsWith(rectangle))
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// Takes a ICanvasElement an returns a rectangle 
    /// </summary>
    /// <typeparam name="T">Generic Type constrained by ICanvasElement</typeparam>
    /// <param name="control">The control in question</param>
    /// <returns>A rectangle representing the element</returns>
    public static Rectangle GetRectangle<T>(this T control) where T : ICanvasElement
    {
        return new Rectangle(control.Location().Round(), control.Size());
    }
    /// <summary>
    /// Transforms a Point to be justified to the actual center of the specified size, rather than the TL
    /// </summary>
    /// <param name="func">The original function that represents the point to be transformed</param>
    /// <param name="size">The size to be transformed by</param>
    /// <returns>An updated function pointer</returns>
    public static Func<PointF> JustifyCenter(this Func<PointF> func, Size size)
    {
        return () => func().AccountForSize(size);
    }
    public static Func<PointF> JustifyCenter(this CanvasButton control)
    {
        return control.Location.JustifyCenter(control.Str.GetSize(control.Font));
    }
    /// <summary>
    /// Takes a string and returns the screen size
    /// </summary>
    /// <param name="str">The string to be measured</param>
    /// <param name="font">The font type</param>
    /// <returns>A size object that represents the screen size of the string</returns>
    public static Size GetSize(this string str, Font font)
    {
        return TextRenderer.MeasureText(str, font);
    }
    /// <summary>
    /// Rounds a PointF to a Point 
    /// </summary>
    /// <param name="pointf">The point to round</param>
    /// <returns>A Rounded point</returns>
    public static Point Round(this PointF pointf)
    {
        return Point.Round(pointf);
    }
    /// <summary>
    /// Gets screen size of string
    /// </summary>
    /// <param name="font">Font to use on string</param>
    /// <param name="str">String to measure</param>
    /// <returns>Size object</returns>
    public static Size GetTextSize(this Font font, string str)
    {
        return TextRenderer.MeasureText(str, font);
    }
    /// <summary>
    /// Returns the default size that a box should be
    /// </summary>
    /// <typeparam name="T">Generic constrained by a PictureBox</typeparam>
    /// <param name="control">Control to be extended onto</param>
    /// <returns>Size object</returns>
    public static Size GetDefaultBoxSize<T>(this T control) where T : PictureBox
    {
        return new Size(200, 100);
    }
    /// <summary>
    /// Takes PointF and a Size and justifies it to center of size rather than TL
    /// </summary>
    /// <param name="point">The position of the size</param>
    /// <param name="size">The size to be accounted for</param>
    /// <returns>A PointF representing the center of the size</returns>
    public static PointF AccountForSize(this PointF point, Size size)
    {
        return new PointF(point.X - (int)(size.Width * 0.5), point.Y - (int)(size.Height * 0.5));
    }
    /// <summary>
    /// Takes a rectangle and justifies it to the center, rather than TL
    /// </summary>
    /// <param name="rect">The rectangle to be justified</param>
    /// <returns>Justified PointF</returns>
    public static PointF AccountForSize(this Rectangle rect)
    {
        return new PointF(rect.X - (int)(0.5 * rect.Size.Width), rect.Y - (int)(0.5 * rect.Size.Height));
    }
    /// <summary>
    /// Generic version of AccountForSize()
    /// </summary>
    /// <typeparam name="T">Generic constraint of ICanvasElement</typeparam>
    /// <param name="control">Control to account for size of</param>
    /// <returns></returns>
    public static PointF AccountForSize<T>(this T control) where T : ICanvasElement
    {
        var loc = control.Location();
        return new PointF(loc.X - (int)(0.5 * control.Size().Width), loc.Y - (int)(0.5 * control.Size().Height));
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
    /// <summary>
    /// Gets the center of a control, offset by dx and dy
    /// </summary>
    /// <typeparam name="T">Generic constrained of Control</typeparam>
    /// <param name="control">The control to be centered</param>
    /// <param name="dx">Offset by X</param>
    /// <param name="dy">Offset by Y</param>
    /// <returns>Changed PointF</returns>
    public static PointF GetCenter<T>(this T control, float dx, float dy) where T : Control, ICanvasElement
    {
        if(typeof(T) is ICanvasElement)
        {
            var size = control.Size();
            return new PointF((size.Width / 2) + dx, (size.Height / 2) + dy);
        }
        else return new PointF((control.Width / 2) + dx, (control.Height / 2) + dy);
    }

    /// <summary>
    /// Non generic version of GetCenter
    /// </summary>
    /// <param name="dx">Horizontal offset from the center</param>
    /// <param name="dy">Vertical offset from the center</param>
    /// <returns>PointF of center offset from parameters</returns>
    public static PointF GetCenter(float dx, float dy)
    {
        return new PointF((View.Current.Canvas.Width / 2) + dx, (View.Current.Canvas.Height / 2) + dy);
    }

    public static GraphicsPath GetRoundedCorners(PointF point, Size size)
    {
        int radius = 20;
        var path = new GraphicsPath();
        path.AddArc(point.X, point.Y, radius, radius, 180, 90);
        path.AddArc(point.X + size.Width - radius, point.Y, radius, radius, 270, 90);
        path.AddArc(point.X + size.Width - radius, point.Y + size.Height - radius, radius, radius, 0, 90);
        path.AddArc(point.X, point.Y + size.Height - radius, radius, radius, 90, 90);
        path.CloseFigure();
        return path;
    }



    ///// USER METHODS

    public static List<User> Users { get; set; } = new();
    // public static User LoggedIn { get; set; } = new(null, null);

    /// <summary>
    /// Hashes a password with SHA256 
    /// </summary>
    /// <param name="password">The password to be hashed</param>
    /// <returns>A hashed string</returns>
    public static void Hash(this string password, out string hashedPassword)
    {
        using(SHA256 s = SHA256.Create())
        {
            byte[] hashed = s.ComputeHash(Encoding.UTF8.GetBytes(password));
            hashedPassword = BitConverter.ToString(hashed).Replace("-", "").ToLower();
        }
    }

    static string p = "../../../Users.json";
    
    public static void SerializeUsers(this IEnumerable<User> users)
    {
        string json = JsonConvert.SerializeObject(users);
        using(StreamWriter sw = new(p, false))
        {
            sw.WriteLine(json);
        }
    }
    public static List<User> DeserializeUsers()
    {
        string json = File.ReadAllLines(p)[0];
        return JsonConvert.DeserializeObject<List<User>>(json)!;
    }

    public static bool TryLogin(this User user)
    {
        foreach(var u in Users)
        {
            if(u.Password == user.Password && u.Username == u.Username)
            {
                return true;
            }
        }
        return false;
    }
}
public enum TextBoxType
{
    None,
    Username,
    Password
}
