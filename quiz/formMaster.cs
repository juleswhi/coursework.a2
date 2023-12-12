namespace quiz;

public partial class formMaster : Form
{
    public formMaster()
    {
        InitializeComponent();
        Size = new(Size.Width, Size.Height + 100);
        CenterToScreen();

        PictureBox canvas = new View(this, new()
        {
            new CanvasText("Main Menu", Helper.Fonts[3], Helper.Colourscheme[4], (_c) => _c.GetCenter(0, _c.Top - 175)),
            new CanvasButton("Play Quiz!", Helper.Fonts[2], Helper.Colourscheme[1], Helper.Colourscheme[3], (_c) => _c.GetDefaultBoxSize(), (_c) => _c.GetCenter(0, -75)),
            new CanvasButton("Leaderboard", Helper.Fonts[2], Helper.Colourscheme[1], Helper.Colourscheme[3], (_c) => _c.GetDefaultBoxSize(), (_c) => _c.GetCenter(0, 50)),
            new CanvasButton("Settings", Helper.Fonts[2], Helper.Colourscheme[1], Helper.Colourscheme[3], (_c) => _c.GetDefaultBoxSize(), (_c) => _c.GetCenter(0, 175)),
        }).Run();

        Controls.Add(canvas);

        System.Timers.Timer timer = new(16);

        timer.Elapsed += (s, e) =>
        {
            Helper.MouseLocation = PointToClient(MousePosition);
        };

        timer.Start();

    }
}
