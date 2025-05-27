using Godot;
using Godot.Collections;

public partial class PlayerMovement(PlayerScene player) : Node2D
{
    private PlayerScene _player = player;

    public override void _PhysicsProcess(double delta)
    {
        // move player
        Vector2 inputDirection = GetInputVector();
        if (inputDirection.Equals(Vector2.Zero)) return;

        Vector2 offset = new(
            (float)(inputDirection.X * _player.Speed * delta),
            (float)(inputDirection.Y * _player.Speed * delta)
        );

        _player.Position += offset;
        _player.EmitSignal(PlayerScene.SignalName.PlayerMove);

        // check collisions
        PhysicsShapeQueryParameters2D query = new()
        {
            CollideWithBodies = true,
            CollideWithAreas = false,
            Shape = _player.HurtboxShape.Shape,
            Motion = offset,
            Transform = new Transform2D(0, _player.Position),
            CollisionMask = 2 // 2 = enemy layer
        };

        Array<Dictionary> collisions = GetWorld2D().DirectSpaceState.IntersectShape(query, 16);
        foreach (Dictionary col in collisions) { _player.OnCollision(col); }
    }

    public static Vector2 GetInputVector()
    {
        Vector2 inputDirection = Input.GetVector("left", "right", "up", "down");
        return inputDirection;
    }
}