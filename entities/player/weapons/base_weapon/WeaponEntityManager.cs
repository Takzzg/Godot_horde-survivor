using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;

public partial class WeaponEntityManager() : Node2D
{
    public void SetUpEntity(WeaponEntity entity)
    {
        // GD.Print($"Spawning entity at {entity.Position}");
        Transform2D posTransform = new(0, entity.Position);

        // draw entity
        entity.CanvasItemRid = RenderingServer.CanvasItemCreate();
        RenderingServer.CanvasItemSetParent(entity.CanvasItemRid, GetCanvasItem());
        RenderingServer.CanvasItemSetTransform(entity.CanvasItemRid, posTransform);
        RenderingServer.CanvasItemAddCircle(entity.CanvasItemRid, Vector2.Zero, entity.Radius, Colors.DarkGray);
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

    public void CheckCollision(WeaponEntity entity, int maxColisions, Action<WeaponEntity, BasicEnemy> onCollide)
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

        // check collision
        Array<Dictionary> collisions = GetWorld2D().DirectSpaceState.IntersectShape(query, maxColisions);
        if (collisions.Count == 0) return;
        GD.Print($"collisions.Count: {collisions.Count}");

        // deal damage
        List<Rid> collisionRids = [];

        foreach (Dictionary col in collisions)
        {
            Rid rid = (Rid)col["rid"];

            collisionRids.Add(rid);
            if (entity.CollidingWith.Contains(rid)) { continue; }

            BasicEnemy enemy = GameManager.Instance.EnemiesManager.FindEnemyByAreaRid(rid);
            onCollide(entity, enemy);
        }

        entity.CollidingWith = collisionRids;
    }

    public static void FreeEntityRids(WeaponEntity entity)
    {
        RenderingServer.FreeRid(entity.CanvasItemRid);
    }
}