using System.Collections.Generic;
using Godot;

public static class ResourcePaths
{
    public enum EnumPathsDictionary { LOADING_SCREEN, MAIN_MENU, PLAYER_GAMEPLAY_UI, PLAYER_DEATH_UI }
    public static Dictionary<EnumPathsDictionary, string> PathsDictionary = new() {
        {EnumPathsDictionary.MAIN_MENU, "res://scenes/main_menu/main_menu.tscn"},
        {EnumPathsDictionary.LOADING_SCREEN, "res://scenes/loading_screen/loading_screen.tscn"},
        {EnumPathsDictionary.PLAYER_DEATH_UI, "res://entities/player/components/player_ui/death_ui.tscn"},
        {EnumPathsDictionary.PLAYER_GAMEPLAY_UI, "res://entities/player/components/player_ui/gameplay_ui/gameplay_ui.tscn"},
    };

    public static PackedScene GetPackedSceneFromEnum(EnumPathsDictionary key)
    {
        PackedScene scene = GD.Load<PackedScene>(PathsDictionary[key]);
        return scene;
    }

    public static T GetInstanceFromEnum<T>(EnumPathsDictionary key) where T : Node
    {
        PackedScene scene = GetPackedSceneFromEnum(key);
        T instance = scene.Instantiate<T>();
        return instance;
    }
}