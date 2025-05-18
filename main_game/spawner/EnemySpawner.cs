using Godot;
using System;

public partial class EnemySpawner : Node2D
{
    [Export]
    private int _spawnRadius = 100;

    [Export]
    private Timer _spawnTimer;
    [Export]
    private Node2D _enemiesContainer;
    [Export]
    private PackedScene _enemyScene;

    private RandomNumberGenerator rng = new();

    public override void _Ready()
    {
        _spawnTimer.Timeout += SpawnEnemy;
    }

    public void SetUpSignals()
    {
        GameManager.Instance.Player.PlayerDeath += _spawnTimer.Stop;
    }

    private void SpawnEnemy()
    {
        // GD.Print($"spawning enemy");

        float angle = rng.RandiRange(0, 359);
        Vector2 player = GameManager.Instance.Player.Position;
        Vector2 target = new(
            player.X + _spawnRadius * (float)Math.Cos(angle),
            player.Y + _spawnRadius * (float)Math.Sin(angle)
        );

        EnemyScene enemy = _enemyScene.Instantiate<EnemyScene>();
        enemy.Position = target;
        enemy.Name = "Enemy" + Time.GetUnixTimeFromSystem();

        _enemiesContainer.AddChild(enemy);
    }
}
