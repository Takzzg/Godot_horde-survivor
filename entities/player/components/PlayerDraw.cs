using Godot;

public partial class PlayerDraw : BasePlayerComponent
{
    private Rid _canvasItemRid;

    public PlayerDraw(PlayerScene player) : base(player, false)
    {
        Transform2D posTransform = new(0, Position);

        _canvasItemRid = RenderingServer.CanvasItemCreate();
        RenderingServer.CanvasItemSetParent(_canvasItemRid, GetCanvasItem());
        RenderingServer.CanvasItemSetTransform(_canvasItemRid, posTransform);
        RenderingServer.CanvasItemAddCircle(_canvasItemRid, Vector2.Zero, 5, Colors.SlateGray);
    }
}