using System;
using System.Collections.Generic;
using Godot;

public abstract partial class BaseWeapon : DebuggerNode
{
    public enum TypeEnum { PROJECTILE, STATIONARY, RELATIVE }
    public TypeEnum Type;

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
        WeaponEntityManager.ProcessEntities(delta, GlobalPosition, UpdateEntityPosition, ProcessCollision);

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

    // -------------------------------------------- Modifiers --------------------------------------------
    public readonly List<BaseModifier> Modifiers = [];

    public void AddModifier(BaseModifier mod)
    {
        Modifiers.Add(mod);
        DebugUpdateEntityDetails();
    }

    private void OnTrigger()
    {
        // create entity
        WeaponEntity entity = CreateEntity();
        // apply modifiers
        Modifiers.ForEach(mod => mod.OnCreateEntity(entity));
        // register entity
        WeaponEntityManager.RegisterEntity(entity);

        // update debug label
        DebugTryUpdateField("entities_count", WeaponEntityManager.EntitiesList.Count.ToString());
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
    public abstract WeaponEntity CreateEntity();
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

    protected static Vector2 GetRandomTrajectory() { return Vector2.One.Rotated(GameManager.Instance.RNG.RandfRange(0, 360)); }
    public Vector2 GetFacingTrajectory() { return _player.PlayerMovement.FacingDirection; }
    public virtual Vector2 GetFixedTrajectory() { throw new NotImplementedException(); }

    // -------------------------------------------- Entities --------------------------------------------
    public WeaponEntityManager WeaponEntityManager;
    public int MaxCollisionsPerFrame = 16;

    public abstract WeaponEntity GetBaseEntity();
    public abstract void UpdateEntityPosition(WeaponEntity entity, double delta);
    public virtual void OnCollision(WeaponEntity entity, Rid areaRid) { }

    public void ProcessCollision(WeaponEntity entity, Rid areaRid)
    {
        OnCollision(entity, areaRid);
        EnemyEntity enemy = GameManager.Instance.EnemiesManager.EnemyReceiveDamage(areaRid, entity.Damage);

        // increase stats
        _player.PlayerStats.IncreaseDamageDealt(entity.Damage);
        if (!enemy.Alive) _player.PlayerStats.IncreaseKillCount(1);
    }

    // -------------------------------------------- Display --------------------------------------------

    public WeaponDisplay GetWeaponDisplay(Action<WeaponDisplay> onClick = null)
    {
        WeaponDisplay display = ResourcePaths.GetSceneInstanceFromEnum<WeaponDisplay>(ResourcePaths.ScenePathsEnum.WEAPON_DISPLAY);
        display.Ready += () => display.UpdateValues(this, onClick);
        return display;
    }

    // -------------------------------------------- Debug --------------------------------------------
    public override DebugCategory DebugCreateCategory()
    {
        DebugCategory category = new($"{Type.ToString().Capitalize()} weapon");

        category.CreateDivider("Weapon Stats");
        category.CreateLabelField("entity_trajectory", "Trajectory.", Trajectory.ToString());
        category.CreateLabelField("entities_count", "Entities", WeaponEntityManager.EntitiesList.Count.ToString());
        category.CreateLabelField("timer_delay", "Delay", TEST_MANUAL ? "MANUAL" : $"{TimerDelay}s ({1 / TimerDelay}bps)");
        category.CreateLabelField("collisions_per_frame", "Collisions/f", MaxCollisionsPerFrame.ToString());

        WeaponEntity baseEntity = GetBaseEntity();

        category.CreateDivider("Entity Stats");
        category.CreateLabelField("entity_damage", "Damage", baseEntity.Damage.ToString());
        category.CreateLabelField("entity_radius", "Radius", baseEntity.Radius.ToString());
        category.CreateLabelField("entity_speed", "Speed", baseEntity.Speed.ToString());
        category.CreateLabelField("entity_pierce_count", "Pierce", baseEntity.MaxPierceCount.ToString());
        category.CreateLabelField("entity_max_dist", "Max dist.", baseEntity.MaxDistance.ToString());
        category.CreateLabelField("entity_max_lifetime", "Lifespan", baseEntity.MaxLifeTime.ToString());

        return category;
    }

    private void DebugUpdateEntityDetails()
    {
        // create entity
        WeaponEntity entity = CreateEntity();
        // apply modifiers
        Modifiers.ForEach(mod => mod.OnCreateEntity(entity));

        DebugTryUpdateField("entity_damage", entity.Damage.ToString());
        DebugTryUpdateField("entity_radius", entity.Radius.ToString());
        DebugTryUpdateField("entity_speed", entity.Speed.ToString());
        DebugTryUpdateField("entity_pierce_count", entity.MaxPierceCount.ToString());
        DebugTryUpdateField("entity_max_dist", entity.MaxDistance.ToString());
        DebugTryUpdateField("entity_max_lifetime", entity.MaxLifeTime.ToString());
    }
}