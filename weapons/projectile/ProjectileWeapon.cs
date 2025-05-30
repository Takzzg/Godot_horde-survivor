using System.Collections.Generic;
using Godot;

public partial class ProjectileWeapon : DebugNode2D
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

        TreeExiting += () => { BulletsList.ForEach(BulletsManager.FreeBulletRids); };
    }

    public override void _PhysicsProcess(double delta)
    {
        if (BulletsList.Count == 0) return;
        ProcessBullets(delta);
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
        DebugTryUpdateField("bullets_count", BulletsList.Count.ToString());
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
        if (enemyDied) GameManager.Instance.Player.PlayerStats.IncreaseKillCount(1);
    }

    public void DestroyBullet(BasicBullet bullet)
    {
        BulletsManager.FreeBulletRids(bullet);
        BulletsList.Remove(bullet);

        // update debug label
        DebugTryUpdateField("bullets_count", BulletsList.Count.ToString());
    }

    public void OnPlayerDeath()
    {
        WeaponShooting.TimedAttackSetRunning(false);

        BulletsList.ForEach(BulletsManager.FreeBulletRids);
        BulletsList.Clear();
    }

    public override DebugCategory DebugCreateCategory()
    {
        DebugCategory category = new("Projectile Weapon");

        category.CreateDivider("Weapon Stats");
        category.CreateLabelField("shooting_style", "Shooting s.", WeaponShooting.Style.ToString());
        category.CreateLabelField("aiming_style", "Aiming s.", WeaponAiming.Style.ToString());
        category.CreateLabelField("bullets_count", "Bullets count", BulletsList.Count.ToString());

        category.CreateDivider("Bullet Stats");
        category.CreateLabelField("bullet_damage", "Damage", BulletDamage.ToString());
        category.CreateLabelField("bullet_radius", "Radius", BulletRadius.ToString());
        category.CreateLabelField("bullet_speed", "Speed", BulletSpeed.ToString());
        category.CreateLabelField("bullet_max_dist", "Max dist.", BulletMaxDistance.ToString());
        category.CreateLabelField("bullet_max_lifetime", "Lifespan", BulletMaxLifetime.ToString());
        category.CreateLabelField("bullet_pierce_count", "Pierce", BulletPierceCount.ToString());

        return category;
    }
}
