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
    private Dictionary<string, Font> _fonts;

    public override void _Ready()
    {
        _cancelBtn.Pressed += QueueFree;

        LoadFonts();
    }

    private void LoadFonts()
    {
        string fontsPath = ResourcePaths.AssetPaths[ResourcePaths.AssetPathsEnum.FONTS];
        _fonts = ResourcePaths.LoadResourcesFromDirectory<Font>(fontsPath);

        // GD.Print($"fonts: {_fonts}");
        // GD.Print($"fonts.Keys.Count: {_fonts.Keys.Count}");

        _fontOptionBtn.AddItem("Default");
        _fontOptionBtn.Selected = 0;
        _fontOptionBtn.ItemSelected += ChangeFont;

        foreach (string key in _fonts.Keys) { _fontOptionBtn.AddItem(key); }
    }

    private void ChangeFont(long index)
    {
        // GD.Print($"selected option: index {index}, text {_fontOptionBtn.GetItemText((int)index)}");
        Theme theme = ThemeDB.GetProjectTheme();

        Font font = index == 0 ? ThemeDB.FallbackFont : _fonts[_fonts.Keys.ElementAt((Index)(index - 1))];
        theme.DefaultFont = font;
    }
}
