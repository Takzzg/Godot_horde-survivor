using System.Collections.Generic;
using Godot;

public partial class ExperienceManager : Node2D
{
    private readonly List<ExperienceEntity> _entitiesQueued = [];
    private readonly List<ExperienceEntity> _entitiesList = [];
    public int ExperienceEntitySize = 2;
    public Shape2D SharedShape;

    public ExperienceManager()
    {
        SharedShape = new CircleShape2D() { Radius = ExperienceEntitySize };
        GameManager.Instance.Player.PlayerMovement.PlayerMove += RenderExperienceEntities;
    }

    public override void _PhysicsProcess(double delta)
    {
        // spawn entities
        _entitiesQueued.ForEach((entity) =>
        {
            RenderExperienceEntity(entity);
            _entitiesList.Add(entity);
        });
        _entitiesQueued.Clear();
    }

    private void RenderExperienceEntities()
    {
        Vector2 playerPos = GameManager.Instance.Player.Position;

        _entitiesList.ForEach((entity) =>
        {
            if (entity.Position.DistanceTo(playerPos) > GameManager.RENDER_DISTANCE) { FreeExperienceEntityRids(entity); }
            else { RenderExperienceEntity(entity); }
        });
    }

    public void QueueExperienceEntitySpawn(Vector2 pos, int amount)
    {
        ExperienceEntity entity = new(pos, amount);
        _entitiesQueued.Add(entity);
    }

    private void RenderExperienceEntity(ExperienceEntity entity)
    {
        if (entity.Rendered) return;

        entity.Rendered = true;
        Transform2D posTransform = new(0, entity.Position);

        // create area
        entity.AreaRid = PhysicsServer2D.AreaCreate();
        PhysicsServer2D.AreaAddShape(entity.AreaRid, SharedShape.GetRid(), posTransform);
        PhysicsServer2D.AreaSetSpace(entity.AreaRid, GetWorld2D().Space);
        PhysicsServer2D.AreaSetCollisionLayer(entity.AreaRid, 16); // 16 = entities layer
        // PhysicsServer2D.AreaSetCollisionMask(entity.AreaRid, 16); // 16 = entities layer
        PhysicsServer2D.AreaSetMonitorable(entity.AreaRid, true);

        // create canvas entity
        entity.CanvasItemRid = RenderingServer.CanvasItemCreate();
        RenderingServer.CanvasItemSetParent(entity.CanvasItemRid, GetCanvasItem());
        RenderingServer.CanvasItemSetTransform(entity.CanvasItemRid, posTransform);
        RenderingServer.CanvasItemAddCircle(entity.CanvasItemRid, Vector2.Zero, ExperienceEntitySize, new Color(0.125f, 0.125f, 0.125f, 1f));
    }

    private void FreeExperienceEntityRids(ExperienceEntity entity)
    {
        if (!entity.Rendered) return;

        entity.Rendered = false;
        PhysicsServer2D.FreeRid(entity.AreaRid);
        RenderingServer.FreeRid(entity.CanvasItemRid);
    }

    public void DestroyExperienceEntity(ExperienceEntity entity)
    {
        FreeExperienceEntityRids(entity);
        _entitiesList.Remove(entity);
    }

    public ExperienceEntity FindExperienceEntityFromAreaRid(Rid areaRid)
    {
        ExperienceEntity entity = _entitiesList.Find((i) => i.AreaRid == areaRid);
        return entity;
    }
}