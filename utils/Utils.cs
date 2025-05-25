using Godot;
using System;

public partial class Utils : Node
{
    public static Vector2 GetRandomPointOnCircle(Vector2 center, float radius)
    {
        float angle = GameManager.Instance.RNG.RandiRange(0, 359);
        Vector2 point = new(
            center.X + radius * (float)Math.Cos(angle),
            center.Y + radius * (float)Math.Sin(angle)
        );
        return point;
    }
}
