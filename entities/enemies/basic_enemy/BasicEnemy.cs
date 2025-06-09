using Godot;

public class BasicEnemy
{
    public BasicEnemy(Vector2 pos, int health, int speed, int damage, int xp)
    {
        Position = pos;
        Health = health;
        Speed = speed;
        Damage = damage;
        ExperienceDropped = xp;
    }

    public Vector2 Position;

    public float Health;
    public int Speed;
    public int Damage;
    public int ExperienceDropped;

    public Rid CanvasItemRid;
    public Rid BodyRid;
    public Rid HurtboxRid;
}