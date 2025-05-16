using Godot;
using System;

public partial class MainGame : Node2D
{
    [Export]
    public PackedScene EnemyScene;

    [Export]
    private CharacterBody2D _player;

    [Export]
    private Timer _spawnTimer;

    public override void _Ready()
    {
        GameManager.Instance.Player = _player;
        _spawnTimer.Timeout += SpawnEnemy;
    }

    public void SpawnEnemy()
    {
        Vector2 pos = new(0, 0);

        Node2D enemy = EnemyScene.Instantiate<Node2D>();
        enemy.Position = pos;

        AddChild(enemy);
    }
}
