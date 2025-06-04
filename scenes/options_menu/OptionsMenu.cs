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

    public override void _Ready()
    {
        _cancelBtn.Pressed += QueueFree;

        SetupOptionBtn_Font();
        SetupOptionBtn_Window();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("back"))
        {
            GetViewport().SetInputAsHandled();
            QueueFree();
        }
    }

    // -------------------------------------------- Font --------------------------------------------

    [Export]
    private OptionButton _fontOptionBtn;
    private Dictionary<string, string> _fontPaths;

    private void SetupOptionBtn_Font()
    {
        // read files from fonst dir
        string fontsDir = ResourcePaths.AssetPaths[ResourcePaths.AssetPathsEnum.FONTS];
        _fontPaths = ResourcePaths.GetAllResourcePathsInDirectory(fontsDir);

        // create items
        _fontOptionBtn.AddItem("Default");
        foreach (string file_name in _fontPaths.Keys) { _fontOptionBtn.AddItem(file_name); }

        // initialize options button
        _fontOptionBtn.Selected = 0;
        _fontOptionBtn.ItemSelected += ChangeFont;
    }

    private void ChangeFont(long index)
    {
        Theme theme = ThemeDB.GetProjectTheme();
        Font font = ThemeDB.FallbackFont;

        if (index != 0)
        {
            string file_name = _fontPaths.Keys.ElementAt((Index)(index - 1));
            font = ResourcePaths.LoadResourceFromPath<Font>(_fontPaths[file_name]);
        }

        theme.DefaultFont = font;
    }

    // -------------------------------------------- Window --------------------------------------------
    [Export]
    private OptionButton _windowOptionBtn;
    private static List<DisplayServer.WindowMode> _windowModes = [
        DisplayServer.WindowMode.Windowed,
        DisplayServer.WindowMode.ExclusiveFullscreen,
    ];

    private void SetupOptionBtn_Window()
    {
        // create items
        foreach (DisplayServer.WindowMode mode in _windowModes) { _windowOptionBtn.AddItem(mode.ToString()); }

        // initialize options button
        _windowOptionBtn.Selected = 0;
        _windowOptionBtn.ItemSelected += ChangeWindowMode;
    }

    private void ChangeWindowMode(long index)
    {
        DisplayServer.WindowMode mode = _windowModes.ElementAt((Index)index);
        DisplayServer.WindowSetMode(mode);
    }
}
