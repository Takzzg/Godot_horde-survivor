using System;
using System.Collections.Generic;
using Godot;

public partial class DebugCategory : PanelContainer
{
    private BoxContainer _container;

    public DebugCategory(string title)
    {
        MarginContainer margin = new();
        AddChild(margin);

        BoxContainer boxCont = new() { Vertical = true };
        margin.AddChild(boxCont);

        PanelContainer panel = new() { };
        boxCont.AddChild(panel);

        Label label = new()
        {
            Text = title,
            HorizontalAlignment = HorizontalAlignment.Center,
            Theme = new Theme() { DefaultFontSize = 16 }
        };
        panel.AddChild(label);

        _container = new BoxContainer() { Vertical = true };
        boxCont.AddChild(_container);
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

    public void CreateDivider(string text)
    {
        Divider divider = new(text);
        _container.AddChild(divider);
    }

    // -------------------------------------------- Label Field --------------------------------------------
    private readonly Dictionary<string, LabelField> _labelFieldList = [];

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

    public void CreateLabelField(string id, string title, string value)
    {
        LabelField field = new(title, value);
        _labelFieldList.Add(id, field);
        _container.AddChild(field);
    }

    public void UpdateLabelField(string id, string value)
    {
        // GD.Print($"DebugTryUpdateField id: {id}, value: {value}");
        _labelFieldList[id].UpdateValue(value);
    }

    // -------------------------------------------- Button Field --------------------------------------------
    private readonly Dictionary<string, ButtonField> _buttonFieldList = [];

    private partial class ButtonField : BoxContainer
    {
        private Label _title;
        private Button _btn;

        public ButtonField(string title, string value, Action callback)
        {
            Theme theme = new() { DefaultFontSize = 14 };
            _title = new() { Theme = theme, Text = title };
            _btn = new() { Theme = theme, Text = value };
            _btn.Pressed += callback;

            AddChild(_title);
            AddChild(_btn);
        }

        public void UpdateValue(string value)
        {
            if (_btn.Text == value) return;
            _btn.Text = value;
        }
    }

    public void CreateButtonField(string id, string title, string value, Action callback)
    {
        ButtonField field = new(title, value, callback);
        _buttonFieldList.Add(id, field);
        _container.AddChild(field);
    }

    public void UpdateButtonField(string id, string value)
    {
        _buttonFieldList[id].UpdateValue(value);
    }
}