using Godot;
using System;

public partial class BasicBullet : Node2D
{
    [Export]
    public double Speed = 100;
    [Export]
    public int Damage = 25;
    [Export]
    public int Pierce = 1;
    [Export]
    public Area2D HitBox;

    private Vector2 _direction;

    public override void _Ready()
    {
        _direction = GameManager.Instance.Player.GlobalPosition.DirectionTo(GlobalPosition).Normalized();
        HitBox.AreaEntered += OnHitEnemy;
    }

    private void OnHitEnemy(Area2D area)
    {
        EnemyScene enemy = area.GetParent<EnemyScene>();
        // GD.Print($"enemy.Name: {enemy.Name}, area.Name: {area.Name}");
        enemy.ReceiveDamage(Damage);

        if (Pierce == 0) QueueFree();
        else Pierce -= 1;
    }

    public override void _Process(double delta)
    {
        GlobalPosition = new Vector2(
            (float)(GlobalPosition.X + (_direction.X * Speed * delta)),
            (float)(GlobalPosition.Y + (_direction.Y * Speed * delta))
        );

        if (GlobalPosition.DistanceTo(GameManager.Instance.Player.GlobalPosition) > 200) QueueFree();
    }
}
