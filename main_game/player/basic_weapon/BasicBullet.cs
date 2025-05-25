using Godot;

public class BasicBullet
{
    public Vector2 Position;
    public int Speed;
    public Vector2 Direction;
    public double LifeTime;
    public int MaxDistance;
    public int MaxLifeTime;

    public Texture2D ImageOffset;
    public double AnimationLifeTime;

    public Rid BodyRid;
    public Rid SpriteRid;
    public Rid AreaRid;
}