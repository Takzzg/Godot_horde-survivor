using System;

public partial class ProjectileWeapon : BaseWeapon
{
    public ProjectileWeapon(TrajectoryStyleEnum entityTrajectory, bool test_manual = false) : base(entityTrajectory, test_manual)
    {
        MaxCollisionsPerFrame = 1;
    }

    public override string DebugGetCategoryTitle() => "Projectile";

    public override WeaponEntity GetBaseEntity()
    {
        return new WeaponEntity()
        {
            Damage = 25,
            Speed = 50,
            Radius = 2,
            MaxPierceCount = 1,
        };
    }

    public override void OnTrigger()
    {
        WeaponEntity entity = GetBaseEntity();
        entity.Position = GlobalPosition;
        entity.Direction = GetTrajectory();

        WeaponEntityManager.SetUpEntity(entity);
        EntitiesList.Add(entity);

        // update debug label
        DebugTryUpdateField("entities_count", EntitiesList.Count.ToString());
    }

    public override void OnCollision(WeaponEntity entity, BasicEnemy enemy)
    {
        entity.MaxPierceCount = Math.Max(entity.MaxPierceCount - 1, 0);
        var (enemyDied, damage_dealt) = GameManager.Instance.EnemiesManager.EnemyReceiveDamage(enemy, entity.Damage);

        // increase stats
        _player.PlayerStats.IncreaseDamageDealt(damage_dealt);
        if (enemyDied) _player.PlayerStats.IncreaseKillCount(1);
    }

    public override void UpdateEntityPosition(WeaponEntity entity, double delta)
    {
        WeaponEntityManager.MoveEntity(entity, delta);
    }
}
