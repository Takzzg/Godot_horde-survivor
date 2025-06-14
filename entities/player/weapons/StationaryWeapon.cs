using Godot;
using static Utils;

public partial class StationaryWeapon : BaseWeapon
{
    public StationaryWeapon(RarityEnum rarity, TrajectoryStyleEnum entityTrajectory, bool test_manual = false) : base(rarity, entityTrajectory, test_manual)
    {
        Type = TypeEnum.STATIONARY;
        TimerDelay = 0.75f;
    }

    public override WeaponEntity GetBaseEntity()
    {
        return new WeaponEntity()
        {
            Damage = 4,
            Speed = 0,
            Radius = 20,
            MaxLifeTime = 1.5f,
            Foreground = false,
        };
    }

    public override WeaponEntity CreateEntity()
    {
        WeaponEntity entity = GetBaseEntity();

        float distance = GameManager.Instance.RNG.RandfRange(50, 150);
        float angle = Vector2.Right.AngleTo(GetTrajectory());
        entity.Position = Utils.GetPointArounOrigin(GlobalPosition, distance, angle);

        return entity;
    }

    public override void UpdateEntityPosition(WeaponEntity entity, double delta)
    {
        if (Trajectory == TrajectoryStyleEnum.NONE || entity.Speed == 0) return;
        WeaponEntityManager.MoveEntity(entity, delta);
    }
}