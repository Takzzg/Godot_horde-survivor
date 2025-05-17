using Godot;
using System;

public partial class MainGame : Node2D
{
    [Export]
    private PackedScene _enemyScene;
    [Export]
    private PackedScene _playerScene;
    [Export]
    private PackedScene _uiScene;

    private Timer _spawnTimer;

    public double GetCurrentTimeStamp()
    {
        return Time.GetUnixTimeFromSystem();
    }

    public override void _Ready()
    {
        GD.Print($"MainGame ready!");

        // create player
        GD.Print($"creating player");
        GameManager.Instance.Player = _playerScene.Instantiate<PlayerScene>();
        GameManager.Instance.Player.Name = "PlayerNode";
        AddChild(GameManager.Instance.Player);

        // create spawner
        GD.Print($"creating enemy spawner");
        _spawnTimer = new Timer() { Autostart = true, OneShot = false, WaitTime = 2 };
        _spawnTimer.Timeout += SpawnEnemy;
        AddChild(_spawnTimer);

        // create UI
        GD.Print($"creating ui");
        GameManager.Instance.UI = _uiScene.Instantiate<MainUi>();
        GameManager.Instance.UI.UpdateHealthLabel(GameManager.Instance.Player.Health);
        CanvasLayer layer = new();
        layer.AddChild(GameManager.Instance.UI);
        AddChild(layer);
    }

    public void SpawnEnemy()
    {
        GD.Print($"spawning enemy");
        EnemyScene enemy = _enemyScene.Instantiate<EnemyScene>();

        Vector2 pos = new(0, 0);
        enemy.Position = pos;
        enemy.Name = "Enemy" + GetCurrentTimeStamp();

        AddChild(enemy);
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionPressed("open_menu"))
        {
            GD.Print($"open_menu key pressed");
            SceneManager.Instance.ChangeScene(SceneManager.SceneEnum.MAIN_MENU);
        }
    }

}
