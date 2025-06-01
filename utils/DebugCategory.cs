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
        LabelField field = new(title, value);
        _debugLabelsList.Add(id, field);
        _container.AddChild(field);
    }

    public void UpdateLabelField(string id, string value)
    {
        // GD.Print($"DebugTryUpdateField id: {id}, value: {value}");
        _debugLabelsList[id].UpdateValue(value);
    }

    // -------------------------------------------- Title  --------------------------------------------
    private partial class Title : PanelContainer
    {
        public int MarginSize = 4;

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
                HorizontalAlignment = HorizontalAlignment.Center,
                Theme = new Theme() { DefaultFontSize = 16 }
            };
            margin.AddChild(title);
        }
    }

    // -------------------------------------------- Divier --------------------------------------------
    private partial class Divider : PanelContainer
    {
        public int MarginSize = 2;

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
                Theme = new Theme() { DefaultFontSize = 14 }
            };
            margin.AddChild(title);
        }
    }

    // -------------------------------------------- Label Field --------------------------------------------
    private partial class LabelField : BoxContainer
    {
        private Label _title;
        private Label _value;

        public LabelField(string title, string value)
        {
            Theme theme = new() { DefaultFontSize = 14 };
            _title = new() { Theme = theme, Text = title };
            _value = new() { Theme = theme, Text = value };

            AddChild(_title);
            AddChild(new Label() { Text = ":" });
            AddChild(_value);
        }

        public void UpdateValue(string value)
        {
            if (_value.Text == value) return;
            _value.Text = value;
        }
    }
}