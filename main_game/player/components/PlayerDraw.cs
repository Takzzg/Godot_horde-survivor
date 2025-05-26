using Godot;

public partial class PlayerDraw : Node2D
{
    private Rid _canvasItemRid;

    public PlayerDraw()
    {
        Transform2D posTransform = new(0, Position);

        _canvasItemRid = RenderingServer.CanvasItemCreate();
        RenderingServer.CanvasItemSetParent(_canvasItemRid, GetCanvasItem());
        RenderingServer.CanvasItemSetTransform(_canvasItemRid, posTransform);
        RenderingServer.CanvasItemAddCircle(_canvasItemRid, Vector2.Zero, 5, Colors.SlateGray);
    }
}