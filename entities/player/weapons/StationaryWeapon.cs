using Godot;

public partial class StationaryWeapon : BaseWeapon
{
    public StationaryWeapon(TrajectoryStyleEnum entityTrajectory, bool test_manual = false) : base(entityTrajectory, test_manual)
    {
        MaxCollisionsPerFrame = 16;
    }

    public override string DebugGetCategoryTitle() => "Stationary";

    public override WeaponEntity GetBaseEntity()
    {
        return new WeaponEntity()
        {
            Damage = 10,
            Speed = 0,
            Radius = 20,
            MaxLifeTime = 10,
        };
    }

    public override void OnTrigger()
    {
        WeaponEntity entity = GetBaseEntity();

        if (TEST_MANUAL) { entity.Position = GlobalPosition + (Vector2.Right * 100); }
        else
        {
            float distance = GameManager.Instance.RNG.RandfRange(50, 150);
            float angle = GameManager.Instance.RNG.RandfRange(0, 360);
            entity.Position = Utils.GetPointArounOrigin(GlobalPosition, distance, angle);
        }

        WeaponEntityManager.SetUpEntity(entity);
        EntitiesList.Add(entity);

        // update debug label
        DebugTryUpdateField("entities_count", EntitiesList.Count.ToString());
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
        if (Trajectory == TrajectoryStyleEnum.NONE || entity.Speed == 0) return;
        WeaponEntityManager.MoveEntity(entity, delta);
    }
}