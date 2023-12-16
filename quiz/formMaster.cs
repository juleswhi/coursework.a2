using static quiz.Helper;
using static quiz.TextBoxType;
using static quiz.formMaster.MenuType;
using static quiz.FontSize;
using System.Runtime.CompilerServices;

namespace quiz;

public partial class formMaster : Form
{
    private const MenuType StartingMenu = MainMenu;
    private PictureBox currentCanvas { get; set; } = new();
    public formMaster()
    {
        Helper.Form = this;
        InitializeComponent();
        Size = new(Size.Width, Size.Height + 175);
        CenterToScreen();
        DoubleBuffered = true;
        SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        SetStyle(ControlStyles.ResizeRedraw, true);
        SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        SetStyle(ControlStyles.UserPaint, true);


        Resize += (s, e) =>
        {
            Invalidate();
            Refresh();
        };
        Move += (s, e) =>
        {
            Invalidate();
            Refresh();
        };

        Fonts = new()
        {
            { HeadingOne, new Font(FontFamily.GenericMonospace, 10) },
            { HeadingTwo, new Font(FontFamily.GenericMonospace, 20) },
            { HeadingThree, new Font(FontFamily.GenericMonospace, 30) },
            { HeadingFour, new Font(FontFamily.GenericMonospace, 40) },
            { HeadingFive, new Font(FontFamily.GenericMonospace, 50) },
        };

        KeyDown += (s, e) =>
        {
            CurrentTextBox?.KeyPress(e.KeyCode);
        };

        KeyUp += (s, e) =>
        {
            if (e.KeyCode != Keys.ShiftKey) return;
            CanvasTextBox.ShiftPressed = false;
        };

        TimeSinceLastClick = new();
        TimeSinceLastClick.Start();

        var initialView = ViewBuilder[StartingMenu]();
        initialView.Run();
        currentCanvas = initialView.Canvas;

        Controls.Add(currentCanvas);

        System.Timers.Timer timer = new(1);

        timer.Elapsed += (s, e) =>
            Helper.Form.Invoke(() => MouseLocation = PointToClient(MousePosition));

        timer.Start();
    }

    public enum MenuType
    {
        MainMenu,
        Quiz,
        Leaderboard,
        Profile,
        Login
    }

    private static Dictionary<MenuType, Func<View>> ViewBuilder = new()
    {
        {
            MainMenu, () => new View(new List<ICanvasElement>()
            {
                new CanvasText("Main Menu", null, () => GetCenter(0, View.Current.Canvas.Top - 225)),
                new CanvasButton("Play Quiz!", null, null, () => GetCenter(0, -125), () =>
                {
                    ViewBuilder?[Quiz]().Run();
                }),
                new CanvasButton("Leaderboard", null, null, () => GetCenter(0, 0), () =>
                {
                    ViewBuilder?[Leaderboard]().Run();
                }),
                new CanvasButton("Settings", null, null, () => GetCenter(0, 125), () =>
                {
                     View.Current.Popup("Hello, World");
                }),
                new CanvasButton("Quiz", null, null, () => GetCenter(0, 250), () =>
                {
                    View.Current.Stop();
                    Environment.Exit(0);
                }),
            }, Helper.Form.currentCanvas)
        },

        {
            Quiz, () => new View(new List<ICanvasElement>()
            {
                new CanvasText("Question Title", null, () => GetCenter(0, View.Current.Canvas.Top - 175)),
                new CanvasButton("Answer 1", null, null, () => GetCenter(-125, -75), () =>
                {
                    ViewBuilder?[MainMenu]().Run();
                }),
                new CanvasButton("Answer 2", null, null, () => GetCenter(125, -75), () =>
                {}),
                new CanvasButton("Answer 3", null, null, () => GetCenter(-125, 50), () =>
                {}),
                new CanvasButton("Answer 4", null, null, () => GetCenter(125, 50), () =>
                {}),
            }, Helper.Form.currentCanvas)
        },

        {
            Profile, () => new View(new List<ICanvasElement>()
            {
                new CanvasText("Users' Name", null, () => GetCenter(0, View.Current.Canvas.Top - 175)),
            }, Helper.Form.currentCanvas)
        },

        {
            Leaderboard, () => new View(new List<ICanvasElement>()
            {
                new CanvasText("Leaderboard", null, () => GetCenter(0, View.Current.Canvas.Top - 175)),
                new CanvasButton("Back", null, null, () => GetCenter(0, 200), () =>
                {
                    ViewBuilder?[MainMenu]().Run();

                }),
                new CanvasTextBox(null, null, None, () => GetCenter(0, 0), () => {
                    return;
                })
            }, Helper.Form.currentCanvas)
        },
        {
            Login, () => new View(new List<ICanvasElement>()
            {
                new CanvasText("Leaderboard", null, () => GetCenter(0, View.Current.Canvas.Top - 175)),
                new CanvasText("Username: ", Fonts[HeadingTwo], () => GetCenter(-200, -50)),
                new CanvasTextBox(null, () => new Size(200, 75), Username, () => GetCenter(0, -50), () => {
                    return;
                }),
                new CanvasText("Password: ", Fonts[HeadingTwo], () => GetCenter(-200, 50)),
                new CanvasTextBox(null, () => new Size(200, 75), Password, () => GetCenter(0, 50), () => {
                    return;
                }),
                new CanvasButton("Login", null, null, () => GetCenter(0, 200), () =>
                {
                    var username = View.Current.Elements.Where(x => x is CanvasTextBox)
                        .Where(x => (x as CanvasTextBox)?.Tag == Username).Select(x => (x as CanvasTextBox)?.Text).FirstOrDefault();

                    var password = View.Current.Elements.Where(x => x is CanvasTextBox)
                        .Where(x => (x as CanvasTextBox)?.Tag == Password).Select(x => (x as CanvasTextBox)?.Text).FirstOrDefault();

                    if(username is null || password is null)
                    {
                        return;
                    }
                    Users = DeserializeUsers();

                    User user = new User(string.Join("", username), string.Join("", password));

                    if(user.TryLogin())
                    {
                        ViewBuilder?[MainMenu]().Run();
                    }

                }),
            }, Helper.Form.currentCanvas)
        }
    };
}
