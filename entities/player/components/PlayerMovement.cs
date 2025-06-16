using Godot;
using Godot.Collections;

public partial class PlayerMovement : BaseComponent<PlayerScene>
{
    public int Speed = 50;
    public int PlayerSize = 5;
    public CollisionShape2D HurtboxShape;
    public Vector2 FacingDirection = Vector2.Right;

    public PlayerMovement(PlayerScene player) : base(player)
    {
        Parent.CollisionLayer = 1; // 1 = player layer
        Parent.CollisionMask = 3; // 1 = player layer, 2 = enemy layer

        // create hurtbox
        HurtboxShape = new CollisionShape2D() { Shape = new CircleShape2D() { Radius = PlayerSize } };
        Parent.AddChild(HurtboxShape);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!Parent.PlayerHealth.Alive) return;

        // check collisions
        PhysicsShapeQueryParameters2D query = new()
        {
            CollideWithBodies = true,
            CollideWithAreas = false,
            Shape = HurtboxShape.Shape,
            Transform = new Transform2D(0, Parent.Position),
            CollisionMask = 2 // 2 = enemy layer
        };

        Array<Dictionary> collisions = GetWorld2D().DirectSpaceState.IntersectShape(query, 16);
        foreach (Dictionary col in collisions) { Parent.PlayerHealth.OnCollision(col); }

        Vector2 inputDirection = GetInputVector();
        if (inputDirection == Vector2.Zero) return;

        Vector2 offset = new(
            (float)(inputDirection.X * Speed * delta),
            (float)(inputDirection.Y * Speed * delta)
        );

        Parent.Position += offset;
        FacingDirection = inputDirection;

        Parent.PlayerStats.IncreaseDistanceTraveled(offset.Length());
        Parent.DebugTryUpdateField("player_pos", Parent.Position.ToString("0.0"));
    }

    public static Vector2 GetInputVector()
    {
        Vector2 inputDirection = Input.GetVector("left", "right", "up", "down");
        return inputDirection;
    }

    // -------------------------------------------- Debug --------------------------------------------
    public override void DebugCreateSubCategory(DebugCategory category)
    {
        category.CreateDivider("Movement");
        category.CreateLabelField("player_pos", "Pos", Parent.Position.ToString("0.0"));
        category.CreateLabelField("player_speed", "Speed", Speed.ToString());
    }
}