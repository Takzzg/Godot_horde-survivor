using Godot;

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
            Damage = 5,
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

    public override void OnCollision(WeaponEntity entity, Rid areaRid)
    {
        entity.MaxPierceCount -= 1;
    }

    public override void UpdateEntityPosition(WeaponEntity entity, double delta)
    {
        WeaponEntityManager.MoveEntity(entity, delta);
    }
}
