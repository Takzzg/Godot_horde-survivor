using System.Collections.Generic;
using Godot;

public class WeaponEntity
{
    public int Damage;
    public int Speed;
    public int Radius;

    public double LifeTime;
    public Vector2 Position;
    public Vector2 Offset;
    public Vector2 Direction;

    public int MaxDistance = GameManager.RENDER_DISTANCE;
    public int MaxPierceCount = 0;
    public double MaxLifeTime = 10;

    public Rid CanvasItemRid;

    public List<Rid> CollidingWith = [];
}