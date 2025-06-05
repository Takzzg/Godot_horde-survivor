using System;
using System.Collections.Generic;
using Godot;

public abstract partial class BaseWeapon : DebuggerNode
{
    public BaseWeapon(TrajectoryStyleEnum entityTrajectory, bool test_manual = false)
    {
        TEST_MANUAL = test_manual;
        Trajectory = entityTrajectory;

        CreateEntityManager();

        if (TEST_MANUAL) return;
        CreateTimer();
    }

    public override void _PhysicsProcess(double delta)
    {
        ProcessEntities(delta);
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

        EntitiesList.ForEach(WeaponEntityManager.FreeEntityRids);
        EntitiesList.Clear();

        // update debug label
        DebugTryUpdateField("entities_count", EntitiesList.Count.ToString());
    }

    // -------------------------------------------- Timer --------------------------------------------
    protected bool TEST_MANUAL = false;
    private Timer _timer;
    public abstract void OnTrigger();

    public void TimedSetRunning(bool state)
    {
        if (TEST_MANUAL) return;

        if (state) _timer.Start();
        else _timer.Stop();
    }

    public void CreateTimer()
    {
        _timer = new() { Autostart = true, OneShot = false, WaitTime = 0.25 };
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
    public readonly List<WeaponEntity> EntitiesList = [];
    public WeaponEntityManager WeaponEntityManager;
    public int MaxCollisionsPerFrame;
    public double EntityTickDelay = 0.5f;

    public abstract void OnCollision(WeaponEntity entity, BasicEnemy enemy);
    public abstract WeaponEntity GetBaseEntity();
    public abstract void UpdateEntityPosition(WeaponEntity entity, double delta);

    public void CreateEntityManager()
    {
        // create entity manager
        WeaponEntityManager = new WeaponEntityManager() { TopLevel = true };
        AddChild(WeaponEntityManager);

        // clear entities on exit
        TreeExiting += () => { EntitiesList.ForEach(WeaponEntityManager.FreeEntityRids); };
    }

    private void DestroyEntity(WeaponEntity entity)
    {
        WeaponEntityManager.FreeEntityRids(entity);
        EntitiesList.Remove(entity);

        // update debug label
        DebugTryUpdateField("entities_count", EntitiesList.Count.ToString());
    }

    private void ProcessEntities(double delta)
    {
        if (EntitiesList.Count == 0) return;

        // GD.Print($"ProcessEntities()");
        // GD.Print($"EntitiesList.Count: {EntitiesList.Count}");

        List<WeaponEntity> expiredEntities = [];

        foreach (WeaponEntity entity in EntitiesList)
        {
            // move entity
            UpdateEntityPosition(entity, delta);

            // update entity lifetime
            entity.LifeTime += delta;

            // check collision
            WeaponEntityManager.CheckCollision(entity, MaxCollisionsPerFrame, EntityTickDelay, OnCollision);

            // check expired 
            if (
                (entity.MaxPierceCount < 0) || // no pierce left
                (entity.LifeTime > entity.MaxLifeTime) || // lifetime elapsed
                (entity.Position.DistanceTo(GlobalPosition) > entity.MaxDistance) // too far
            )
            { expiredEntities.Add(entity); }
        }

        // destroy expired entities
        if (expiredEntities.Count > 0) { expiredEntities.ForEach(DestroyEntity); }
    }

    // -------------------------------------------- Debug --------------------------------------------
    public abstract string DebugGetCategoryTitle();

    public override DebugCategory DebugCreateCategory()
    {
        DebugCategory category = new($"{DebugGetCategoryTitle()} weapon");

        category.CreateDivider("Weapon Stats");
        category.CreateLabelField("entity_trajectory", "Trajectory.", Trajectory.ToString());
        category.CreateLabelField("entities_count", "Entities", EntitiesList.Count.ToString());

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