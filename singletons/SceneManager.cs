using System;
using System.Linq;
using Godot;

public partial class SceneManager : Node
{
    public static SceneManager Instance { get; private set; }
    private Node _currentNode;
    public enum EnumScenes { MAIN_MENU, MAIN_GAME, TEST_SCENARIO };

    // loading
    private LoadingScreen _loadingScreen;
    private string _loadDir;
    private bool _isloading;

    public override void _Ready()
    {
        GD.Print($"SceneManager singleton ready!");
        Instance = this;

        _currentNode = GetTree().Root.GetChildren().Last();
        GD.Print($"_currentNode.Name: {_currentNode.Name}");
    }

    public override void _Process(double delta)
    {
        if (!_isloading) return;
        if (_loadDir == "") return;

        // GD.Print($"SceneManager _Process()");
        // GD.Print($"_isloading: {_isloading}, _loadDir: {_loadDir}");

        Godot.Collections.Array loadpercent = [];
        ResourceLoader.ThreadLoadStatus status = ResourceLoader.LoadThreadedGetStatus(_loadDir, loadpercent);
        double percentage = (double)loadpercent[0] * 100;

        // GD.Print($"load status: {status}");
        // GD.Print($"load percent: {percentage}");

        if (status == ResourceLoader.ThreadLoadStatus.Loaded || status == ResourceLoader.ThreadLoadStatus.InvalidResource)
        {
            if (_loadingScreen.Tween != null)
            {
                // GD.Print($"tween running: {_loadingScreen.Tween.IsRunning()}");
                if (_loadingScreen.Tween.IsRunning()) return;
            }

            // GD.Print($"creating final tween");
            _isloading = false;
            if (status == ResourceLoader.ThreadLoadStatus.Loaded) _loadingScreen.TweenProgressBar(percentage, InstantiatePackedScene);
            if (status == ResourceLoader.ThreadLoadStatus.InvalidResource) _loadingScreen.TweenProgressBar(100, FinishLoadingScene);
        }

        _loadingScreen.TweenProgressBar(percentage);
    }

    private void InstantiatePackedScene()
    {
        GD.Print($"InstantiateScene()");

        PackedScene scene = ResourceLoader.LoadThreadedGet(_loadDir) as PackedScene;
        _currentNode = scene.Instantiate();
        FinishLoadingScene();
    }

    private void FinishLoadingScene()
    {
        GetTree().Root.AddChild(_currentNode);
        GD.Print($"free loading screen");
        _loadingScreen.QueueFree();
    }

    public void ChangeScene(EnumScenes scene)
    {
        GD.Print($"ChangeScene()");

        // GD.Print($"_isloading: {_isloading}");
        if (_isloading) return;

        _isloading = true;

        GD.Print($"creating loading screen");
        _loadingScreen = ResourcePaths.GetSceneInstanceFromEnum<LoadingScreen>(ResourcePaths.ScenePathsEnum.LOADING_SCREEN);
        AddChild(_loadingScreen);
        // GetTree().Root.CallDeferred("add_child", _loadingScreen);

        GD.Print($"free current node {_currentNode.Name}");
        _currentNode?.QueueFree();

        switch (scene)
        {
            case EnumScenes.MAIN_MENU:
                LoadPackedScene(EnumScenes.MAIN_MENU);
                break;

            case EnumScenes.MAIN_GAME:
                _currentNode = new MainGame() { TextureFilter = CanvasItem.TextureFilterEnum.Nearest };
                break;

            case EnumScenes.TEST_SCENARIO:
                _currentNode = new TestScenario() { TextureFilter = CanvasItem.TextureFilterEnum.Nearest };
                break;
        }
    }

    private void LoadPackedScene(EnumScenes scene)
    {
        ResourcePaths.ScenePathsEnum pathKey = scene switch
        {
            EnumScenes.MAIN_MENU => ResourcePaths.ScenePathsEnum.MAIN_MENU,
            _ => throw new NotImplementedException($"Cant load packed scene {scene}"),
        };

        _loadDir = ResourcePaths.ScenePaths[pathKey];

        GD.Print($"request load {scene} packed scene");
        ResourceLoader.LoadThreadedRequest(_loadDir);
    }

    public void OpenOptionsMenu()
    {
        OptionsMenu menu = ResourcePaths.GetSceneInstanceFromEnum<OptionsMenu>(ResourcePaths.ScenePathsEnum.OPTIONS_MENU);
        _currentNode.AddChild(menu);
    }
}