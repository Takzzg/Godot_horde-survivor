using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class BasicWeapon : Node2D
{
    [Export]
    public Timer Delay;

    [ExportCategory("Bullets")]
    [Export]
    public int BulletRadius = 1;
    [Export]
    public int BulletSpeed = 10;
    [Export]
    public int BulletMaxDistance = 100;
    [Export]
    public int BulletMaxLifetime = 10;
    [Export]
    public Texture2D BulletTexture;

    public List<BasicBullet> BulletsArr = [];
    public Area2D SharedBulletArea;

    public override void _Ready()
    {
        // bind shoot timer
        Delay.Timeout += Shoot;

        // create shared area2d for bullets
        CollisionShape2D circle = new() { Shape = new CircleShape2D() { Radius = BulletRadius } };
        SharedBulletArea = new Area2D();
        SharedBulletArea.AddChild(circle);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (BulletsArr.Count == 0) return;

        MoveBullets(delta);
        QueueRedraw();
    }

    public override void _Draw()
    {
        DrawBullets();
    }

    public void Shoot()
    {
        SpawnBullet();
    }

    public void SpawnBullet()
    {
        Vector2 dir = Vector2.One.Rotated(GameManager.Instance.RNG.RandfRange(0, 360));
        BasicBullet bullet = new()
        {
            Position = Position,
            Speed = BulletSpeed,
            Direction = dir,
        };

        ConfigureBulletCollision(bullet);
        BulletsArr = [.. BulletsArr, bullet];
    }

    public void ConfigureBulletCollision(BasicBullet bullet)
    {
        Transform2D usedTransform = Transform2D.Identity;
        usedTransform.Rotated(0);
        // usedTransform.Translated(bullet.Position);
        usedTransform.Origin = bullet.Position;

        Rid shape = PhysicsServer2D.CircleShapeCreate();
        PhysicsServer2D.ShapeSetData(shape, BulletRadius);
        PhysicsServer2D.AreaAddShape(SharedBulletArea.GetRid(), shape, usedTransform);

        bullet.ShapeRid = shape;
    }

    public void MoveBullets(double delta)
    {
        Transform2D usedTransform = Transform2D.Identity;
        List<BasicBullet> bulletsQueuedForDestruction = [];

        for (int i = 0; i < BulletsArr.Count; i++)
        {
            BasicBullet bullet = BulletsArr[i];

            if (bullet.Position.DistanceTo(Position) > BulletMaxDistance || bullet.LifeTime > BulletMaxLifetime)
            {
                bulletsQueuedForDestruction.Add(bullet);
                continue;
            }

            Vector2 offset = new(
                (float)(bullet.Direction.X * bullet.Speed * delta),
                (float)(bullet.Direction.Y * bullet.Speed * delta)
            );

            bullet.Position += offset;
            usedTransform.Origin = bullet.Position;
            PhysicsServer2D.AreaSetShapeTransform(SharedBulletArea.GetRid(), i, usedTransform);

            bullet.LifeTime += delta;
        }

        ClearBullets(bulletsQueuedForDestruction);
    }

    private void ClearBullets(List<BasicBullet> bullets)
    {
        if (bullets.Count == 0) return;
        foreach (BasicBullet bullet in bullets)
        {
            PhysicsServer2D.FreeRid(bullet.ShapeRid);
            BulletsArr.Remove(bullet);
        }
        bullets.Clear();
    }

    public void DrawBullets()
    {
        Vector2 offset = BulletTexture.GetSize() / 2;
        foreach (BasicBullet bullet in BulletsArr) { DrawTexture(BulletTexture, bullet.Position - offset); }
    }
}
