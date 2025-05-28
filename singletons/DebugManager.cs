using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class DebugManager : Node2D
{
    public static DebugManager Instance { get; private set; }
    public bool DebugEnabled = true;
    public int MarginSize = 8;

    private CanvasLayer _layer;
    private PanelContainer _panel;
    private BoxContainer _container;

    [Signal]
    public delegate void DebugStateToggledEventHandler(bool state);

    public DebugManager()
    {
        Instance = this;

        // create layer
        _layer = new CanvasLayer() { Layer = 100 };
        AddChild(_layer);

        // create panel container
        _panel = new PanelContainer() { CustomMinimumSize = new Vector2(250, 0) };
        _panel.SetAnchorsPreset(Control.LayoutPreset.LeftWide);
        _layer.AddChild(_panel);

        // create scroll container
        ScrollContainer scroll = new();
        _panel.AddChild(scroll);

        // create margin
        MarginContainer margin = new() { SizeFlagsHorizontal = Control.SizeFlags.ExpandFill };
        margin.AddThemeConstantOverride("margin_top", MarginSize);
        margin.AddThemeConstantOverride("margin_left", MarginSize);
        margin.AddThemeConstantOverride("margin_bottom", MarginSize);
        margin.AddThemeConstantOverride("margin_right", MarginSize);
        scroll.AddChild(margin);

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
        if (node.Size.X > _panel.Size.X) _panel.Size = _panel.Size with { X = node.Size.X };
    }

    public partial class DebugTitle : MarginContainer
    {
        public int MarginSize = 8;

        public DebugTitle(string text)
        {
            AddThemeConstantOverride("margin_top", MarginSize);
            AddThemeConstantOverride("margin_left", MarginSize);
            AddThemeConstantOverride("margin_bottom", MarginSize);
            AddThemeConstantOverride("margin_right", MarginSize);

            Label title = new()
            {
                Text = text,
                Theme = new Theme() { DefaultFontSize = 16 },
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            AddChild(title);
        }
    }

    public partial class DebugCategory : PanelContainer
    {
        public string ID;
        public Label TitleLabel;
        public int MarginSize = 8;

        private BoxContainer _labelsCont;

        private readonly Dictionary<string, DebugField> _debugLabelsList = [];

        public DebugCategory(string id, string title)
        {
            ID = id;

            MarginContainer margin = new();
            margin.AddThemeConstantOverride("margin_top", MarginSize);
            margin.AddThemeConstantOverride("margin_left", MarginSize);
            margin.AddThemeConstantOverride("margin_bottom", MarginSize);
            margin.AddThemeConstantOverride("margin_right", MarginSize);
            AddChild(margin);

            BoxContainer cont = new() { Vertical = true };
            margin.AddChild(cont);

            TitleLabel = new Label() { Text = title, HorizontalAlignment = HorizontalAlignment.Center };
            cont.AddChild(TitleLabel);

            _labelsCont = new() { Vertical = true };
            cont.AddChild(_labelsCont);
        }

        public void CreateField(string id, string title, string value)
        {
            Label titleLabel = new() { Text = title };
            Label valueLabel = new() { Text = value };
            DebugField field = new(id, titleLabel, valueLabel);
            _debugLabelsList.Add(id, field);

            BoxContainer cont = new();
            cont.AddChild(titleLabel);
            cont.AddChild(new Label() { Text = ":" });
            cont.AddChild(valueLabel);

            _labelsCont.AddChild(cont);
        }

        public void UpdateFieldValue(string id, string value)
        {
            _debugLabelsList[id].Value.Text = value;
        }

        public class DebugField(string id, Label title, Label value)
        {
            public string ID = id;
            public Label Title = title;
            public Label Value = value;
        }
    }
}