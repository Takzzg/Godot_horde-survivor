using Godot;

public partial class RelativeWeapon : BaseWeapon
{
    public int RelativeOffset = 10;

    public RelativeWeapon(TrajectoryStyleEnum entityTrajectory, bool test_manual = false) : base(entityTrajectory, test_manual)
    {
        MaxCollisionsPerFrame = 16;
    }

    public override string GetWeaponType() => "Relative";

    public override WeaponEntity GetBaseEntity()
    {
        return new WeaponEntity()
        {
            Damage = 15,
            Speed = 0,
            Radius = 10,
            MaxLifeTime = 5,
        };
    }

    public override void OnTrigger()
    {
        WeaponEntity entity = GetBaseEntity();
        if (TEST_MANUAL) { entity.Offset = new Vector2(RelativeOffset, 0); }
        else
        {
            float angle = GameManager.Instance.RNG.RandfRange(0, 360);
            entity.Offset = Utils.GetPointArounOrigin(_player.Position, RelativeOffset, angle);
        }
        entity.Position = _player.Position + entity.Offset;
        entity.Direction = GetTrajectory();

        WeaponEntityManager.SetUpEntity(entity);
        EntitiesList.Add(entity);

        // update debug label
        DebugTryUpdateField("entities_count", EntitiesList.Count.ToString());
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
        WeaponEntityManager.SetEntityPosition(entity, _player.Position + entity.Offset);
    }
}