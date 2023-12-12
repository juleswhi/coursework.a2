namespace quiz;

internal class View 
{
    public static PictureBox Current { get; set; } = new();
    private PictureBox canvas;
    private List<ICanvasElement> _elements;

    public View(Form form, List<ICanvasElement> elements)
    {
        canvas = new();
        _elements = elements;

        System.Timers.Timer timer = new(16);

        timer.Elapsed += (s, e) =>
        {
            if (Control.MouseButtons == MouseButtons.Left)
            {
                foreach(var element in _elements)
                {
                    element.Click();
                }
            }
        };

        timer.Start();
    }

    public PictureBox Run()
    {
        canvas.Show();
        Current = canvas;
        canvas.BorderStyle = BorderStyle.None;
        canvas.BackColor = Color.FromArgb(113, 91, 100);

        canvas.Anchor = AnchorStyles.None;
        canvas.Dock = DockStyle.Fill;
        canvas.BringToFront();

        canvas.Paint += (s, e) =>
        {
            e.Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;

            foreach(var element in _elements)
            {
                if(element.GetRectangle().GetMouseOver())
                {
                    element.Selected = true;
                }
                else
                {
                    element.Selected = false;
                }

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

        };

        // ~60 fps 
        System.Timers.Timer timer = new(16);
        timer.Start();
        timer.Elapsed += (s, e) =>
        {
            canvas.Refresh();
        };

        return canvas;
    }
}
