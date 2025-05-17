using Godot;
using System;

public partial class EnemyScene : CharacterBody2D
{
    [Export]
    public int Health = 50;
    [Export]
    public int Speed = 50;
    [Export]
    public int Damage = 5;

    [Export]
    private NavigationAgent2D _nav;

    private void OnDealDamage(Node2D body)
    {
        GD.Print("Enemy collided with ", body.Name);
        GameManager.Instance.Player.EmitSignal(PlayerScene.SignalName.PlayerReceiveDamage, Damage);
    }

    public override void _PhysicsProcess(double delta)
    {
        // if (_nav.IsNavigationFinished()) return;
        GD.Print(GameManager.Instance.Player);
        if (GameManager.Instance.Player == null) return;
        _nav.TargetPosition = GameManager.Instance.Player.Position;

        Vector2 currentAgentPosition = GlobalTransform.Origin;
        Vector2 nextPathPosition = _nav.GetNextPathPosition();

        Velocity = currentAgentPosition.DirectionTo(nextPathPosition) * Speed;
        MoveAndSlide();
    }
}
