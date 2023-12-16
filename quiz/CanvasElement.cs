using static quiz.Helper;
using static quiz.FontSize;
namespace quiz;
public record BrushTypes(Brush Default, Brush Selected);
public interface ICanvasElement
{
    /// <summary>
    /// Is true if mouse over
    /// </summary>
    public bool Selected { get; set; }
    /// <summary>
    /// If the element is functional or not
    /// </summary>
    public bool Enabled { get; set; }
    /// <summary>
    /// Both a brush for selected and non-selected
    /// </summary>
    public BrushTypes Brush { get; set; }
    /// <summary>
    /// Custom behaviour for click. Commonly used to open new View
    /// </summary>
    public Action OnClick { get; set; }
    /// <summary>
    /// Returns a function pointer so it dynamically updates location
    /// This is evaluated every render cycle
    /// </summary>
    public Func<PointF> Location { get; set; }
    /// <summary>
    /// Max Size of the element
    /// </summary>
    public Func<Size> Size { get; set; }
    /// <summary>
    /// First Render Cycle. Used for base elements such as Boxes
    /// </summary>
    /// <param name="g">Graphics object of <c>PictureBox</c></param>
    public void PreRender(Graphics g);
    /// <summary>
    /// Second Render Cycle. Used for secondary elements like Text
    /// </summary>
    /// <param name="g">Graphics object of <c>PictureBox</c></param>
    public void Render(Graphics g);
    /// <summary>
    /// Last Render Cycle. Used for Post
    /// </summary>
    /// <param name="g"></param>
    public void PostRender(Graphics g);
}

public interface ICanvasText
{
    public Font Font { get; set; }
    public string Str { get; set; }
    public Func<PointF> Location { get; set; }
}

public class CanvasText : ICanvasElement, ICanvasText
{
    /// <summary>
    /// Font of the displayed text
    /// </summary>
    public Font Font { get; set; }
    /// <summary>
    /// Brush to paint the element
    /// </summary>
    public BrushTypes Brush { get; set; }
    /// <summary>
    /// Actual string to display
    /// </summary>
    public string Str { get; set; }
    /// <summary>
    /// Action to call when clicked 
    /// </summary>
    public Action OnClick { get; set; }
    /// <summary>
    /// Func pointer to dynamically update the location
    /// </summary>
    public Func<PointF> Location { get; set; }
    /// <summary>
    /// Size of the Text
    /// </summary>
    public Func<Size> Size { get; set; }
    /// <summary>
    /// Checks if the element is selected / hovered over
    /// </summary>
    public bool Selected { get; set; } = false;
    public bool Enabled { get; set; }

    /// <summary>
    /// Constructor for Text Element
    /// </summary>
    /// <param name="str">String to display</param>
    /// <param name="font">Font of string</param>
    /// <param name="brush">Colour to draw text</param>
    /// <param name="loc">Func pointer for location</param>
    public CanvasText(string str, Font? font, Func<PointF> loc)
    {
        if (font is null)
        {
            Font = Fonts[HeadingThree];
        }
        else
        {
            Font = font;
        }
        Brush = GetTextColours();
        Str = str;
        Location = loc;
        Size = () => Font.GetTextSize(Str);
        Enabled = true;
        OnClick += () => { if (!Selected || !Enabled) return; };
    }

    public void PreRender(Graphics g)
    { }
    public void Render(Graphics g)
    {
        // Dynamically grab the location
        g.DrawString(Str, Font, Brush.Selected, this.AccountForSize());
    }
    public void PostRender(Graphics g)
    { }

}

public class CanvasBox : ICanvasElement
{
    public BrushTypes Brush { get; set; }
    public virtual Action OnClick { get; set; }
    public Func<PointF> Location { get; set; }
    public Func<Size> Size { get; set; }
    public bool Selected { get; set; }
    public bool Enabled { get; set; }

    public CanvasBox(Func<Size>? size, Func<PointF> loc)
    {
        Brush = GetButtonColours();
        Size = size == null ? DefaultCanvasBoxSize : size;
        Location = loc.JustifyCenter(Size());
        Enabled = true;
        OnClick += () => { if (!Selected || !Enabled) return; };
    }

    public virtual void PreRender(Graphics g)
    {
        g.FillPath(Selected ? Brush.Selected : Brush.Default, GetRoundedCorners(Location(), Size()));
    }

    public virtual void Render(Graphics g)
    { }

    public virtual void PostRender(Graphics g)
    { }

}

public sealed class CanvasButton : CanvasBox, ICanvasText
{
    public string Str { get; set; }
    public Func<PointF> StrLocation { get; set; }
    public override Action OnClick { get; set; }
    public BrushTypes TextBrush { get; set; }
    public Font Font { get; set; }
    public CanvasButton(string str, Font? font, Func<Size>? size, Func<PointF> loc, Action onClick) : base(size, loc)
    {
        Str = str;
        Font = font == null ? Fonts[HeadingTwo] : font;
        TextBrush = GetTextColours();
        Size textSize = Str.GetSize(Font);
        StrLocation = loc.JustifyCenter(Size());
        OnClick += () => { if (!Selected || !Enabled) return; };
        OnClick += onClick;
    }

    public override void Render(Graphics g)
    {
        g.DrawString(Str, Font, Selected ? TextBrush.Selected : TextBrush.Default, new PointF(StrLocation().X + (int)(0.5 * Size().Width) - (int)(0.5 * Str.GetSize(Font).Width), StrLocation().Y + (int)(0.5 * Size().Height) - (int)(0.5 * Str.GetSize(Font).Height)));
    }

}

class CanvasTextBox : CanvasBox, ICanvasElement
{
    public TextBoxType Tag { get; set; }
    public static bool ShiftPressed { get; set; } = false;
    public static bool CapsToggled { get; set; } = false;
    public Font Font { get; set; }
    public List<char> Text { get; set; } = new();
    public Func<PointF> TextLocation { get; set; }
    public BrushTypes TextBrush { get; set; }
    public override Action OnClick { get; set; }
    public void KeyPress(Keys key)
    {
        switch (key)
        {
            case Keys.Space:
                Text.Add(' ');
                break;
            case Keys.Back:
                if (Text.Count > 0)
                    Text.RemoveAt(Text.Count - 1);
                break;
            case Keys.Enter:
                Text.Add('\n');
                break;
            case Keys.LShiftKey:
            case Keys.ShiftKey:
            case Keys.Shift:
                ShiftPressed = true;
                break;
            case Keys.CapsLock:
                CapsToggled = !CapsToggled;
                break;
            case Keys.Control:
                break;
            default:
                if (ShiftPressed || CapsToggled)
                {
                    Text.Add(key.ToString()[0]);
                }
                else
                {
                    Text.Add(key.ToString().ToLower()[0]);
                }
                break;
        }
    }

    public CanvasTextBox(Font? font, Func<Size>? size, TextBoxType tag, Func<PointF> loc, Action onClick) : base(size, loc)
    {
        Tag = tag;
        Font = font == null ? Fonts[HeadingTwo] : font;
        Size = size == null ? DefaultTextBoxSize : size;
        TextBrush = GetTextColours();
        Location = loc.JustifyCenter(Size());
        TextLocation = loc.JustifyCenter(Size());
        CurrentTextBox = this;
        OnClick += () =>
        {
            if (!Selected || !Enabled) return;
            CurrentTextBox = this;
        };
        OnClick += onClick;
    }

    int verticalOffset = 0;

    public override void Render(Graphics g)
    {
        List<string> strs = GetString(Text);
        foreach(var str in strs)
        {
            g.DrawString(str, Font, Selected ? TextBrush.Selected : TextBrush.Default, new PointF(TextLocation().X, TextLocation().Y + verticalOffset));
            verticalOffset += str.GetSize(Font).Height;
        }
        verticalOffset = 0;
    }
    private List<string> GetString(List<char> chars)
    {
        string longString = string.Join("", chars);
        int i1 = 0;
        List<string> strs = new();
        for(int i = 0; i < chars.Count; i++)
        {
            int i2 = i;
            if (chars[i] != '\n' && inSize(longString.Substring(i1, i2 - i1)))
            {
                continue;
            }
            strs.Add(longString.Substring(i1, i2 - i1 - 1));
            i1 = i;
        }
        strs.Add(longString.Substring(i1));
        return strs;
    }
    private bool inSize(string str)
    {
        if(str.GetSize(Font).Width < Size().Width)
        {
            return true;
        }
        return false;
    }
}

public class CanvasPopup : ICanvasElement, ICanvasText
{
    public bool Selected { get; set; }
    public bool Enabled { get; set; }
    public Action OnClick { get; set; }
    public Func<PointF> Location { get; set; }
    public Func<Size> Size { get; set; }
    public BrushTypes Brush { get; set; }
    public Font Font { get; set; }
    public string Str { get; set; }
    public CanvasButton OkButton { get; set; }

    public CanvasPopup(string message, Font? font, Func<Size>? size, Func<PointF> location, Action onClick)
    {
        Brush = GetPopupColours();
        Str = message;
        Font = font == null ? Fonts[HeadingTwo] : font;
        Size = size == null ? GetDefaultPopupSize : size;
        Location = location.JustifyCenter(Size());
        // OnClick = () => { if (!Selected) return; };
        OnClick = onClick;

        OkButton = new CanvasButton("Ok", font, () => new Size(50, 20), () => GetCenter(0,125), onClick);
    }


    public void PreRender(Graphics g)
    {}

    public void Render(Graphics g)
    {}

    public void PostRender(Graphics g)
    {
        g.FillPath(Brush.Default, GetRoundedCorners(Location(), Size()));
        OkButton.PreRender(g);
        OkButton.Render(g);
        OkButton.PostRender(g);
    }
}
