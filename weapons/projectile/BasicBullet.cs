using System.Collections.Generic;
using Godot;

public class BasicBullet
{
    public int Damage;
    public Vector2 Position;
    public int Speed;
    public Vector2 Direction;
    public double LifeTime;
    public int MaxDistance;
    public int MaxLifeTime;
    public int PierceCount;

    public Texture2D ImageOffset;
    public double AnimationLifeTime;

    public Rid CanvasItemRid;
    public Rid AreaRid;

    public List<Rid> CollidingWith = [];
}