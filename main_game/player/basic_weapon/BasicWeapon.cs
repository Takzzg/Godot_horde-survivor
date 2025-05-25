using System.Collections.Generic;
using Godot;

public partial class BasicWeapon : Node2D
{
    public Timer Delay;

    public int BulletRadius = 2;
    public int BulletSpeed = 75;
    public int BulletMaxDistance = 100;
    public int BulletMaxLifetime = 10;

    public List<BasicBullet> BulletsArr = [];
    public BulletsManager BulletsManager;

    public override void _Ready()
    {
        // bind shoot timer
        Delay = new Timer() { Autostart = true, OneShot = false, WaitTime = 1 };
        Delay.Timeout += Shoot;
        AddChild(Delay);

        // create shared texture
        Texture2D BulletTileSet = GD.Load<Texture2D>("res://main_game/player/basic_weapon/basic_bullets_tileset.png");
        Vector2 TileSize = new(8, 8);
        Vector2 TileOffset = new(8, 8);
        AtlasTexture texture = new() { Atlas = BulletTileSet, Region = new Rect2(TileOffset, TileSize) };

        // create bullet manager
        BulletsManager = new(texture, BulletRadius) { TopLevel = true };
        AddChild(BulletsManager);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (BulletsArr.Count == 0) return;

        MoveBullets(delta);
    }

    public void Shoot()
    {
        // Vector2 dir = Vector2.One.Rotated(GameManager.Instance.RNG.RandfRange(0, 360));
        Vector2 dir = Vector2.Right;
        BasicBullet bullet = BulletsManager.SpawnBullet(GlobalPosition, dir, BulletSpeed);
        BulletsArr.Add(bullet);
    }

    public void MoveBullets(double delta)
    {
        List<BasicBullet> bulletsQueuedForDestruction = [];

        foreach (BasicBullet bullet in BulletsArr)
        {
            // move bullet
            BulletsManager.MoveBullet(bullet, delta);

            // destroy if too far or lifetime elapsed
            if (bullet.Position.DistanceTo(GlobalPosition) > BulletMaxDistance || bullet.MaxLifeTime > BulletMaxLifetime)
            {
                bulletsQueuedForDestruction.Add(bullet);
                continue;
            }
        }

        // destroy bullets
        if (bulletsQueuedForDestruction.Count == 0) return;
        foreach (BasicBullet bullet in bulletsQueuedForDestruction)
        {
            BulletsManager.DestroyBullet(bullet);
            BulletsArr.Remove(bullet);
        }
    }
}
