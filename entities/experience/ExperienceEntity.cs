using Godot;

public partial class ExperienceEntity
{
    public Vector2 Position;
    public int Amount;
    public bool Rendered = false;

    public ExperienceEntity(Vector2 pos, int amount)
    {
        Position = pos;
        Amount = amount;
    }

    public Rid CanvasItemRid;
    public Rid AreaRid;
}