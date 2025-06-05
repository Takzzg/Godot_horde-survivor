using Godot;

public partial class DebugManager : Node2D
{
    public static DebugManager Instance { get; private set; }
    public bool DebugEnabled = true;
    public int MarginSize = 8;

    private CanvasLayer _layer;
    private BoxContainer _container;

    [Signal]
    public delegate void DebugStateToggledEventHandler(bool state);

    public DebugManager()
    {
        ProcessMode = ProcessModeEnum.Always;
        Instance = this;

        // create layer
        _layer = new CanvasLayer() { Layer = 100 };
        AddChild(_layer);

        // create scroll container
        ScrollContainer scroll = new()
        {
            CustomMinimumSize = new Vector2(250, 0),
            GrowHorizontal = Control.GrowDirection.Begin
        };
        scroll.SetAnchorsPreset(Control.LayoutPreset.RightWide);
        _layer.AddChild(scroll);

        // create panel container
        PanelContainer panel = new()
        {
            SizeFlagsHorizontal = Control.SizeFlags.ExpandFill,
            SizeFlagsVertical = Control.SizeFlags.ExpandFill
        };
        scroll.AddChild(panel);

        // create margin
        MarginContainer margin = new();
        margin.AddThemeConstantOverride("margin_top", MarginSize);
        margin.AddThemeConstantOverride("margin_left", MarginSize);
        margin.AddThemeConstantOverride("margin_bottom", MarginSize);
        margin.AddThemeConstantOverride("margin_right", MarginSize);
        panel.AddChild(margin);

        // create box container
        _container = new BoxContainer() { Vertical = true };
        margin.AddChild(_container);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("toggle_debug")) { ToggleDebugPanel(); }
    }

    private void ToggleDebugPanel()
    {
        DebugEnabled = !DebugEnabled;
        _layer.Visible = DebugEnabled;

        EmitSignal(SignalName.DebugStateToggled, DebugEnabled);
    }

    public void RenderNode(Control node)
    {
        _container.AddChild(node);
    }
}