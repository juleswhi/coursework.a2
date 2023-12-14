namespace quiz;

internal class View
{
    public static View Current { get; set; } = new(new(), new());
    public PictureBox Canvas { get; set; }
    private List<ICanvasElement> _elements;

    // ~60 fps 
    System.Timers.Timer timer = new(16);
    public bool Stopped { get; set; } = false;

    public View(List<ICanvasElement> elements, PictureBox canvas)
    {
        Canvas = canvas;
        _elements = elements;
    }

    public void Stop()
    {
        Canvas.Paint -= Paint!;
        timer.Stop();
        Stopped = true;
    }

    public void Run()
    {
        Current.Stop();

        System.Timers.Timer timer = new(16);

        timer.Elapsed += (s, e) =>
        {
            if (Stopped)
            {
                timer.Stop();
                return;
            }
            if (Helper.TimeSinceLastClick.ElapsedMilliseconds <= 200) return;
            if (Control.MouseButtons != MouseButtons.Left) return;

            Helper.TimeSinceLastClick.Restart();

            foreach (var element in _elements.Where(x => x.Selected))
            {
                element.OnClick();
            }
        };

        timer.Start();
        Canvas.Show();
        Current = this;
        Canvas.BorderStyle = BorderStyle.None;
        Canvas.BackColor = Color.FromArgb(113, 91, 100);

        Canvas.Anchor = AnchorStyles.None;
        Canvas.Dock = DockStyle.Fill;
        Canvas.BringToFront();

        Canvas.Paint += Paint!;

        timer.Elapsed += (s, e) =>
        {
            if (Stopped)
            {
                timer.Stop();
                return;
            }
            Helper.Form.Invoke(Canvas.Refresh);
        };

        timer.Start();
    }


    private void Paint(object sender, PaintEventArgs e)
    {
        e.Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
        e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
        e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
        e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;

        foreach (var element in _elements)
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
    }
}


class QuestionView : View
{
    public QuestionView(List<ICanvasElement> elements, PictureBox canvas) : base(elements, canvas)
    {

    }
}
