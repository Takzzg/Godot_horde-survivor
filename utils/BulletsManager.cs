using Godot;
using Godot.Collections;

public partial class BulletsManager : Node2D
{
    public static BasicBullet SpawnBullet(Vector2 pos, Vector2 dir, double spd)
    {
        BasicBullet bullet = new()
        {
            Position = pos,
            Speed = spd,
            Direction = dir,
        };
        return bullet;
    }

    public static void MoveBullet(BasicBullet bullet, double delta)
    {
        Vector2 offset = new(
            (float)(bullet.Direction.X * bullet.Speed * delta),
            (float)(bullet.Direction.Y * bullet.Speed * delta)
        );

        bullet.Position += offset;
        bullet.LifeTime += delta;
    }

    public void DrawBullet(Texture2D tex, Vector2 pos)
    {
        Vector2 offset = tex.GetSize() / 2;
        DrawTexture(tex, pos - offset);
    }

    public void CheckCollision(BasicBullet bullet, Resource shape)
    {
        PhysicsShapeQueryParameters2D query = new()
        {
            Shape = shape,
            Transform = new Transform2D(0, bullet.Position),
            CollideWithAreas = true,
            CollideWithBodies = false,
        };

        var result = GetWorld2D().DirectSpaceState.IntersectShape(query);
        foreach (Dictionary dic in result) { GD.Print($"dic: {dic}"); }
    }
}