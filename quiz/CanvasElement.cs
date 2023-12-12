namespace quiz;
record BrushTypes(Brush Default, Brush Selected);
interface ICanvasElement
{
    public bool Selected { get; set; }
    public BrushTypes Brush { get; set; }
    public Func<PictureBox, PointF> Location { get; set; }
    public Size Size { get; set; }
    // This will render before everything else, ie: Button Backgrounds
    public void PreRender(Graphics g);
    // This will render after Pre, ie: Text
    public void Render(Graphics g);
    // This will render last, ie: Post
    public void PostRender(Graphics g);
    public void Click();
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
    public Func<PictureBox, PointF> Location { get; set; }
    public Size Size { get; set; }

    public bool Selected { get; set; }

    public CanvasBox(BrushTypes brush, Size size, Func<PictureBox, PointF> loc)
    {
        Brush = brush;
        Size = size;
        Location = loc.JustifyCenter(Size);
    }

    public void PreRender(Graphics g)
    {
        g.FillRectangle(Selected ? Brush.Selected : Brush.Default, new(Location(View.Current).Round(), Size));
    }

    public virtual void Render(Graphics g)
    {}

    public void PostRender(Graphics g)
    {}

    public virtual void Click()
    {
        if (!Selected) return;


    }
}

sealed class CanvasButton : CanvasBox, ICanvasText
{
    public string Str { get; set; }
    public Func<PictureBox, PointF> StrLocation { get; set; }
    public BrushTypes TextBrush { get; set; }
    public Font Font { get; set; }
    public CanvasButton(string str, Font font, BrushTypes backBrush, BrushTypes textBrush, Func<PictureBox, Size> size, Func<PictureBox, PointF> loc) : base(backBrush, size(View.Current), loc)
    {
        Str = str;
        Font = font;
        TextBrush = textBrush;
        Size textSize = Str.GetSize(Font);
        StrLocation = loc.JustifyCenter(Size);
    }

    public override void Render(Graphics g)
    {
        g.DrawString(Str, Font, Selected ? TextBrush.Selected : TextBrush.Default, new PointF(StrLocation(View.Current).X + (int)(0.5*Size.Width) - (int)(0.5*Str.GetSize(Font).Width), StrLocation(View.Current).Y + (int)(0.5*Size.Height) - (int)(0.5*Str.GetSize(Font).Height)));
    }
    public override void Click()
    {
        if (!Selected) return;

    }
}
