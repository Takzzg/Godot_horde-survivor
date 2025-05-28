using System.Collections.Generic;
using Godot;

public partial class ProjectileWeapon : Node2D
{
    // components
    public WeaponShooting WeaponShooting;
    public WeaponAiming WeaponAiming;

    // bullet stats
    public int BulletDamage = 25;
    public int BulletRadius = 2;
    public int BulletSpeed = 75;
    public int BulletMaxDistance = 100;
    public int BulletMaxLifetime = 10;
    public int BulletPierceCount = 1;

    // bullet management
    public List<BasicBullet> BulletsList = [];
    public BulletManager BulletsManager;

    public ProjectileWeapon(WeaponShooting.EnumShootingStyle shooting, WeaponAiming.EnumAimingStyle aiming)
    {
        BulletsManager = new(BulletRadius, OnEnemyCollision) { TopLevel = true };
        AddChild(BulletsManager);

        WeaponShooting = new(shooting, Shoot);
        AddChild(WeaponShooting);

        WeaponAiming = new(aiming);
        GameManager.Instance.Player.PlayerHealth.PlayerDeath += OnPlayerDeath;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (BulletsList.Count == 0) return;
        ProcessBullets(delta);
    }

    public override void _ExitTree()
    {
        BulletsList.ForEach(BulletsManager.FreeBulletRids);
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
        BulletsList.Add(bullet);
    }

    public void ProcessBullets(double delta)
    {
        List<BasicBullet> expiredBullets = [];

        foreach (BasicBullet bullet in BulletsList)
        {
            // move bullet
            BulletsManager.MoveBullet(bullet, delta);

            // mark expired if too far or lifetime elapsed
            if (bullet.Position.DistanceTo(GlobalPosition) > BulletMaxDistance || bullet.MaxLifeTime > BulletMaxLifetime)
            {
                expiredBullets.Add(bullet);
                continue;
            }

            // check collision
            BulletsManager.CheckCollision(bullet, 1);

            // mark expired if no pierce
            if (bullet.PierceCount <= 0) expiredBullets.Add(bullet);
        }

        // destroy expired bullets
        if (expiredBullets.Count > 0) { expiredBullets.ForEach(DestroyBullet); }
    }

    public void OnEnemyCollision(BasicBullet bullet, BasicEnemy enemy)
    {
        GameManager.Instance.EnemiesManager.EnemyReceiveDamage(enemy, bullet.Damage);
        bullet.PierceCount -= 1;
    }

    public void DestroyBullet(BasicBullet bullet)
    {
        BulletsManager.FreeBulletRids(bullet);
        BulletsList.Remove(bullet);
    }

    public void OnPlayerDeath()
    {
        BulletsList.ForEach(BulletsManager.FreeBulletRids);
        BulletsList.Clear();
    }
}
