using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;

public partial class WeaponEntityManager : Node2D
{
    public readonly List<WeaponEntity> EntitiesList = [];
    public double EntityTickDelay = 0.5f;
    public int MaxCollisionsPerFrame;

    public WeaponEntityManager(int maxCollisions)
    {
        MaxCollisionsPerFrame = maxCollisions;

        // clear entities on exit
        TreeExiting += ClearEntities;
    }

    public void ClearEntities()
    {
        EntitiesList.ForEach(FreeEntityRids);
        EntitiesList.Clear();
    }

    public void ProcessEntities(double delta, Action<WeaponEntity, double> updatePos, Action<WeaponEntity, BasicEnemy> onCollide)
    {
        if (EntitiesList.Count == 0) return;

        // GD.Print($"ProcessEntities()");
        // GD.Print($"EntitiesList.Count: {EntitiesList.Count}");

        List<WeaponEntity> expiredEntities = [];

        foreach (WeaponEntity entity in EntitiesList)
        {
            // move entity
            updatePos(entity, delta);

            // update entity lifetime
            entity.LifeTime += delta;

            // check collision
            UpdateEntityCollisions(entity, EntityTickDelay, delta);
            CheckNewCollisions(entity, onCollide);

            // check expired 
            if (
                (entity.MaxPierceCount < 0) || // no pierce left
                (entity.LifeTime > entity.MaxLifeTime) || // lifetime elapsed
                (entity.Position.DistanceTo(GlobalPosition) > entity.MaxDistance) // too far
            )
            { expiredEntities.Add(entity); }
        }

        // destroy expired entities
        if (expiredEntities.Count > 0) { expiredEntities.ForEach(DestroyEntity); }
    }

    private void DestroyEntity(WeaponEntity entity)
    {
        FreeEntityRids(entity);
        EntitiesList.Remove(entity);
    }

    public static void FreeEntityRids(WeaponEntity entity)
    {
        RenderingServer.FreeRid(entity.CanvasItemRid);
    }

    public void CreateEntity(WeaponEntity entity)
    {
        // GD.Print($"Spawning entity at {entity.Position}");
        Transform2D posTransform = new(0, entity.Position);

        // draw entity
        entity.CanvasItemRid = RenderingServer.CanvasItemCreate();
        RenderingServer.CanvasItemSetParent(entity.CanvasItemRid, GetCanvasItem());
        RenderingServer.CanvasItemSetZIndex(entity.CanvasItemRid, entity.Foreground ? 30 : 0);
        RenderingServer.CanvasItemSetTransform(entity.CanvasItemRid, posTransform);
        RenderingServer.CanvasItemAddCircle(entity.CanvasItemRid, Vector2.Zero, entity.Radius, Colors.DarkGray);

        // add to list
        EntitiesList.Add(entity);
    }

    public static void MoveEntity(WeaponEntity entity, double delta)
    {
        // calculate motion
        Vector2 offset = new(
            (float)(entity.Direction.X * entity.Speed * delta),
            (float)(entity.Direction.Y * entity.Speed * delta)
        );

        // move entity
        SetEntityPosition(entity, entity.Position + offset);
    }

    public static void SetEntityPosition(WeaponEntity entity, Vector2 pos)
    {
        entity.Position = pos;

        // move canvas item
        Transform2D posTransform = new(0, entity.Position);
        RenderingServer.CanvasItemSetTransform(entity.CanvasItemRid, posTransform);
    }

    public void UpdateEntityCollisions(WeaponEntity entity, double tickeDelay, double delta)
    {
        foreach (Rid key in entity.CollidingWith.Keys)
        {
            if (entity.CollidingWith[key] > tickeDelay) { entity.CollidingWith.Remove(key); }
            else { entity.CollidingWith[key] += delta; }
        }
    }

    public void CheckNewCollisions(WeaponEntity entity, Action<WeaponEntity, BasicEnemy> onCollide)
    {
        Transform2D posTransform = new(0, entity.Position);
        PhysicsShapeQueryParameters2D query = new()
        {
            CollideWithAreas = true,
            CollideWithBodies = false,
            Shape = new CircleShape2D() { Radius = entity.Radius },
            CollisionMask = 4, // 4 = enemy hurtbox layer
            Transform = posTransform,
        };

        Array<Dictionary> collisions = GetWorld2D().DirectSpaceState.IntersectShape(query, MaxCollisionsPerFrame);
        if (collisions.Count == 0) return;

        // trigger collision callback
        foreach (Dictionary col in collisions)
        {
            Rid rid = (Rid)col["rid"];
            if (entity.CollidingWith.ContainsKey(rid)) { continue; } // already colliding

            entity.CollidingWith.Add(rid, 0);

            BasicEnemy enemy = GameManager.Instance.EnemiesManager.FindEnemyByAreaRid(rid);
            onCollide(entity, enemy); // deal damage
        }
    }
}