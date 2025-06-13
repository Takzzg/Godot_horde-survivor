using Godot;

public partial class RelativeWeapon : BaseWeapon
{
    public int RelativeOffset = 10;

    public RelativeWeapon(TrajectoryStyleEnum entityTrajectory, bool test_manual = false) : base(entityTrajectory, test_manual)
    {
        Type = TypeEnum.RELATIVE;
    }

    public override WeaponEntity GetBaseEntity()
    {
        return new WeaponEntity()
        {
            Damage = 2,
            Speed = 0,
            Radius = 10,
            MaxLifeTime = 5,
        };
    }

    public override WeaponEntity CreateEntity()
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

        return entity;
    }

    public override void UpdateEntityPosition(WeaponEntity entity, double delta)
    {
        WeaponEntityManager.SetEntityPosition(entity, _player.Position + entity.Offset);
    }
}