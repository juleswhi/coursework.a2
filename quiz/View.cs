using static quiz.Helper;
namespace quiz;

internal class View
{
    public static View Current { get; set; } = new(new(), new());
    public PictureBox Canvas { get; set; }
    public List<ICanvasElement> Elements;

    // ~60 fps 
    System.Timers.Timer timer = new(1);
    public bool Stopped { get; set; } = false;

    public View(List<ICanvasElement> elements, PictureBox canvas)
    {
        Canvas = canvas;
        Elements = elements;
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

            foreach (var element in Elements.Where(x => x.Selected))
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

    public void Popup(string message)
    {
        Elements.ForEach(x => x.Enabled = false);
        Elements.Add(new CanvasPopup(message, null, null, () => GetCenter(0, 0), () =>
        {
            Current.Elements.RemoveAt(Current.Elements.Count - 1);
            // Current.Elements.ForEach(x => x.Enabled = true);
        }));
    }


    private void Paint(object sender, PaintEventArgs e)
    {
        e.Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
        e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
        e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
        e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;

        foreach (var element in Elements)
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

        foreach (var element in Elements)
        {
            element.Render(e.Graphics);
        }

        foreach (var element in Elements)
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
