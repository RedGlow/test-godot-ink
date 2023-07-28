using Godot;

using GodotInk;

using System.Linq;

public partial class Interactable : StaticBody3D
{
    private InkStory _inkStory;

    public void Initialize(InkStory inkStory)
    {
        _inkStory = inkStory;
        _inkStory.Continued += Check;
        Check();
    }

    public void Deinitialize()
    {
        _inkStory.Continued -= Check;
    }

    private void Check()
    {
        // if this choice does not involve objects, ignore it
        if(InkHelpers.GetTagArguments(_inkStory.CurrentTags, "objects") == null)
        {
            return;
        }
        // This object is visible only if the corresponding choice is present
        Visible = _inkStory.CurrentChoices.Any(choice =>
            InkHelpers.ObjectChoiceName(choice.Text) == Name);
    }
}
