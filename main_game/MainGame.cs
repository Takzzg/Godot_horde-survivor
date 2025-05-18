using Godot;
using System;

public partial class MainGame : Node2D
{
    [Export]
    private PackedScene _enemyScene;
    [Export]
    private PackedScene _playerScene;
    [Export]
    private MainUI _mainUI;

    [Export]
    private Timer _spawnTimer;
    [Export]
    private Node2D _enemiesContainer;

    public override void _Ready()
    {
        GD.Print($"MainGame ready!");

        // create player
        PlayerScene player = _playerScene.Instantiate<PlayerScene>();
        player.Name = "PlayerNode";
        player.PlayerDeath += _spawnTimer.Stop;

        AddChild(player);

        // bind spawner timer
        // GD.Print($"creating enemy spawner");
        _spawnTimer.Timeout += SpawnEnemy;

        // save node refs to singleton
        GameManager.Instance.Player = player;
        GameManager.Instance.UI = _mainUI;

        player.SetUpSignals();
        _mainUI.SetUpSignals();
    }

    private void SpawnEnemy()
    {
        GD.Print($"spawning enemy");
        EnemyScene enemy = _enemyScene.Instantiate<EnemyScene>();

        Vector2 pos = new(0, 0);
        enemy.Position = pos;
        enemy.Name = "Enemy" + Time.GetUnixTimeFromSystem();

        _enemiesContainer.AddChild(enemy);
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
