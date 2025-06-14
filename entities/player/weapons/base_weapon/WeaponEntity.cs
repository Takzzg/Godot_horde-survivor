using Godot;
using Godot.Collections;

public class WeaponEntity
{
    public float Damage;
    public float Speed;
    public float Radius;

    public double LifeTime;
    public Vector2 Position;
    public Vector2 Offset;
    public Vector2 Direction;

    public int MaxDistance = GameManager.RENDER_DISTANCE;
    public int MaxPierceCount;
    public double MaxLifeTime;

    public bool Foreground = true;

    public Rid CanvasItemRid;

    public Dictionary<Rid, double> CollidingWith = [];
}