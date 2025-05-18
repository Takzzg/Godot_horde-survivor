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
    [Export]
    private Area2D _hitBox;

    private Timer _damageTimer;
    private double _damageInterval = 0.5;

    public override void _Ready()
    {
        _hitBox.BodyEntered += OnPlayerBodyEntered;
        _hitBox.BodyExited += OnPlayerBodyExited;

        GameManager.Instance.Player.PlayerDeath += OnPlayerDeath;
    }

    public override void _ExitTree()
    {
        GameManager.Instance.Player.PlayerDeath -= OnPlayerDeath;
    }

    public void ReceiveDamage(int amount)
    {
        Health -= amount;
        if (Health > 0) return;
        QueueFree();
    }

    private void OnPlayerBodyEntered(Node2D body)
    {
        DealDamage();
        _damageTimer = new Timer() { Autostart = true, OneShot = false, WaitTime = _damageInterval };
        _damageTimer.Timeout += DealDamage;
        AddChild(_damageTimer);
    }

    public void OnPlayerBodyExited(Node2D body)
    {
        _damageTimer?.QueueFree();
    }

    private void DealDamage()
    {
        GameManager.Instance.Player.EmitSignal(PlayerScene.SignalName.PlayerReceiveDamage, Damage);
    }

    private void OnPlayerDeath()
    {
        RandomNumberGenerator rng = new();
        Timer delay = new() { Autostart = true, WaitTime = rng.RandfRange(0.5f, 1.5f) };
        delay.Timeout += QueueFree;
        AddChild(delay);
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 target = GameManager.Instance.Player.Position;
        Velocity = Position.DirectionTo(target) * Speed;
        MoveAndSlide();
    }
}
