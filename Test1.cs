using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Godot;

using GodotInk;

public partial class Test1 : Node3D
{
    [Export] private float _rayDistance = 2000;

    [Export] private InkStory _inkStory;

    [Export] private Dialogue _dialogue;

    [Export] private Fade _fade;

    [Export] private Control _winInterface;

    private Node3D _currentScene;

    public override void _Ready()
    {
        _inkStory.Errored += OnError;
        _inkStory.BindExternalFunction("goToRoom",
            new Callable(this, MethodName.GoToRoomInk));
        _inkStory.BindExternalFunction("win",
            () => _winInterface.Visible = true);
        _inkStory.StoreVariable("IN_GODOT", true);
        _inkStory.Continue();
    }

    private void OnError(string message, string type)
    {
        GD.PrintErr(type, message);
    }

    private Vector2? _viewportPositionClick;

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton eventMouseButton &&
            eventMouseButton.Pressed)
        {
            _viewportPositionClick = eventMouseButton.Position;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        // check if we have a click to process
        if (_viewportPositionClick is not Vector2 position)
        {
            return;
        }
        // launch a raycast query to check if there's some object to activate
        _viewportPositionClick = null;
        var spaceState = GetWorld3D().DirectSpaceState;
        var camera = GetViewport().GetCamera3D();
        var origin = camera.ProjectRayOrigin(position);
        var destination = origin + camera.ProjectRayNormal(position) * _rayDistance;
        var query = PhysicsRayQueryParameters3D.Create(
            origin,
            destination
        );
        query.CollideWithBodies = true;
        var result = spaceState.IntersectRay(query);
        if (result.TryGetValue("collider", out var collider) &&
            collider.VariantType == Variant.Type.Object &&
            collider.Obj is Node node)
        {
            // there is: check if there's a related story node
            foreach (var choice in _inkStory.CurrentChoices)
            {
                if (InkHelpers.ObjectChoiceName(choice.Text) == node.Name)
                {
                    // there is: follow this choice
                    _inkStory.ChooseChoiceIndex(choice.Index);
                    _inkStory.Continue();
                    break;
                }
            }
        }
    }

    private void GoToRoomInk(InkPath roomDivert)
    {
        _ = GoToRoom(roomDivert.ToString());
    }

    private async Task GoToRoom(string room)
    {
        // close dialogue
        _dialogue.Visible = false;
        // deinitialize interactable objects
        SetupInteractableObjects(false);
        // fade out
        _fade.Do(false);
        await ToSignal(_fade, Fade.SignalName.FadeFinished);
        // remove current room (if present)
        if (_currentScene != null)
        {
            RemoveChild(_currentScene);
            _currentScene.QueueFree();
        }
        // get room scene name
        var tagStrings = _inkStory.TagsForContentAtPath(room);
        var sceneArguments = InkHelpers.GetTagArguments(tagStrings, "scene");
        string sceneName = sceneArguments?[0];
        // add new room
        var scene = GD.Load($"res://{sceneName}") as PackedScene;
        _currentScene = scene.Instantiate<Node3D>();
        AddChild(_currentScene);
        // setup interactable objects
        SetupInteractableObjects(true);
        // fade in
        _fade.Do(true);
        await ToSignal(_fade, Fade.SignalName.FadeFinished);
    }

    private void SetupInteractableObjects(bool initialize)
    {
        if (_currentScene == null)
        {
            return;
        }
        for (var i = 0; i < _currentScene.GetChildCount(); i++)
        {
            if (_currentScene.GetChild(i) is Interactable interactable)
            {
                if (initialize)
                {
                    interactable.Initialize(_inkStory);
                }
                else
                {
                    interactable.Deinitialize();
                }
            }
        }
    }
}
