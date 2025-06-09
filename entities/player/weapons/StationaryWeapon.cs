using Godot;

public partial class StationaryWeapon(BaseWeapon.TrajectoryStyleEnum entityTrajectory, bool test_manual = false) : BaseWeapon(entityTrajectory, test_manual)
{
    public override string GetWeaponType() => "Stationary";

    public override WeaponEntity GetBaseEntity()
    {
        return new WeaponEntity()
        {
            Damage = 10,
            Speed = 0,
            Radius = 20,
            MaxLifeTime = 10,
            Foreground = false,
        };
    }

    public override WeaponEntity CreateEntity()
    {
        WeaponEntity entity = GetBaseEntity();

        if (TEST_MANUAL) { entity.Position = GlobalPosition + (Vector2.Right * 100); }
        else
        {
            float distance = GameManager.Instance.RNG.RandfRange(50, 150);
            float angle = GameManager.Instance.RNG.RandfRange(0, 360);
            entity.Position = Utils.GetPointArounOrigin(GlobalPosition, distance, angle);
        }

        return entity;
    }

    public override void OnCollision(WeaponEntity entity, BasicEnemy enemy)
    {
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