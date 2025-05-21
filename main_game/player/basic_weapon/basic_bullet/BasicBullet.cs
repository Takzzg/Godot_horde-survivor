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

    private Vector2 _direction;

    public override void _Ready()
    {
        // get travel direction
        _direction = GameManager.Instance.Player.GlobalPosition.DirectionTo(GlobalPosition).Normalized();
    }

    public override void _PhysicsProcess(double delta)
    {
        // move bullet
        GlobalPosition = new Vector2(
            (float)(GlobalPosition.X + _direction.X * Speed * delta),
            (float)(GlobalPosition.Y + _direction.Y * Speed * delta)
        );

        // destroy bullet if too far
        if (GlobalPosition.DistanceTo(GameManager.Instance.Player.GlobalPosition) > GameManager.RENDER_DISTANCE)
            QueueFree();
    }

    public int DealDamage()
    {
        // destroy bullet if collided
        if (Pierce <= 0) QueueFree();
        else Pierce -= 1;

        return Damage;
    }
}
