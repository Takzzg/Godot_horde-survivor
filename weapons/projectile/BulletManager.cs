using System;
using Godot;

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

        // create area
        bullet.AreaRid = PhysicsServer2D.AreaCreate();
        PhysicsServer2D.AreaAddShape(bullet.AreaRid, SharedShape.GetRid(), Transform2D.Identity);
        PhysicsServer2D.AreaSetSpace(bullet.AreaRid, GetWorld2D().Space);
        PhysicsServer2D.AreaSetCollisionLayer(bullet.AreaRid, 8); // 8 = bullet layer
        PhysicsServer2D.AreaSetCollisionMask(bullet.AreaRid, 4); // 4 = enemy hurtbox layer
        PhysicsServer2D.AreaSetMonitorable(bullet.AreaRid, true);

        // create callback
        Callable callback = Callable.From((int status, Rid areaRid, int instance_id, int area_shape_idx, int self_shape_idx) => OnAreaEntered(status, areaRid, bullet));
        PhysicsServer2D.AreaSetAreaMonitorCallback(bullet.AreaRid, callback);

        // draw enemy
        bullet.CanvasItemRid = RenderingServer.CanvasItemCreate();
        RenderingServer.CanvasItemSetParent(bullet.CanvasItemRid, GetCanvasItem());
        RenderingServer.CanvasItemSetTransform(bullet.CanvasItemRid, posTransform);
        RenderingServer.CanvasItemAddCircle(bullet.CanvasItemRid, Vector2.Zero, BulletSize, Colors.DarkGray);
    }

    public void OnAreaEntered(int status, Rid areaRid, BasicBullet bullet)
    {
        // GD.Print($"BulletManager OnAreaEntered");
        // area entering = 0, area exiting = 1
        if (status == 1) return;
        BasicEnemy enemy = GameManager.Instance.EnemiesManager.FindEnemyByAreaRid(areaRid);
        OnEnemyCollision(bullet, enemy);
        // GD.Print($"status: {status}, areaRid: {areaRid}, bullet: {bullet}, enemy {enemy}");
    }

    public void MoveBullet(BasicBullet bullet, double delta)
    {
        Vector2 offset = new(
            (float)(bullet.Direction.X * bullet.Speed * delta),
            (float)(bullet.Direction.Y * bullet.Speed * delta)
        );

        bullet.Position += offset;
        Transform2D posTransform = new(0, bullet.Position);

        PhysicsServer2D.AreaSetTransform(bullet.AreaRid, posTransform);
        RenderingServer.CanvasItemSetTransform(bullet.CanvasItemRid, posTransform);
    }

    public void DestroyBullet(BasicBullet bullet)
    {
        PhysicsServer2D.FreeRid(bullet.AreaRid);
        RenderingServer.FreeRid(bullet.CanvasItemRid);
    }
}