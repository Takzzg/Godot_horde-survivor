using System.Collections.Generic;
using Godot;

public partial class ExperienceManager : Node2D
{
    public readonly List<ExperienceEntity> ExperienceItemsList = [];
    public int ExperienceItemSize = 2;
    public Shape2D SharedShape;

    public ExperienceManager()
    {
        SharedShape = new CircleShape2D() { Radius = ExperienceItemSize };
    }

    public void SpawnExperienceItem(Vector2 pos)
    {
        ExperienceEntity item = new(pos, 5);
        Transform2D posTransform = new(0, pos);

        // create area
        item.AreaRid = PhysicsServer2D.AreaCreate();
        PhysicsServer2D.AreaAddShape(item.AreaRid, SharedShape.GetRid(), posTransform);
        PhysicsServer2D.AreaSetSpace(item.AreaRid, GetWorld2D().Space);
        PhysicsServer2D.AreaSetCollisionLayer(item.AreaRid, 16); // 16 = items layer
        // PhysicsServer2D.AreaSetMonitorable(item.AreaRid, true);

        // create canvas item
        item.CanvasItemRid = RenderingServer.CanvasItemCreate();
        RenderingServer.CanvasItemSetParent(item.CanvasItemRid, GetCanvasItem());
        RenderingServer.CanvasItemSetTransform(item.CanvasItemRid, posTransform);
        RenderingServer.CanvasItemAddCircle(item.CanvasItemRid, Vector2.Zero, ExperienceItemSize, new Color(0.125f, 0.125f, 0.125f, 1f));
    }
}