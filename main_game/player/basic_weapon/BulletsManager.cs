using System;
using Godot;

public partial class BulletsManager : Node2D
{
    public int BulletSize;
    public AtlasTexture SharedTexture;
    public Shape2D SharedShape;

    public BulletsManager(AtlasTexture texture, int size)
    {
        SharedTexture = texture;
        BulletSize = size;

        SharedShape = new CircleShape2D() { Radius = BulletSize };
    }

    public BasicBullet SpawnBullet(Vector2 pos, Vector2 dir, int spd)
    {
        Transform2D posTransform = new(0, pos);
        BasicBullet bullet = new()
        {
            Position = pos,
            Speed = spd,
            Direction = dir,
            BodyRid = PhysicsServer2D.BodyCreate(),
            AreaRid = PhysicsServer2D.AreaCreate(),
            SpriteRid = RenderingServer.CanvasItemCreate(),
        };

        // create body
        PhysicsServer2D.BodySetSpace(bullet.BodyRid, GetWorld2D().Space);
        PhysicsServer2D.BodySetMode(bullet.BodyRid, PhysicsServer2D.BodyMode.RigidLinear);
        PhysicsServer2D.BodySetParam(bullet.BodyRid, PhysicsServer2D.BodyParameter.GravityScale, 0);
        PhysicsServer2D.BodySetState(bullet.BodyRid, PhysicsServer2D.BodyState.Transform, posTransform);
        PhysicsServer2D.BodySetState(bullet.BodyRid, PhysicsServer2D.BodyState.LinearVelocity, bullet.Direction * bullet.Speed);

        // create area
        PhysicsServer2D.AreaAddShape(bullet.AreaRid, SharedShape.GetRid(), Transform2D.Identity);
        PhysicsServer2D.AreaSetSpace(bullet.AreaRid, GetWorld2D().Space);
        // PhysicsServer2D.AreaSetCollisionLayer(bullet.AreaRid, 8);
        // PhysicsServer2D.AreaSetCollisionMask(bullet.AreaRid, 4);
        PhysicsServer2D.AreaSetMonitorable(bullet.AreaRid, true);
        PhysicsServer2D.AreaSetAreaMonitorCallback(bullet.AreaRid, new Callable(this, MethodName.OnAreaEntered));

        // create sprite
        Vector2 textureSize = SharedTexture.GetSize();
        RenderingServer.CanvasItemSetParent(bullet.SpriteRid, GetCanvasItem());
        RenderingServer.CanvasItemAddTextureRect(bullet.SpriteRid, new Rect2(-textureSize / 2, textureSize), SharedTexture.GetRid());
        RenderingServer.CanvasItemSetTransform(bullet.SpriteRid, posTransform);

        // create debug shapes
        RenderingServer.CanvasItemAddCircle(bullet.SpriteRid, Vector2.Zero, BulletSize, new Color(Colors.Red, 0.25f));

        return bullet;
    }

    public void OnAreaEntered(int status, Rid areaRid, int instance_id, int area_shape_idx, int self_shape_idx)
    {
        GD.Print($"BulletManager OnAreaEntered");
        GD.Print($"status: {status}, areaRid: {areaRid}, instance_id: {instance_id}, area_shape_idx: {area_shape_idx}, self_shape_idx: {self_shape_idx},");
    }

    public void MoveBullet(BasicBullet bullet, double delta)
    {
        Transform2D posTransform = (Transform2D)PhysicsServer2D.BodyGetState(bullet.BodyRid, PhysicsServer2D.BodyState.Transform);
        bullet.Position = posTransform.Origin;

        PhysicsServer2D.AreaSetTransform(bullet.AreaRid, posTransform);
        RenderingServer.CanvasItemSetTransform(bullet.SpriteRid, posTransform);
    }

    public void DestroyBullet(BasicBullet bullet)
    {
        PhysicsServer2D.FreeRid(bullet.BodyRid);
        PhysicsServer2D.FreeRid(bullet.AreaRid);

        RenderingServer.FreeRid(bullet.SpriteRid);
    }
}