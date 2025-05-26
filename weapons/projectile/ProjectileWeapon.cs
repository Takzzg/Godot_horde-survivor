using System.Collections.Generic;
using Godot;

public partial class ProjectileWeapon : Node2D
{
    // components
    public WeaponShooting WeaponShooting;
    public WeaponAiming WeaponAiming;

    // bullet stats
    public int BulletDamage = 50;
    public int BulletRadius = 2;
    public int BulletSpeed = 75;
    public int BulletMaxDistance = 100;
    public int BulletMaxLifetime = 10;
    public int BulletPierceCount = 1;

    // bullet management
    public List<BasicBullet> BulletsArr = [];
    public BulletManager BulletsManager;

    public ProjectileWeapon(WeaponShooting.EnumShootingStyle shooting, WeaponAiming.EnumAimingStyle aiming)
    {
        // create shared texture
        Texture2D BulletTileSet = GD.Load<Texture2D>("res://weapons/projectile/basic_bullets_tileset.png");
        Vector2 TileSize = new(8, 8);
        Vector2 TileOffset = new(8, 8);
        AtlasTexture texture = new() { Atlas = BulletTileSet, Region = new Rect2(TileOffset, TileSize) };

        // create bullet manager
        BulletsManager = new(texture, BulletRadius, OnEnemyCollision) { TopLevel = true };
        AddChild(BulletsManager);

        // create components
        WeaponShooting = new(shooting, Shoot);
        AddChild(WeaponShooting);
        WeaponAiming = new(aiming);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (BulletsArr.Count == 0) return;
        MoveBullets(delta);
    }

    // -------------------------------------------- Bullets --------------------------------------------
    public void Shoot()
    {
        Vector2 dir = WeaponAiming.GetTrajectory();

        BasicBullet bullet = new()
        {
            Damage = BulletDamage,
            Position = GlobalPosition,
            Speed = BulletSpeed,
            Direction = dir,
            PierceCount = BulletPierceCount,
        };

        BulletsManager.SetUpBullet(bullet);
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
        foreach (BasicBullet bullet in bulletsQueuedForDestruction) { DestroyBullet(bullet); }
    }

    public void OnEnemyCollision(BasicBullet bullet, BasicEnemy enemy)
    {
        GameManager.Instance.EnemyManager.EnemyReceiveDamage(enemy, bullet.Damage);

        if (bullet.PierceCount <= 0) DestroyBullet(bullet);
        bullet.PierceCount -= 1;
    }

    public void DestroyBullet(BasicBullet bullet)
    {
        BulletsManager.DestroyBullet(bullet);
        BulletsArr.Remove(bullet);
    }
}
