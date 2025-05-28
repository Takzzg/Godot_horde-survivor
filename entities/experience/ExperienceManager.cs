using System.Collections.Generic;
using Godot;

public partial class ExperienceManager : Node2D
{
    // entities
    public int EntitiesCount => _entitiesList.Count;
    private int _renderedEntitiesCount = 0;
    private readonly List<ExperienceEntity> _entitiesQueued = [];
    private readonly List<ExperienceEntity> _entitiesList = [];

    // shared resources
    public int ExperienceEntitySize = 2;
    public Shape2D SharedShape;

    // debug
    private DebugCategoryComponent _debug;

    public ExperienceManager()
    {
        SharedShape = new CircleShape2D() { Radius = ExperienceEntitySize };
        GameManager.Instance.Player.PlayerMovement.PlayerMove += RenderExperienceEntities;

        // create debug component
        _debug = new DebugCategoryComponent((instance) =>
        {
            instance.TryCreateCategory(new DebugManager.DebugCategory("experience_manager", "XP Manager"));
            instance.TryCreateField("current_count", "Count", $"{_entitiesList.Count} ({_renderedEntitiesCount})");
        })
        { Name = "ExperienceManagerDebugComp" };
        AddChild(_debug, true);
    }

    public override void _ExitTree()
    {
        _entitiesList.ForEach(FreeExperienceEntityRids);
    }

    public override void _PhysicsProcess(double delta)
    {
        // spawn entities
        _entitiesQueued.ForEach(_entitiesList.Add);
        _entitiesQueued.Clear();
        RenderExperienceEntities();
    }

    private void RenderExperienceEntities()
    {
        Vector2 playerPos = GameManager.Instance.Player.Position;
        int rendered = 0;

        _entitiesList.ForEach((entity) =>
        {
            if (entity.Position.DistanceTo(playerPos) > GameManager.RENDER_DISTANCE) { FreeExperienceEntityRids(entity); }
            else
            {
                RenderExperienceEntity(entity);
                rendered++;
            }
        });

        _renderedEntitiesCount = rendered;

        // update debug labels
        _debug.TryUpdateField("current_count", $"{_entitiesList.Count} ({_renderedEntitiesCount})");
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
        PhysicsServer2D.AreaSetCollisionLayer(entity.AreaRid, 16); // 16 = experience layer
        PhysicsServer2D.AreaSetMonitorable(entity.AreaRid, true);

        // create canvas entity
        entity.CanvasItemRid = RenderingServer.CanvasItemCreate();
        RenderingServer.CanvasItemSetParent(entity.CanvasItemRid, GetCanvasItem());
        RenderingServer.CanvasItemSetTransform(entity.CanvasItemRid, posTransform);
        RenderingServer.CanvasItemAddCircle(entity.CanvasItemRid, Vector2.Zero, ExperienceEntitySize, new Color(0.125f, 0.125f, 0.125f, 1f));
    }

    private static void FreeExperienceEntityRids(ExperienceEntity entity)
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

        // update debug label
        _debug.TryUpdateField("current_count", $"{_entitiesList.Count} ({_renderedEntitiesCount})");
    }

    public ExperienceEntity FindExperienceEntityFromAreaRid(Rid areaRid)
    {
        ExperienceEntity entity = _entitiesList.Find((i) => i.AreaRid == areaRid);
        return entity;
    }
}