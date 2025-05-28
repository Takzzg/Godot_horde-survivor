using Godot;
using Godot.Collections;

public partial class PlayerMovement : BasePlayerComponent
{
    public int Speed = 50;
    public int PlayerSize = 5;
    public CollisionShape2D HurtboxShape;

    [Signal]
    public delegate void PlayerMoveEventHandler();

    public PlayerMovement(PlayerScene player) : base(player)
    {
        _player.CollisionLayer = 1; // 1 = player layer
        _player.CollisionMask = 3; // 1 = player layer, 2 = enemy layer

        // create hurtbox
        HurtboxShape = new CollisionShape2D() { Shape = new CircleShape2D() { Radius = PlayerSize } };
        _player.AddChild(HurtboxShape);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!_player.PlayerHealth.Alive) return;

        // check collisions
        PhysicsShapeQueryParameters2D query = new()
        {
            CollideWithBodies = true,
            CollideWithAreas = false,
            Shape = HurtboxShape.Shape,
            Transform = new Transform2D(0, _player.Position),
            CollisionMask = 2 // 2 = enemy layer
        };

        Array<Dictionary> collisions = GetWorld2D().DirectSpaceState.IntersectShape(query, 16);
        foreach (Dictionary col in collisions) { _player.PlayerHealth.OnCollision(col); }

        // move player
        Vector2 inputDirection = GetInputVector();
        if (inputDirection.Equals(Vector2.Zero)) return;

        Vector2 offset = new(
            (float)(inputDirection.X * Speed * delta),
            (float)(inputDirection.Y * Speed * delta)
        );

        _player.Position += offset;
        _player.PlayerStats.IncreaseDistanceTraveled(offset.Length());
        EmitSignal(SignalName.PlayerMove);
    }

    public static Vector2 GetInputVector()
    {
        Vector2 inputDirection = Input.GetVector("left", "right", "up", "down");
        return inputDirection;
    }
}