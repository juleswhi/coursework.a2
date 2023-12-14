namespace quiz;
record BrushTypes(Brush Default, Brush Selected);
interface ICanvasElement
{
    public bool Selected { get; set; }
    public BrushTypes Brush { get; set; }
    /// <summary>
    /// Called when any mouse click is registered.
    /// </summary>
    public void Click();
    /// <summary>
    /// Custom behaviour for click. Commonly used to open new View
    /// </summary>
    public Action OnClick { get; set; }
    /// <summary>
    /// Returns a function pointer so it dynamically updates location
    /// This is evaluated every render cycle
    /// </summary>
    public Func<PictureBox, PointF> Location { get; set; }
    /// <summary>
    /// Max Size of the element
    /// </summary>
    public Size Size { get; set; }
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
    public Func<PictureBox, PointF> Location { get; set; }
}

class CanvasText : ICanvasElement, ICanvasText 
{
    public Font Font { get; set; }
    public BrushTypes Brush { get; set; }
    public string Str { get; set; }
    public Action OnClick { get; set; }
    public Func<PictureBox, PointF> Location { get; set; }
    public Size Size { get; set; }
    public bool Selected { get; set; } = false;

    public CanvasText(string str, Font font, BrushTypes brush, Func<PictureBox, PointF> loc)
    {
        Font = font;
        Brush = brush;
        Str = str;
        Location = loc;
        Size = Font.GetTextSize(Str);
        OnClick = () => { };
    }

    public void PreRender(Graphics g)
    {}
    public void Render(Graphics g)
    {
        // Dynamically grab the location
        g.DrawString(Str, Font, Brush.Selected, this.AccountForSize());
    }
    public void PostRender(Graphics g)
    {}

    public void Click() {}
}

class CanvasBox : ICanvasElement
{
    public BrushTypes Brush { get; set; }
    public virtual Action OnClick { get; set; }
    public Func<PictureBox, PointF> Location { get; set; }
    public Size Size { get; set; }

    public bool Selected { get; set; }

    public CanvasBox(BrushTypes brush, Size size, Func<PictureBox, PointF> loc)
    {
        Brush = brush;
        Size = size;
        Location = loc.JustifyCenter(Size);
        OnClick = () => { };
    }

    public void PreRender(Graphics g)
    {
        g.FillRectangle(Selected ? Brush.Selected : Brush.Default, new(Location(View.Current.Canvas).Round(), Size));
    }

    public virtual void Render(Graphics g)
    {}

    public void PostRender(Graphics g)
    {}

    public virtual void Click()
    {
        if (!Selected) return;
        OnClick();
    }
}

sealed class CanvasButton : CanvasBox, ICanvasText
{
    public string Str { get; set; }
    public Func<PictureBox, PointF> StrLocation { get; set; }
    public override Action OnClick { get; set; }
    public BrushTypes TextBrush { get; set; }
    public Font Font { get; set; }
    public CanvasButton(string str, Font font, BrushTypes backBrush, BrushTypes textBrush, Func<PictureBox, Size> size, Func<PictureBox, PointF> loc, Action onClick) : base(backBrush, size(View.Current.Canvas), loc)
    {
        Str = str;
        Font = font;
        TextBrush = textBrush;
        Size textSize = Str.GetSize(Font);
        StrLocation = loc.JustifyCenter(Size);
        OnClick = onClick;
    }

    public override void Render(Graphics g)
    {
        g.DrawString(Str, Font, Selected ? TextBrush.Selected : TextBrush.Default, new PointF(StrLocation(View.Current.Canvas).X + (int)(0.5*Size.Width) - (int)(0.5*Str.GetSize(Font).Width), StrLocation(View.Current.Canvas).Y + (int)(0.5*Size.Height) - (int)(0.5*Str.GetSize(Font).Height)));
    }
    public override void Click()
    {
        if (!Selected) return;

        OnClick();
    }
}
