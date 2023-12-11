namespace quiz;

public partial class formMaster : Form
{
    public Form Active { get; set; }
    private Panel _holder { get; set; } = new();
    public formMaster()
    {
        InitializeComponent();
        _holder.Dock = DockStyle.Fill;
        _holder.BackColor = Color.Aquamarine;
        Controls.Add(_holder);

        Open<formMain>();

    }

    public void Open<T>() where T : Form, new()
    {
        T child = new();
        if (Active != null) Active.Close();

        Active = child;
        Active.TopLevel = false;
        child.FormBorderStyle = FormBorderStyle.None;
        child.Dock = DockStyle.Fill;
        _holder.Controls.Add(child);
        child.BringToFront();
        child.Show();
    }
}
