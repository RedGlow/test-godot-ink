using System.Linq;

using Godot;

using GodotInk;

public partial class Dialogue : Control
{
    [Export]
    private InkStory _inkStory;

    [Export]
    private Control _container;

    private Control _choicesContainer;

    public override void _Ready()
    {
        _inkStory.Continued += InkStory_Continued;
        Visible = false;
    }

    private void InkStory_Continued()
    {
        var dialogueChoices = (
            from choice in _inkStory.CurrentChoices
            where InkHelpers.ObjectChoiceName(choice.Text) == null
            select choice)
            .ToList();
        var currentText = _inkStory.CurrentText.Trim();

        // clear the choices container: if we are at a new step, previous choices
        // are outdated anyway
        if (_choicesContainer != null)
        {
            foreach (var child in _choicesContainer.GetChildren())
            {
                child.QueueFree();
            }
            _container.RemoveChild(_choicesContainer);
            _choicesContainer.QueueFree();
            _choicesContainer = null;
        }

        // if we are going to show the dialogue, empty it first
        if (!Visible)
        {
            foreach (var child in _container.GetChildren())
            {
                child.QueueFree();
            }
        }

        // add text if there's some
        if (!string.IsNullOrEmpty(currentText))
        {
            var label = new Label
            {
                Text = currentText
            };
            _container.AddChild(label);
            Visible = true;
        }

        // if we have choices, add the list of choices
        if (dialogueChoices.Count > 0)
        {
            _choicesContainer = new HBoxContainer();
            foreach (var choice in dialogueChoices)
            {
                var button = new Button { Text = choice.Text };
                button.Pressed += () =>
                {
                    _inkStory.ChooseChoiceIndex(choice.Index);
                    _inkStory.Continue();
                };
                _choicesContainer.AddChild(button);
            }
            _container.AddChild(_choicesContainer);
            Visible = true;
        }
        // otherwise, maybe we can continue? then add a continue button
        else if (_inkStory.CanContinue)
        {
            _choicesContainer = new HBoxContainer();
            var continueButton = new Button { Text = "Continue" };
            continueButton.Pressed += () =>
            {
                _inkStory.Continue();
            };
            _choicesContainer.AddChild(continueButton);
            _container.AddChild(_choicesContainer);
            Visible = true;
        }
        // there are no choices and neither we can continue: if there's some
        // content displayed, show a button to close the dialogue (otherwise
        // just do nothing!)
        else if (Visible)
        {
            var quitButton = new Button
            {
                Text = "[close]"
            };
            quitButton.Pressed += () => Visible = false;
            _container.AddChild(quitButton);
        }
    }
}
