namespace quiz;

enum JustType
{
    Center
}

interface ICanvasElement
{
    public Brush Brush { get; set; }
    public Func<PointF> Location { get; set; }
    public Size Size { get; set; }
    // This will render before everything else, ie: Button Backgrounds
    public void PreRender(Graphics g);
    // This will render after Pre, ie: Text
    public void Render(Graphics g);
    // This will render last, ie: Post
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
    public Font Font { get; set; }
    public Brush Brush { get; set; }
    public string Str { get; set; }
    public Func<PointF> Location { get; set; }
    public Size Size { get; set; }

    public CanvasText(string str, Font font, Brush brush, Func<PointF> loc)
    {
        Font = font;
        Brush = brush;
        Str = str;
        Location = loc;
        Size = Font.GetTextSize(Str);
    }

    public void PreRender(Graphics g)
    {}
    public void Render(Graphics g)
    {
        // Dynamically grab the location
        g.DrawString(Str, Font, Brush, this.AccountForSize());
    }
    public void PostRender(Graphics g)
    {}
}

class CanvasBox : ICanvasElement
{
    public Brush Brush { get; set; }
    public Func<PointF> Location { get; set; }
    public Size Size { get; set; }

    public CanvasBox(Brush? brush, Size size, Func<PointF> loc)
    {
        Brush = Brush == null ? this.GetDefaultBrush() : brush!;
        Size = size;
        Location = loc.JustifyCenter(Size);
    }

    public void PreRender(Graphics g)
    {
        g.FillRectangle(Brush, new(Location().Round(), Size));
    }

    public virtual void Render(Graphics g)
    {}

    public void PostRender(Graphics g)
    {}
}

sealed class CanvasButton : CanvasBox, ICanvasText
{
    public string Str { get; set; }
    public PointF StrLocation { get; set; }
    public Brush TextBrush { get; set; }
    public Font Font { get; set; }
    public CanvasButton(string str, Font font, Brush? backBrush, Brush textBrush, Size size, Func<PointF> loc) : base(backBrush, size, loc)
    {
        Str = str;
        Font = font;
        TextBrush = textBrush;
        Size textSize = Str.GetSize(Font);
        StrLocation = loc.JustifyCenter(Size)().AccountForTextSize(Str, Font);
    }

    public override void Render(Graphics g)
    {
        g.DrawString(Str, Font, TextBrush, StrLocation);
    }
}
