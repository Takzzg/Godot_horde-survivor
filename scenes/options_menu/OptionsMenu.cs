using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class OptionsMenu : Control
{
    [Export]
    private Button _cancelBtn;
    [Export]
    private Button _defatulBtn;
    [Export]
    private Button _saveBtn;

    [Export]
    private OptionButton _fontOptionBtn;
    [Export]
    private OptionButton _windowOptionBtn;

    public override void _Ready()
    {
        _cancelBtn.Pressed += OnCancel;
        _defatulBtn.Pressed += OnDefault;
        _saveBtn.Pressed += OnSave;

        CreateItems();
        ReadValuesFromFile();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("back"))
        {
            GetViewport().SetInputAsHandled();
            OnCancel();
        }
    }

    private void CreateItems()
    {
        // Font
        _fontOptionBtn.AddItem("Default");
        foreach (string file_name in GameManager.Instance.Settings.FontPaths.Keys) { _fontOptionBtn.AddItem(file_name); }
        _fontOptionBtn.ItemSelected += GameManager.Instance.Settings.ChangeFont;

        // Window
        foreach (DisplayServer.WindowMode mode in GameManager.Instance.Settings.WindowModes) { _windowOptionBtn.AddItem(mode.ToString()); }
        _windowOptionBtn.ItemSelected += GameManager.Instance.Settings.ChangeWindowMode;
    }

    private void ReadValuesFromFile()
    {
        ConfigFile config = UserSettings.GetConfigFile();
        _fontOptionBtn.Selected = (int)config.GetValue("Display", "Font");
        _windowOptionBtn.Selected = (int)config.GetValue("Display", "Window");
    }

    private void OnCancel()
    {
        GameManager.Instance.Settings.LoadSettingsFile();
        QueueFree();
    }

    private void OnSave()
    {
        UserSettings.SaveSettingsFile(
            _fontOptionBtn.Selected,
            _windowOptionBtn.Selected
        );
    }

    private void OnDefault()
    {
        GameManager.Instance.Settings.DeafultSettingsFile();
        ReadValuesFromFile();
    }
}
