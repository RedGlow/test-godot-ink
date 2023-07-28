using Godot;

public partial class Fade : Control
{
    [Export]
    AnimationPlayer _animationPlayer;

    [Signal]
    public delegate void FadeFinishedEventHandler();

    public void Do(bool fadeIn)
    {
        _animationPlayer.Play(fadeIn ? "fade_in" : "fade_out");
        Visible = true;
    }

    private void AnimationFinished(StringName animName)
    {
        if (animName == "fade_in")
        {
            Visible = false;
        }
        EmitSignal(SignalName.FadeFinished);
    }
}
