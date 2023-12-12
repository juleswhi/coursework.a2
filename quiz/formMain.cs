namespace quiz;

public partial class formMain : Form
{
    private PictureBox _c = new();
    private static Font _font = new(FontFamily.GenericMonospace, 20);

    private List<ICanvasElement> _elements = new();

    public formMain()
    {
        // Create the canvas programmatically
        InitializeComponent();
        Controls.Add(_c);
        _c.Paint += _c_Paint;
        _c.BorderStyle = BorderStyle.None;
        _c.BackColor = Color.FromArgb(113, 91, 100);

        // Fill to screen and Dynamically update
        _c.Anchor = AnchorStyles.None;
        _c.Dock = DockStyle.Fill;

        // Add the banner
        _elements.Add(new CanvasText("Main Menu", _font, new BrushTypes(Brushes.Pink, Brushes.LightPink), () => _c.GetCenter(0, _c.Top - 100)));
        _elements.Add(new CanvasButton("EXE", _font, new BrushTypes(Brushes.Gray, Brushes.LightGray), new BrushTypes(Brushes.Black, Brushes.DarkGray), _c.GetDefaultBoxSize(), () => _c.GetCenterControl()));

        // 60~ fps
        System.Timers.Timer t = new(16);
        t.Start();
        t.Elapsed += (s, e) =>
        {
            Helper.MouseLocation = PointToClient(MousePosition);
            _c.Refresh();
        };

    }
    
    private void _c_Paint(object? sender, PaintEventArgs e)
    {
        e.Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
        e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
        e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
        e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;

        foreach(var element in _elements)
        {
            if (element.GetRectangle().GetMouseOver())
            {
                element.Selected = true;
            }
            else
            {
                element.Selected = false;
            }

            element.PreRender(e.Graphics);
        }
        foreach (var element in _elements)
        {
            element.Render(e.Graphics);
        }
        foreach (var element in _elements)
        {
            element.PostRender(e.Graphics);
        }
        // e.Graphics.DrawLine(Pens.DarkGreen, new(_c.Width / 2, 0), new(_c.Width / 2, _c.Height));
    }


}
