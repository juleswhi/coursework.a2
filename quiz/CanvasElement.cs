using System.Drawing.Drawing2D;
using static quiz.FontSize;
namespace quiz;
record BrushTypes(Brush Default, Brush Selected);
interface ICanvasElement
{
    public bool Selected { get; set; }
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

interface ICanvasText
{
    public Font Font { get; set; }
    public string Str { get; set; }
    public Func<PointF> Location { get; set; }
}

class CanvasText : ICanvasElement, ICanvasText
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

    /// <summary>
    /// Constructor for Text Element
    /// </summary>
    /// <param name="str">String to display</param>
    /// <param name="font">Font of string</param>
    /// <param name="brush">Colour to draw text</param>
    /// <param name="loc">Func pointer for location</param>
    public CanvasText(string str, Font? font, Func<PointF> loc)
    {
        if(font is null)
        {
            Font = Helper.Fonts[HeadingThree];
        }
        else
        {
            Font = font;
        }
        Brush = Helper.GetTextColours();
        Str = str;
        Location = loc;
        Size = () => Font.GetTextSize(Str);
        OnClick += () => { if (!Selected) return; };
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

class CanvasBox : ICanvasElement
{
    public BrushTypes Brush { get; set; }
    public virtual Action OnClick { get; set; }
    public Func<PointF> Location { get; set; }
    public Func<Size> Size { get; set; }
    public bool Selected { get; set; }

    public CanvasBox(Func<Size>? size, Func<PointF> loc)
    {
        Brush = Helper.GetButtonColours();
        Size = size == null ? Helper.DefaultCanvasBoxSize : size;
        Location = loc.JustifyCenter(Size());
        OnClick += () => { if (!Selected) return; };
    }

    public virtual void PreRender(Graphics g)
    {
        int radius = 20;
        float x = Location().X;
        float y = Location().Y;
        float width = Size().Width;
        float height = Size().Height;
        var path = new GraphicsPath();
        path.AddArc(x, y, radius, radius, 180, 90);
        path.AddArc(x + width - radius, y, radius, radius, 270, 90);
        path.AddArc(x + width - radius, y + height - radius, radius, radius, 0, 90);
        path.AddArc(x, y + height - radius, radius, radius, 90, 90);
        path.CloseFigure();
        g.FillPath(Selected ? Brush.Selected : Brush.Default, path);
    }

    public virtual void Render(Graphics g)
    { }

    public virtual void PostRender(Graphics g)
    { }

}

sealed class CanvasButton : CanvasBox, ICanvasText
{
    public string Str { get; set; }
    public Func<PointF> StrLocation { get; set; }
    public override Action OnClick { get; set; }
    public BrushTypes TextBrush { get; set; }
    public Font Font { get; set; }
    public CanvasButton(string str, Font? font, Func<Size>? size, Func<PointF> loc, Action onClick) : base(size, loc)
    {

        Str = str;
        Font = font == null ? Helper.Fonts[HeadingTwo] : font;
        TextBrush = Helper.GetTextColours();
        Size textSize = Str.GetSize(Font);
        StrLocation = loc.JustifyCenter(Size());
        OnClick += () => { if (!Selected) return; };
        OnClick += onClick;
    }

    public override void Render(Graphics g)
    {
        g.DrawString(Str, Font, Selected ? TextBrush.Selected : TextBrush.Default, new PointF(StrLocation().X + (int)(0.5 * Size().Width) - (int)(0.5 * Str.GetSize(Font).Width), StrLocation().Y + (int)(0.5 * Size().Height) - (int)(0.5 * Str.GetSize(Font).Height)));
    }

}

class CanvasTextBox : CanvasBox, ICanvasElement
{
    public Font Font { get; set; }
    public string Text { get; set; } = String.Empty;
    public Func<PointF> TextLocation { get; set; }
    public BrushTypes TextBrush { get; set; }
    public override Action OnClick { get; set; }

    public CanvasTextBox(Font? font, Func<Size>? size, Func<PointF> loc, Action onClick) : base(size, loc)
    {
        Font = font == null ? Helper.Fonts[HeadingTwo] : font;
        TextBrush = Helper.GetTextColours();
        TextLocation = loc.JustifyCenter(Size());
        OnClick += () => { if (!Selected) return; };
        OnClick += onClick;
    }

    public override void Render(Graphics g)
    {
        g.DrawString(Text, Font, Selected ? TextBrush.Selected : TextBrush.Default, new PointF(TextLocation().X + (int)(0.5 * Size().Width) - (int)(0.5 * Text.GetSize(Font).Width), TextLocation().Y + (int)(0.5 * Size().Height) - (int)(0.5 * Text.GetSize(Font).Height)));
    }

}
