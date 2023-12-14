namespace quiz;
public static class Colourscheme
{
    public enum Colours
    {
        PALE_DOGWOOD,
        KHAKI,
        RESEDA_GREEN,
        WENGE,
        VIOLET
    }

    public static Dictionary<Colours, Color> ColorDictionary = new() {
        { Colours.PALE_DOGWOOD, Color.FromArgb(211,189,176) },
        { Colours.KHAKI, Color.FromArgb(193,174,159) },
        { Colours.RESEDA_GREEN, Color.FromArgb(137,147,124) },
        { Colours.WENGE, Color.FromArgb(113,91,100) },
        { Colours.VIOLET, Color.FromArgb(105,56,92) }
    };

    public static Dictionary<Colours, Brush> BrushDictionary = new() {
        { Colours.PALE_DOGWOOD, new SolidBrush(Color.FromArgb(211,189,176)) },
        { Colours.KHAKI, new SolidBrush(Color.FromArgb(193,174,159)) },
        { Colours.RESEDA_GREEN, new SolidBrush(Color.FromArgb(137,147,124)) },
        { Colours.WENGE, new SolidBrush(Color.FromArgb(113,91,100)) },
        { Colours.VIOLET, new SolidBrush(Color.FromArgb(105,56,92)) }
    };

}