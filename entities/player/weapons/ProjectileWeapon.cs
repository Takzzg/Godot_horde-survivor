public partial class ProjectileWeapon : BaseWeapon
{
    public ProjectileWeapon(TrajectoryStyleEnum entityTrajectory, bool test_manual = false) : base(entityTrajectory, test_manual)
    {
        Type = TypeEnum.PROJECTILE;
        MaxCollisionsPerFrame = 1;
    }

    public override WeaponEntity GetBaseEntity()
    {
        return new WeaponEntity()
        {
            Damage = 25,
            Speed = 75,
            Radius = 2,
            MaxPierceCount = 1,
        };
    }

    public override WeaponEntity CreateEntity()
    {
        WeaponEntity entity = GetBaseEntity();
        entity.Position = GlobalPosition;
        entity.Direction = GetTrajectory();

        return entity;
    }

    public override void OnCollision(WeaponEntity entity, BasicEnemy enemy)
    {
        entity.MaxPierceCount -= 1;
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
