using System.Collections.Generic;
using Godot;

public partial class DebugCategory : PanelContainer
{
    public int MarginSize = 8;

    private BoxContainer _container;

    private readonly Dictionary<string, LabelField> _debugLabelsList = [];

    public DebugCategory(string title)
    {
        MarginContainer margin = new();
        margin.AddThemeConstantOverride("margin_top", MarginSize);
        margin.AddThemeConstantOverride("margin_left", MarginSize);
        margin.AddThemeConstantOverride("margin_bottom", MarginSize);
        margin.AddThemeConstantOverride("margin_right", MarginSize);
        AddChild(margin);

        BoxContainer cont = new() { Vertical = true };
        margin.AddChild(cont);

        Title catTitle = new(title);
        cont.AddChild(catTitle);

        _container = new() { Vertical = true };
        cont.AddChild(_container);
    }

    public void CreateDivider(string text)
    {
        Divider divider = new(text);
        _container.AddChild(divider);
    }

    public void CreateLabelField(string id, string title, string value)
    {
        LabelField field = new(id, title, value);
        _debugLabelsList.Add(id, field);
        _container.AddChild(field);
    }

    public void UpdateLabelField(string id, string value)
    {
        _debugLabelsList[id].Value.Text = value;
    }

    // -------------------------------------------- Title  --------------------------------------------
    private partial class Title : PanelContainer
    {
        public int MarginSize = 8;

        public Title(string text)
        {
            MarginContainer margin = new();
            margin.AddThemeConstantOverride("margin_top", MarginSize);
            margin.AddThemeConstantOverride("margin_left", MarginSize);
            margin.AddThemeConstantOverride("margin_bottom", MarginSize);
            margin.AddThemeConstantOverride("margin_right", MarginSize);
            AddChild(margin);

            Label title = new()
            {
                Text = text,
                Theme = new Theme() { DefaultFontSize = 20 },
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            margin.AddChild(title);
        }
    }

    // -------------------------------------------- Divier --------------------------------------------
    private partial class Divider : PanelContainer
    {
        public int MarginSize = 8;

        public Divider(string text)
        {
            MarginContainer margin = new();
            margin.AddThemeConstantOverride("margin_top", MarginSize);
            margin.AddThemeConstantOverride("margin_left", MarginSize);
            margin.AddThemeConstantOverride("margin_bottom", MarginSize);
            margin.AddThemeConstantOverride("margin_right", MarginSize);
            AddChild(margin);

            Label title = new()
            {
                Text = text,
                HorizontalAlignment = HorizontalAlignment.Center,
                Theme = new Theme() { DefaultFontSize = 12 }
            };
            margin.AddChild(title);
        }
    }

    // -------------------------------------------- Label Field --------------------------------------------
    private partial class LabelField : BoxContainer
    {
        public string ID;
        public Label Title;
        public Label Value;

        public LabelField(string id, string title, string value)
        {
            ID = id;
            Title = new() { Text = title };
            Value = new() { Text = value };

            Label titleLabel = new() { Text = title };
            Label valueLabel = new() { Text = value };

            AddChild(titleLabel);
            AddChild(new Label() { Text = ":" });
            AddChild(valueLabel);
        }
    }
}