using Godot;
using System;

public partial class EnemyScene : Node2D
{
    [Export]
    public int Health = 50;
    [Export]
    public int Speed = 50;
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
        _nav.VelocityComputed += OnVelocityComputed;
        _hitBox.BodyEntered += OnBodyEntered;
        _hitBox.BodyExited += OnBodyExited;

        GameManager.Instance.Player.PlayerDeath += OnPlayerDeath;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body.Name != "PlayerNode") return;
        DealDamage();
        _damageTimer = new Timer() { Autostart = true, OneShot = false, WaitTime = _damageInterval };
        _damageTimer.Timeout += DealDamage;
        AddChild(_damageTimer);
    }

    public void OnBodyExited(Node2D body)
    {
        if (body.Name != "PlayerNode") return;
        _damageTimer.QueueFree();
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

    // -------------------------------------------- Navigation --------------------------------------------
    [Export]
    private NavigationAgent2D _nav;
    private float _movementDelta;

    private void OnVelocityComputed(Vector2 safeVelocity)
    {
        GlobalPosition = GlobalPosition.MoveToward(GlobalPosition + safeVelocity, _movementDelta);
    }

    private void SetMovementTarget(Vector2 movementTarget)
    {
        _nav.TargetPosition = movementTarget;
    }

    public override void _PhysicsProcess(double delta)
    {
        // Do not query when the map has never synchronized and is empty.
        if (NavigationServer2D.MapGetIterationId(_nav.GetNavigationMap()) == 0) return;

        // if (_nav.IsNavigationFinished()) return;
        if (GameManager.Instance.Player == null) return;
        _nav.TargetPosition = GameManager.Instance.Player.Position;

        _movementDelta = Speed * (float)delta;
        Vector2 nextPathPosition = _nav.GetNextPathPosition();
        Vector2 newVelocity = GlobalPosition.DirectionTo(nextPathPosition) * _movementDelta;

        if (_nav.AvoidanceEnabled) _nav.Velocity = newVelocity;
        else OnVelocityComputed(newVelocity);
    }
}
