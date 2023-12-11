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
        _elements.Add(new CanvasText("Main Menu", _font, Brushes.Pink, () => _c.GetCenter(0, _c.Top - 100)));
        _elements.Add(new CanvasButton("Button", _font, null, Brushes.Pink, _c.GetDefaultBoxSize(), () => _c.GetCenter()));

        // 60~ fps
        System.Timers.Timer t = new(16);
        t.Start();
        t.Elapsed += (s, e) => _c.Refresh();

    }
    private void _c_Paint(object? sender, PaintEventArgs e)
    {
        foreach(var element in _elements)
        {
            element.PreRender(e.Graphics);
        }
        foreach(var element in _elements)
        {
            element.Render(e.Graphics);
        }
        foreach(var element in _elements)
        {
            element.PostRender(e.Graphics);
        }
    }
}
