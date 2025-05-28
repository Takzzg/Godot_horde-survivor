using System;
using Godot;
using Godot.Collections;

public partial class BulletManager : Node2D
{
    public int BulletSize;
    public Shape2D SharedShape;
    public Action<BasicBullet, BasicEnemy> OnEnemyCollision;

    public BulletManager(int size, Action<BasicBullet, BasicEnemy> onCollide)
    {
        BulletSize = size;
        OnEnemyCollision = onCollide;

        SharedShape = new CircleShape2D() { Radius = BulletSize };
    }

    public void SetUpBullet(BasicBullet bullet)
    {
        // GD.Print($"Spawning bullet at {pos}");
        Transform2D posTransform = new(0, bullet.Position);

        // draw bullet
        bullet.CanvasItemRid = RenderingServer.CanvasItemCreate();
        RenderingServer.CanvasItemSetParent(bullet.CanvasItemRid, GetCanvasItem());
        RenderingServer.CanvasItemSetTransform(bullet.CanvasItemRid, posTransform);
        RenderingServer.CanvasItemAddCircle(bullet.CanvasItemRid, Vector2.Zero, BulletSize, Colors.DarkGray);
    }

    public void MoveBullet(BasicBullet bullet, double delta)
    {
        // calculate motion
        Vector2 offset = new(
            (float)(bullet.Direction.X * bullet.Speed * delta),
            (float)(bullet.Direction.Y * bullet.Speed * delta)
        );

        // move bullet
        bullet.Position += offset;
        Transform2D posTransform = new(0, bullet.Position);

        // move canvas item
        RenderingServer.CanvasItemSetTransform(bullet.CanvasItemRid, posTransform);
    }

    public void CheckCollision(BasicBullet bullet, int maxColisions)
    {
        Transform2D posTransform = new(0, bullet.Position);
        PhysicsShapeQueryParameters2D query = new()
        {
            CollideWithAreas = true,
            CollideWithBodies = false,
            Shape = SharedShape,
            CollisionMask = 4, // 4 = enemy hurtbox layer
            Transform = posTransform,
        };

        // check collision
        Array<Dictionary> collisions = GetWorld2D().DirectSpaceState.IntersectShape(query, maxColisions);
        if (collisions.Count == 0) return;

        // deal damage
        foreach (Dictionary col in collisions)
        {
            BasicEnemy enemy = GameManager.Instance.EnemiesManager.FindEnemyByAreaRid((Rid)col["rid"]);
            OnEnemyCollision(bullet, enemy);
        }
    }

    public void FreeBulletRids(BasicBullet bullet)
    {
        RenderingServer.FreeRid(bullet.CanvasItemRid);
    }
}