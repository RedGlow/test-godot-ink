using Godot;

public partial class WinInterface : Control
{
    [Export]
    private Label _label;

    public void Show(string text) {
        _label.Text = text;
        Visible = true;
    }
}
