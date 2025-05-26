using Godot;

public class BasicEnemy
{
    public BasicEnemy(Vector2 pos, int health, int speed, int damage)
    {
        Position = pos;
        Health = health;
        Speed = speed;
        Damage = damage;
    }

    public Vector2 Position;

    public int Health;
    public int Speed;
    public int Damage;

    public Rid SpriteRid;
    public Rid BodyRid;
    public Rid HurtboxRid;
}