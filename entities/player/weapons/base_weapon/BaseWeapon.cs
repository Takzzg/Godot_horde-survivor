using System;
using Godot;

public abstract partial class BaseWeapon : DebuggerNode
{
    public BaseWeapon(TrajectoryStyleEnum entityTrajectory, bool test_manual = false)
    {
        TEST_MANUAL = test_manual;
        Trajectory = entityTrajectory;

        // create entity manager
        WeaponEntityManager = new WeaponEntityManager(MaxCollisionsPerFrame) { TopLevel = true };
        AddChild(WeaponEntityManager);

        if (TEST_MANUAL) return;
        CreateTimer();
    }

    public override void _PhysicsProcess(double delta)
    {
        WeaponEntityManager.ProcessEntities(delta, UpdateEntityPosition, OnCollision);

        // update debug label
        DebugTryUpdateField("entities_count", WeaponEntityManager.EntitiesList.Count.ToString());
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (!TEST_MANUAL) return;
        if (@event.IsActionPressed("enter"))
        {
            OnTrigger();
            GetViewport().SetInputAsHandled();
        }
    }

    // -------------------------------------------- Player --------------------------------------------
    protected PlayerScene _player;

    public void SetPlayerReference(PlayerScene player)
    {
        _player = player;
    }

    public void OnPlayerDeath()
    {
        TimedSetRunning(false);
        WeaponEntityManager.ClearEntities();

        // update debug label
        DebugTryUpdateField("entities_count", WeaponEntityManager.EntitiesList.Count.ToString());
    }

    // -------------------------------------------- Timer --------------------------------------------
    protected bool TEST_MANUAL = false;
    private Timer _timer;
    public abstract void OnTrigger();
    public float TimerDelay = 0.25f;

    public void TimedSetRunning(bool state)
    {
        if (TEST_MANUAL) return;

        if (state) _timer.Start();
        else _timer.Stop();
    }

    public void CreateTimer()
    {
        _timer = new() { Autostart = true, OneShot = false, WaitTime = TimerDelay };
        _timer.Timeout += OnTrigger;
        AddChild(_timer);
    }

    // -------------------------------------------- Trajectory --------------------------------------------
    public enum TrajectoryStyleEnum { NONE, RANDOM, FACING, FIXED }
    public TrajectoryStyleEnum Trajectory;

    public Vector2 GetTrajectory()
    {
        return Trajectory switch
        {
            TrajectoryStyleEnum.RANDOM => GetRandomTrajectory(),
            TrajectoryStyleEnum.FACING => GetFacingTrajectory(),
            TrajectoryStyleEnum.FIXED => GetFixedTrajectory(),
            TrajectoryStyleEnum.NONE => Vector2.Zero,
            _ => throw new NotImplementedException(),
        };
    }

    public Func<Vector2> GetFixedTrajectory = () => throw new NotImplementedException();
    public Vector2 GetFacingTrajectory() { return _player.PlayerMovement.FacingDirection; }
    protected static Vector2 GetRandomTrajectory() { return Vector2.One.Rotated(GameManager.Instance.RNG.RandfRange(0, 360)); }

    // -------------------------------------------- Entities --------------------------------------------
    public WeaponEntityManager WeaponEntityManager;
    public int MaxCollisionsPerFrame = 16;

    public abstract void OnCollision(WeaponEntity entity, BasicEnemy enemy);
    public abstract WeaponEntity GetBaseEntity();
    public abstract void UpdateEntityPosition(WeaponEntity entity, double delta);

    // -------------------------------------------- Pause Menu --------------------------------------------
    public abstract string GetWeaponType();

    public Control GetWeaponPanel()
    {
        DebugCategory panel = DebugCreateCategory();
        return panel;
    }

    // -------------------------------------------- Debug --------------------------------------------
    public override DebugCategory DebugCreateCategory()
    {
        DebugCategory category = new($"{GetWeaponType()} weapon");

        category.CreateDivider("Weapon Stats");
        category.CreateLabelField("entity_trajectory", "Trajectory.", Trajectory.ToString());
        category.CreateLabelField("entities_count", "Entities", WeaponEntityManager.EntitiesList.Count.ToString());
        category.CreateLabelField("timer_delay", "Delay", TEST_MANUAL ? "MANUAL" : $"{TimerDelay}s ({1 / TimerDelay}bps)");

        WeaponEntity baseEntity = GetBaseEntity();

        category.CreateDivider("Entity Stats");
        category.CreateLabelField("entity_damage", "Damage", baseEntity.Damage.ToString());
        category.CreateLabelField("entity_radius", "Radius", baseEntity.Radius.ToString());
        category.CreateLabelField("entity_speed", "Speed", baseEntity.Speed.ToString());
        category.CreateLabelField("entity_max_dist", "Max dist.", baseEntity.MaxDistance.ToString());
        category.CreateLabelField("entity_max_lifetime", "Lifespan", baseEntity.MaxLifeTime.ToString());
        category.CreateLabelField("entity_pierce_count", "Pierce", baseEntity.MaxPierceCount.ToString());

        return category;
    }
}