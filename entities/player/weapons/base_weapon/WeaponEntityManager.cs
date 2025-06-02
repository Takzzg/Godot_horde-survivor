using System;
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
        RenderingServer.CanvasItemSetZIndex(entity.CanvasItemRid, entity.Foreground ? 30 : 0);
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

    public void CheckCollision(WeaponEntity entity, int maxColisions, double tickDelay, Action<WeaponEntity, BasicEnemy> onCollide)
    {
        double currentTime = Time.GetUnixTimeFromSystem();

        // clear expired collisions
        foreach (Rid key in entity.CollidingWith.Keys)
        {
            if (entity.CollidingWith[key] < currentTime) { entity.CollidingWith.Remove(key); }
        }

        // check collisions
        Transform2D posTransform = new(0, entity.Position);
        PhysicsShapeQueryParameters2D query = new()
        {
            CollideWithAreas = true,
            CollideWithBodies = false,
            Shape = new CircleShape2D() { Radius = entity.Radius },
            CollisionMask = 4, // 4 = enemy hurtbox layer
            Transform = posTransform,
        };

        Array<Dictionary> collisions = GetWorld2D().DirectSpaceState.IntersectShape(query, maxColisions);
        if (collisions.Count == 0) return;

        // trigger collision callback
        foreach (Dictionary col in collisions)
        {
            Rid rid = (Rid)col["rid"];
            if (entity.CollidingWith.ContainsKey(rid)) { continue; } // already colliding

            entity.CollidingWith.Add(rid, currentTime + tickDelay);

            BasicEnemy enemy = GameManager.Instance.EnemiesManager.FindEnemyByAreaRid(rid);
            onCollide(entity, enemy); // deal damage
        }
    }

    public static void FreeEntityRids(WeaponEntity entity)
    {
        RenderingServer.FreeRid(entity.CanvasItemRid);
    }
}