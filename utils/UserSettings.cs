using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public class UserSettings
{
    public const string SETTINGS_FILE_PATH = "user://settings.ini";

    public readonly Dictionary<string, string> FontPaths;
    public readonly List<DisplayServer.WindowMode> WindowModes = [
        DisplayServer.WindowMode.Windowed,
        DisplayServer.WindowMode.ExclusiveFullscreen,
    ];

    public UserSettings()
    {
        // find fonts
        string fontsDir = ResourcePaths.AssetDirectories[ResourcePaths.AssetPathsEnum.FONTS];
        FontPaths = ResourcePaths.GetAllResourcePathsInDirectory(fontsDir);

        LoadSettingsFile();
    }

    public static ConfigFile GetConfigFile()
    {
        var config = new ConfigFile();
        Error err = config.Load(SETTINGS_FILE_PATH);
        if (err != Error.Ok) { throw new Exception(err.ToString()); }
        return config;
    }

    public void LoadSettingsFile()
    {
        // load file
        var config = new ConfigFile();
        Error err = config.Load(SETTINGS_FILE_PATH);

        if (err == Error.FileNotFound) { DeafultSettingsFile(); } // if no file, create default
        else if (err != Error.Ok) { throw new Exception(err.ToString()); } // if error, throw
        else { ApplySettingsFile(config); } // apply settings
    }

    public static void SaveSettingsFile(Variant font, Variant window)
    {
        var config = new ConfigFile();

        config.SetValue("Display", "Font", font);
        config.SetValue("Display", "Window", window);

        config.Save(SETTINGS_FILE_PATH);
    }

    public void DeafultSettingsFile()
    {
        var config = new ConfigFile();

        config.SetValue("Display", "Font", 0);
        config.SetValue("Display", "Window", 0);

        config.Save(SETTINGS_FILE_PATH);
        ApplySettingsFile(config);
    }

    private void ApplySettingsFile(ConfigFile config)
    {
        ChangeFont((int)config.GetValue("Display", "Font"));
        ChangeWindowMode((int)config.GetValue("Display", "Window"));
    }

    public void ChangeFont(long index)
    {
        Theme theme = ThemeDB.GetProjectTheme();
        Font font = ThemeDB.FallbackFont;

        if (index != 0)
        {
            string file_name = FontPaths.Keys.ElementAt((Index)(index - 1));
            font = ResourcePaths.LoadResourceFromPath<Font>(FontPaths[file_name]);
        }

        theme.DefaultFont = font;
    }

    public void ChangeWindowMode(long index)
    {
        DisplayServer.WindowMode mode = WindowModes.ElementAt((Index)index);
        DisplayServer.WindowSetMode(mode);
    }
}