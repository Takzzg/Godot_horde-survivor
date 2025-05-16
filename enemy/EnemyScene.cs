using Godot;
using System;

public partial class EnemyScene : CharacterBody2D
{
    [Export]
    public int Speed = 50;

    [Export]
    private NavigationAgent2D _nav;

    public override void _Ready()
    {
        _nav.TargetPosition = new Vector2(0, 999);
    }

    public override void _PhysicsProcess(double delta)
    {
        // if (_nav.IsNavigationFinished()) return;
        _nav.TargetPosition = GameManager.Instance.Player.Position;

        Vector2 currentAgentPosition = GlobalTransform.Origin;
        Vector2 nextPathPosition = _nav.GetNextPathPosition();

        Velocity = currentAgentPosition.DirectionTo(nextPathPosition) * Speed;
        MoveAndSlide();
    }
}
