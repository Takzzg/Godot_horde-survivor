using Godot;
using System;

public partial class EnemyScene : CharacterBody2D
{
    [Export]
    public int Health = 50;
    [Export]
    public int Speed = 25;
    [Export]
    public int Damage = 5;
    [Export]
    private Area2D _hurtBox;
    // [Export]
    // private Area2D _hitBox;
    [Export]
    private Timer _damageTimer;

    public override void _Ready()
    {
        _hurtBox.BodyEntered += OnBulletHit;
        // _hitBox.BodyEntered += OnBodyEntered;
        // GameManager.Instance.MainGame.Player.PlayerDeath += OnPlayerDeath;

        RandomNumberGenerator rng = GameManager.Instance.RNG;
        Speed += rng.RandiRange(-5, 5);
    }

    public override void _ExitTree()
    {
        GameManager.Instance.MainGame.Player.PlayerDeath -= OnPlayerDeath;
        GameManager.Instance.MainGame.MainUI.GameplayUI.UpdateEnemiesCountLabel();
    }

    public override void _PhysicsProcess(double delta)
    {
        MoveEnemy(delta);
    }

    private void OnBulletHit(Node2D body)
    {
        // BasicBullet bullet = body as BasicBullet;

        // int damage = bullet.DealDamage();
        // ReceiveDamage(damage);
    }

    public void ReceiveDamage(int amount)
    {
        Health -= amount;
        if (Health > 0) return;
        QueueFree();
    }

    // private void OnBodyEntered(Node2D body)
    // {
    //     // if (!body.IsInGroup("player")) return;

    //     DealDamage(body as PlayerScene);
    //     GD.Print($"player collision with {Name}");
    // }

    private void DealDamage(PlayerScene player)
    {
        if (!_damageTimer.IsStopped()) return;

        player.EmitSignal(PlayerScene.SignalName.PlayerReceiveDamage, Damage);
        _damageTimer.Start();
    }

    private void OnPlayerDeath()
    {
        // despawn after random delay
        RandomNumberGenerator rng = new();
        Timer delay = new() { Autostart = true, WaitTime = rng.RandfRange(0.5f, 1.5f) };
        delay.Timeout += QueueFree;
        AddChild(delay);
    }

    private void MoveEnemy(double delta)
    {
        // // destroy enemy if too far
        // if (Position.DistanceTo(GameManager.Instance.Player.Position) > GameManager.RENDER_DISTANCE * 2)
        // {
        //     QueueFree();
        //     return;
        // }

        // Vector2 target = GameManager.Instance.Player.Position;
        // var velocity = Position.DirectionTo(target) * Speed;

        // Position = new Vector2(
        //     (float)(Position.X + velocity.X * delta),
        //     (float)(Position.Y + velocity.Y * delta)
        // );
    }
}
