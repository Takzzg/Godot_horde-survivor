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

    // debug
    private DebugManager.DebugTitle _title;
    private DebugCategoryComponent _weaponDebug;
    private DebugCategoryComponent _bulletsDebug;

    public ProjectileWeapon(WeaponShooting.EnumStyle shooting, WeaponAiming.EnumStyle aiming)
    {
        // create bullet manager
        BulletsManager = new BulletManager(BulletRadius, OnEnemyCollision) { TopLevel = true };
        AddChild(BulletsManager);

        // create shooting style
        WeaponShooting = new WeaponShooting(shooting, Shoot);
        AddChild(WeaponShooting);

        // create aiming style
        WeaponAiming = new WeaponAiming(aiming);
        GameManager.Instance.Player.PlayerHealth.PlayerDeath += OnPlayerDeath;

        // create debug components
        _title = new DebugManager.DebugTitle("W. Projectile");
        DebugManager.Instance.RenderNode(_title);
        TreeExiting += _title.QueueFree;

        _weaponDebug = new DebugCategoryComponent((instance) =>
        {
            instance.TryCreateCategory(new DebugManager.DebugCategory("weapon_stats", "Weapon Stats"));
            instance.TryCreateField("shooting_style", "Shooting s.", WeaponShooting.Style.ToString());
            instance.TryCreateField("aiming_style", "Aiming s.", WeaponAiming.Style.ToString());
            instance.TryCreateField("bullets_count", "Bullets count", BulletsList.Count.ToString());
        });
        AddChild(_weaponDebug);

        _bulletsDebug = new DebugCategoryComponent((instance) =>
        {
            instance.TryCreateCategory(new DebugManager.DebugCategory("bullet_stats", "Bullet Stats"));
            instance.TryCreateField("damage", "Damage", BulletDamage.ToString());
            instance.TryCreateField("radius", "Radius", BulletRadius.ToString());
            instance.TryCreateField("speed", "Speed", BulletSpeed.ToString());
            instance.TryCreateField("max_dist", "Max dist.", BulletMaxDistance.ToString());
            instance.TryCreateField("max_lifetime", "Lifespan", BulletMaxLifetime.ToString());
            instance.TryCreateField("pierce_count", "Pierce", BulletPierceCount.ToString());
        });
        AddChild(_bulletsDebug);
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

        // update debug label
        _weaponDebug.TryUpdateField("bullets_count", BulletsList.Count.ToString());
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
            if (bullet.PierceCount < 0) expiredBullets.Add(bullet);
        }

        // destroy expired bullets
        if (expiredBullets.Count > 0) { expiredBullets.ForEach(DestroyBullet); }
    }

    public void OnEnemyCollision(BasicBullet bullet, BasicEnemy enemy)
    {
        bullet.PierceCount -= 1;
        var (enemyDied, damage_dealt) = GameManager.Instance.EnemiesManager.EnemyReceiveDamage(enemy, bullet.Damage);

        // increase stats
        GameManager.Instance.Player.PlayerStats.IncreaseDamageDealt(damage_dealt);
        if (enemyDied) GameManager.Instance.Player.PlayerStats.IncreaseKillsCount(1);
    }

    public void DestroyBullet(BasicBullet bullet)
    {
        BulletsManager.FreeBulletRids(bullet);
        BulletsList.Remove(bullet);

        // update debug label
        _weaponDebug.TryUpdateField("bullets_count", BulletsList.Count.ToString());
    }

    public void OnPlayerDeath()
    {
        BulletsList.ForEach(BulletsManager.FreeBulletRids);
        BulletsList.Clear();
    }
}
