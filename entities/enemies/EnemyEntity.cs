using Godot;

public class EnemyEntity
{
    public Vector2 Position;

    public bool Alive = true;
    public float Health;
    public float Speed;
    public int Damage;

    public float HurtboxRadius;
    public float HitboxRadius;

    public int ExperienceDropped;

    public Rid CanvasItemRid;
    public Rid BodyRid;
    public Rid HurtboxRid;

    public void FreeEntityRids()
    {
        PhysicsServer2D.FreeRid(BodyRid);
        PhysicsServer2D.FreeRid(HurtboxRid);
        RenderingServer.FreeRid(CanvasItemRid);
    }

    public void RegisterBody(Transform2D pos, Rid space, Rid hitbox)
    {
        BodyRid = PhysicsServer2D.BodyCreate();
        PhysicsServer2D.BodyAddShape(BodyRid, hitbox, Transform2D.Identity);
        PhysicsServer2D.BodySetSpace(BodyRid, space);
        PhysicsServer2D.BodySetState(BodyRid, PhysicsServer2D.BodyState.Transform, pos);
        PhysicsServer2D.BodySetMode(BodyRid, PhysicsServer2D.BodyMode.RigidLinear);
        PhysicsServer2D.BodySetParam(BodyRid, PhysicsServer2D.BodyParameter.GravityScale, 0);
        PhysicsServer2D.BodySetParam(BodyRid, PhysicsServer2D.BodyParameter.Friction, 0);
        PhysicsServer2D.BodySetCollisionLayer(BodyRid, 2); // 2 = enemy layer
        PhysicsServer2D.BodySetCollisionMask(BodyRid, 3); // 1 = player layer, 2 = enemy layer
    }

    public void RegisterHurtbox(Transform2D pos, Rid space, Rid hurtbox)
    {
        HurtboxRid = PhysicsServer2D.AreaCreate();
        PhysicsServer2D.AreaAddShape(HurtboxRid, hurtbox, Transform2D.Identity);
        PhysicsServer2D.AreaSetTransform(HurtboxRid, pos);
        PhysicsServer2D.AreaSetSpace(HurtboxRid, space);
        PhysicsServer2D.AreaSetCollisionLayer(HurtboxRid, 4); // 4 = enemy hurtbox layer
        PhysicsServer2D.AreaSetCollisionMask(HurtboxRid, 8); // 8 = bullet layer
        PhysicsServer2D.AreaSetMonitorable(HurtboxRid, true);
    }

    public void RegisterSprite(Transform2D pos, Rid canvasItem)
    {
        CanvasItemRid = RenderingServer.CanvasItemCreate();
        RenderingServer.CanvasItemSetParent(CanvasItemRid, canvasItem);
        RenderingServer.CanvasItemSetZIndex(CanvasItemRid, 10);
        RenderingServer.CanvasItemSetTransform(CanvasItemRid, pos);
        RenderingServer.CanvasItemAddCircle(CanvasItemRid, Vector2.Zero, HitboxRadius, Colors.Black);
    }

    public void MoveTowardsTaget(Vector2 target)
    {
        Vector2 dir = (Speed == 0) ? Vector2.Zero : Position.DirectionTo(target).Normalized() * Speed;
        PhysicsServer2D.BodySetState(BodyRid, PhysicsServer2D.BodyState.LinearVelocity, dir);

        Transform2D posTransform = (Transform2D)PhysicsServer2D.BodyGetState(BodyRid, PhysicsServer2D.BodyState.Transform);
        PhysicsServer2D.AreaSetTransform(HurtboxRid, posTransform);
        RenderingServer.CanvasItemSetTransform(CanvasItemRid, posTransform);

        Position = posTransform.Origin;
    }

    public void ReceiveDamage(float amount)
    {
        Health -= amount;
        if (Health < 0) OnDeath();
    }

    public void OnDeath()
    {
        Alive = false;
        FreeEntityRids();
        GameManager.Instance.ExperienceManager.QueueExperienceEntitySpawn(Position, ExperienceDropped);
    }
}