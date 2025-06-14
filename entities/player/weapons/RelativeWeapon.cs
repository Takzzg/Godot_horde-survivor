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

        float angle = Vector2.Right.AngleTo(GetTrajectory());
        entity.Offset = Utils.GetPointArounOrigin(Vector2.Zero, RelativeOffset, angle);
        entity.Position = _player.Position + entity.Offset;

        return entity;
    }

    public override void UpdateEntityPosition(WeaponEntity entity, double delta)
    {
        WeaponEntityManager.SetEntityPosition(entity, _player.Position + entity.Offset);
    }
}