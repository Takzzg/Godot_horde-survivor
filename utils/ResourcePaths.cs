using System.Collections.Generic;
using Godot;

public static class ResourcePaths
{
    public static Dictionary<string, T> LoadResourcesFromDirectory<T>(string path) where T : Resource
    {
        Dictionary<string, T> resources = [];

        DirAccess dir_access = DirAccess.Open(path);
        if (dir_access == null) { return null; }

        string[] files = dir_access.GetFiles();
        if (files == null) { return null; }

        foreach (string file_name in files)
        {
            if (file_name.EndsWith(".import")) { continue; }

            T loaded_resource = GD.Load<T>(path + "/" + file_name);
            if (loaded_resource == null) { continue; }

            GD.Print($"resouce loaded: {path + "/" + file_name}, {loaded_resource}");
            resources[file_name] = loaded_resource;
        }

        return resources;
    }

    // -------------------------------------------- Assets --------------------------------------------
    public enum AssetPathsEnum { FONTS }
    public static Dictionary<AssetPathsEnum, string> AssetPaths = new()
    {
        {AssetPathsEnum.FONTS, "res://assets/fonts"},
    };

    // -------------------------------------------- Scenes --------------------------------------------
    public enum ScenePathsEnum { LOADING_SCREEN, MAIN_MENU, OPTIONS_MENU, PLAYER_GAMEPLAY_UI, PLAYER_DEATH_UI }
    public static Dictionary<ScenePathsEnum, string> ScenePaths = new() {
        {ScenePathsEnum.MAIN_MENU, "res://scenes/main_menu/main_menu.tscn"},
        {ScenePathsEnum.LOADING_SCREEN, "res://scenes/loading_screen/loading_screen.tscn"},
        {ScenePathsEnum.OPTIONS_MENU, "res://scenes/options_menu/options_menu.tscn"},
        {ScenePathsEnum.PLAYER_DEATH_UI, "res://entities/player/components/player_ui/death_ui.tscn"},
        {ScenePathsEnum.PLAYER_GAMEPLAY_UI, "res://entities/player/components/player_ui/gameplay_ui/gameplay_ui.tscn"},
    };

    public static T GetSceneInstanceFromEnum<T>(ScenePathsEnum key) where T : Node
    {
        PackedScene scene = GD.Load<PackedScene>(ScenePaths[key]);
        T instance = scene.Instantiate<T>();
        return instance;
    }
}