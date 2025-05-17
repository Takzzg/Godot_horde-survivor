using System;
using System.Linq;
using Godot;

public partial class SceneManager : Node
{
    // singleton
    public static SceneManager Instance { get; private set; }
    public override void _Ready()
    {
        GD.Print($"SceneManager singleton ready!");
        Instance = this;
        _currentNode = GetTree().Root.GetChildren().Last();
        GD.Print($"_currentNode: {_currentNode.Name}");
    }

    // packed scene paths
    public enum SceneEnum { MAIN_MENU, MAIN_GAME }
    private string _mainMenuScenePath = "res://main_menu/main_menu.tscn";
    private string _mainGameScenePath = "res://main_game/main_game.tscn";
    private const string _loadingScreenScenePath = "res://loading_screen/loading_screen.tscn";

    // resource loader
    private PackedScene _loadingScreenScene = ResourceLoader.Load<PackedScene>(_loadingScreenScenePath);
    private LoadingScreen _loadingScreen;
    private string _loadDir;
    private Node _currentNode;
    private bool _isloading;

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

        if (status == ResourceLoader.ThreadLoadStatus.Loaded)
        {
            if (_loadingScreen.Tween != null)
            {
                // GD.Print($"tween running: {_loadingScreen.Tween.IsRunning()}");
                if (_loadingScreen.Tween.IsRunning()) return;
            }

            GD.Print($"creating final tween");
            _isloading = false;
            _loadingScreen.TweenProgressBar(percentage, InstantiateScene);
        }

        _loadingScreen.TweenProgressBar(percentage);
    }

    private void InstantiateScene()
    {
        GD.Print($"InstantiateScene()");

        GD.Print($"creating delay");
        Timer delay = new() { Autostart = true, OneShot = true, WaitTime = 0.5 };
        GetTree().Root.AddChild(delay);
        delay.Timeout += () =>
        {
            PackedScene scene = ResourceLoader.LoadThreadedGet(_loadDir) as PackedScene;
            _currentNode = scene.Instantiate();
            GetTree().Root.AddChild(_currentNode);

            _loadingScreen.QueueFree();
            delay.QueueFree();
        };
    }

    public void ChangeScene(SceneEnum scene)
    {
        GD.Print($"ChangeScene()");

        GD.Print($"_isloading: {_isloading}");
        if (_isloading) return;

        _isloading = true;

        GD.Print($"creating loading screen");
        _loadingScreen = _loadingScreenScene.Instantiate<LoadingScreen>();
        GetTree().Root.CallDeferred("add_child", _loadingScreen);

        _loadDir = scene switch
        {
            SceneEnum.MAIN_MENU => _mainMenuScenePath,
            SceneEnum.MAIN_GAME => _mainGameScenePath,
            _ => throw new NotImplementedException(),
        };

        GD.Print($"request load {scene} packed scene");
        ResourceLoader.LoadThreadedRequest(_loadDir);

        GD.Print($"free current node");
        _currentNode?.QueueFree();
    }
}