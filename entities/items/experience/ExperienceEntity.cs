using Godot;

public class ExperienceEntity
{
    public Vector2 Position;
    public int Amount;

    public ExperienceEntity(Vector2 pos, int amount)
    {
        Position = pos;
        Amount = amount;
    }

    public Rid CanvasItemRid;
    public Rid AreaRid;
}