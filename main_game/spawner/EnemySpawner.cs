using Godot;
using System;

public partial class EnemySpawner : Node2D
{
    // [Export]
    // private int _spawnRadius = 100;

    [Export]
    private Timer _spawnTimer;
    [Export]
    public Node2D EnemiesContainer;
    [Export]
    private PackedScene _enemyScene;

    private RandomNumberGenerator rng = new();

    public override void _Ready()
    {
        _spawnTimer.Timeout += SpawnEnemy;
    }

    public void SetUpSignals()
    {
        GameManager.Instance.MainGame.Player.PlayerDeath += () =>
        {
            GD.Print($"EnemySpawner OnPLayerDeath()");
            _spawnTimer.Stop();
        };
    }

    private void SpawnEnemy()
    {
        // skip if too many enemies
        // if (EnemiesContainer.GetChildCount() > 400) return;

        // GD.Print($"spawning enemy");
        EnemyScene enemy = _enemyScene.Instantiate<EnemyScene>();
        enemy.Position = Utils.GetRandomPointOnCircle(GameManager.Instance.MainGame.Player.Position, GameManager.RENDER_DISTANCE);
        enemy.Name = "Enemy" + Time.GetUnixTimeFromSystem();
        EnemiesContainer.AddChild(enemy);

        // update ui
        GameManager.Instance.MainGame.MainUI.GameplayUI.UpdateEnemiesCountLabel();
    }
}
