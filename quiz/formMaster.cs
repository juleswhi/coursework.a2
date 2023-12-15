using static quiz.formMaster.MenuType;
using static quiz.FontSize;
namespace quiz;

public partial class formMaster : Form
{
    private PictureBox currentCanvas { get; set; } = new();
    public formMaster()
    {
        DoubleBuffered = true;
        Helper.Form = this;
        InitializeComponent();
        Size = new(Size.Width, Size.Height + 175);
        CenterToScreen();
        Helper.Fonts = new()
        {
            { HeadingOne, new Font(FontFamily.GenericMonospace, 10) },
            { HeadingTwo, new Font(FontFamily.GenericMonospace, 20) },
            { HeadingThree, new Font(FontFamily.GenericMonospace, 30) },
            { HeadingFour, new Font(FontFamily.GenericMonospace, 40) },
            { HeadingFive, new Font(FontFamily.GenericMonospace, 50) },
        };

        KeyDown += (s, e) =>
        {
            Helper.CurrentTextBox?.KeyPress(e.KeyCode);
        };

        KeyUp += (s, e) =>
        {
            if (e.KeyCode != Keys.ShiftKey) return;

            CanvasTextBox.ShiftPressed = false;
        };

        Helper.TimeSinceLastClick = new();
        Helper.TimeSinceLastClick.Start();

        var initialView = ViewBuilder[MainMenu]();
        initialView.Run();
        currentCanvas = initialView.Canvas;

        Controls.Add(currentCanvas);

        System.Timers.Timer timer = new(16);

        timer.Elapsed += (s, e) =>
            Helper.Form.Invoke(() => Helper.MouseLocation = PointToClient(MousePosition));

        timer.Start();
    }

    public enum MenuType
    {
        MainMenu,
        Quiz,
        Leaderboard,
        Profile
    }


    private static Dictionary<MenuType, Func<View>> ViewBuilder = new()
    {
        {
            MainMenu, () => new View(new List<ICanvasElement>()
            {
                new CanvasText("Main Menu", null, () => Helper.GetCenter(0, View.Current.Canvas.Top - 225)),
                new CanvasButton("Play Quiz!", null, null, () => Helper.GetCenter(0, -125), () =>
                {
                    ViewBuilder?[Quiz]().Run();
                }),
                new CanvasButton("Leaderboard", null, null, () => Helper.GetCenter(0, 0), () =>
                {
                    ViewBuilder?[Leaderboard]().Run();
                }),
                new CanvasButton("Settings", null, null, () => Helper.GetCenter(0, 125), () =>
                {}),
                new CanvasButton("Quiz", null, null, () => Helper.GetCenter(0, 250), () =>
                {
                    View.Current.Stop();
                    Environment.Exit(0);
                }),
            }, Helper.Form.currentCanvas)
        },

        {
            Quiz, () => new View(new List<ICanvasElement>()
            {
                new CanvasText("Question Title", null, () => Helper.GetCenter(0, View.Current.Canvas.Top - 175)),
                new CanvasButton("Answer 1", null, null, () => Helper.GetCenter(-125, -75), () =>
                {
                    ViewBuilder?[MainMenu]().Run();
                }),
                new CanvasButton("Answer 2", null, null, () => Helper.GetCenter(125, -75), () =>
                {}),
                new CanvasButton("Answer 3", null, null, () => Helper.GetCenter(-125, 50), () =>
                {}),
                new CanvasButton("Answer 4", null, null, () => Helper.GetCenter(125, 50), () =>
                {}),
            }, Helper.Form.currentCanvas)
        },

        {
            Profile, () => new View(new List<ICanvasElement>()
            {
                new CanvasText("Users' Name", null, () => Helper.GetCenter(0, View.Current.Canvas.Top - 175)),
            }, Helper.Form.currentCanvas)
        },

        {
            Leaderboard, () => new View(new List<ICanvasElement>()
            {
                new CanvasText("Leaderboard", null, () => Helper.GetCenter(0, View.Current.Canvas.Top - 175)),
                new CanvasButton("Back", null, null, () => Helper.GetCenter(0, 200), () =>
                {
                    ViewBuilder?[MainMenu]().Run();

                }),
                new CanvasTextBox(null, null, () => Helper.GetCenter(0, 0), () => {
                    return;
                })
            }, Helper.Form.currentCanvas)
        }
    };
}
