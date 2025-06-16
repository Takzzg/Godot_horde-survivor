using Godot;

public partial class PlayerDraw : BaseComponent<PlayerScene>
{
    private Rid _canvasItemRid;

    public PlayerDraw(PlayerScene player) : base(player)
    {
        Transform2D posTransform = new(0, Position);

        _canvasItemRid = RenderingServer.CanvasItemCreate();
        RenderingServer.CanvasItemSetParent(_canvasItemRid, GetCanvasItem());
        RenderingServer.CanvasItemSetZIndex(_canvasItemRid, 20);
        RenderingServer.CanvasItemSetTransform(_canvasItemRid, posTransform);
        RenderingServer.CanvasItemAddCircle(_canvasItemRid, Vector2.Zero, 5, Colors.SlateGray);
    }
}