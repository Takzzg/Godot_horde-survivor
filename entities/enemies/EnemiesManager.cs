using System;
using System.Collections.Generic;
using Godot;

public partial class EnemiesManager : DebugNode2D
{
    // enemies
    public const int MAX_ENEMIES = 1500;
    public List<BasicEnemy> EnemiesList = [];

    // shared resources
    public int SharedHitBoxSize;
    public int SharedHurtBoxSize;
    public CircleShape2D SharedHitBox;
    public CircleShape2D SharedHurtBox;

    // spawner
    public Timer Timer;
    public double TimerDelay = 0.125;

    public EnemiesManager(int size)
    {
        SharedHitBoxSize = size;
        SharedHurtBoxSize = size + 4;

        SharedHitBox = new CircleShape2D() { Radius = SharedHitBoxSize };
        SharedHurtBox = new CircleShape2D() { Radius = SharedHurtBoxSize };

        Timer = new Timer() { Autostart = false, OneShot = false, WaitTime = TimerDelay };
        Timer.Timeout += SpawnEnemyAroundPlayer;
        AddChild(Timer);

        TreeExiting += () => { EnemiesList.ForEach(FreeEnemyEntityRids); };
    }

    public override void _PhysicsProcess(double delta)
    {
        MoveEnemies();
    }

    public override DebugCategory DebugCreateCategory()
    {
        DebugCategory category = new("Enemies Manager");
        category.CreateLabelField("enemies_count", "Count", $"{EnemiesList.Count}/{MAX_ENEMIES}");
        return category;
    }

    public void SpawnEnemyAroundPlayer()
    {
        Vector2 pos = Utils.GetRandomPointOnCircle(GameManager.Instance.Player.Position, GameManager.RENDER_DISTANCE);
        BasicEnemy enemy = new(pos, 50, 25, 5, 1);
        SpawnEnemy(enemy);
        DebugTryUpdateField("enemies_count", $"{EnemiesList.Count} / {MAX_ENEMIES}");
    }

    public void SpawnEnemy(BasicEnemy enemy)
    {
        if (EnemiesList.Count >= MAX_ENEMIES) return;

        Transform2D posTransform = new(0, enemy.Position);

        // create body
        enemy.BodyRid = PhysicsServer2D.BodyCreate();
        PhysicsServer2D.BodyAddShape(enemy.BodyRid, SharedHitBox.GetRid(), Transform2D.Identity);
        PhysicsServer2D.BodySetSpace(enemy.BodyRid, GetWorld2D().Space);
        PhysicsServer2D.BodySetState(enemy.BodyRid, PhysicsServer2D.BodyState.Transform, posTransform);
        PhysicsServer2D.BodySetMode(enemy.BodyRid, PhysicsServer2D.BodyMode.RigidLinear);
        PhysicsServer2D.BodySetParam(enemy.BodyRid, PhysicsServer2D.BodyParameter.GravityScale, 0);
        PhysicsServer2D.BodySetParam(enemy.BodyRid, PhysicsServer2D.BodyParameter.Friction, 0);
        PhysicsServer2D.BodySetCollisionLayer(enemy.BodyRid, 2); // 2 = enemy layer
        PhysicsServer2D.BodySetCollisionMask(enemy.BodyRid, 3); // 1 = player layer, 2 = enemy layer

        // create hurtbox
        enemy.HurtboxRid = PhysicsServer2D.AreaCreate();
        PhysicsServer2D.AreaAddShape(enemy.HurtboxRid, SharedHurtBox.GetRid(), Transform2D.Identity);
        PhysicsServer2D.AreaSetTransform(enemy.HurtboxRid, posTransform);
        PhysicsServer2D.AreaSetSpace(enemy.HurtboxRid, GetWorld2D().Space);
        PhysicsServer2D.AreaSetCollisionLayer(enemy.HurtboxRid, 4); // 4 = enemy hurtbox layer
        PhysicsServer2D.AreaSetCollisionMask(enemy.HurtboxRid, 8); // 8 = bullet layer
        PhysicsServer2D.AreaSetMonitorable(enemy.HurtboxRid, true);

        // create sprite
        enemy.CanvasItemRid = RenderingServer.CanvasItemCreate();
        RenderingServer.CanvasItemSetParent(enemy.CanvasItemRid, GetCanvasItem());
        RenderingServer.CanvasItemSetTransform(enemy.CanvasItemRid, posTransform);
        RenderingServer.CanvasItemAddCircle(enemy.CanvasItemRid, Vector2.Zero, SharedHitBoxSize, Colors.Black);
        // RenderingServer.CanvasItemAddCircle(enemy.CanvasItemRid, Vector2.Zero, SharedHurtBoxSize, new Color(Colors.Pink, 0.125f));

        // GD.Print($"spawning enemy at {pos}");
        EnemiesList.Add(enemy);
    }

    public void MoveEnemies()
    {
        Vector2 playerPos = GameManager.Instance.Player.Position;

        for (int i = 0; i < EnemiesList.Count; i++)
        {
            BasicEnemy enemy = EnemiesList[i];

            Vector2 dir = (enemy.Speed == 0) ? Vector2.Zero : enemy.Position.DirectionTo(playerPos).Normalized() * enemy.Speed;
            PhysicsServer2D.BodySetState(enemy.BodyRid, PhysicsServer2D.BodyState.LinearVelocity, dir);

            Transform2D posTransform = (Transform2D)PhysicsServer2D.BodyGetState(enemy.BodyRid, PhysicsServer2D.BodyState.Transform);
            PhysicsServer2D.AreaSetTransform(enemy.HurtboxRid, posTransform);
            RenderingServer.CanvasItemSetTransform(enemy.CanvasItemRid, posTransform);

            enemy.Position = posTransform.Origin;
        }
    }

    public BasicEnemy FindEnemyByAreaRid(Rid areaRid)
    {
        foreach (BasicEnemy enemy in EnemiesList) { if (enemy.HurtboxRid.Equals(areaRid)) return enemy; }
        throw new Exception($"no enemy found with HurtBoxRid = {areaRid}");
    }

    public BasicEnemy FindEnemyByBodyRid(Rid bodyRid)
    {
        foreach (BasicEnemy enemy in EnemiesList) { if (enemy.BodyRid.Equals(bodyRid)) return enemy; }
        throw new Exception($"no enemy found with BodyRid = {bodyRid}");
    }

    public (bool enemyDied, int damageDealt) EnemyReceiveDamage(BasicEnemy enemy, int damage)
    {
        if (enemy.Health - damage > 0)
        {
            enemy.Health -= damage;
            return (false, damage); // enemy did not die
        }

        EnemiesList.Remove(enemy);
        FreeEnemyEntityRids(enemy);

        GameManager.Instance.ExperienceManager.QueueExperienceEntitySpawn(enemy.Position, enemy.ExperienceDropped);

        // update debug label
        DebugTryUpdateField("enemies_count", $"{EnemiesList.Count} / {MAX_ENEMIES}");

        return (true, enemy.Health); // enemy died
    }

    public static void FreeEnemyEntityRids(BasicEnemy enemy)
    {
        PhysicsServer2D.FreeRid(enemy.BodyRid);
        PhysicsServer2D.FreeRid(enemy.HurtboxRid);
        RenderingServer.FreeRid(enemy.CanvasItemRid);
    }

    public void OnPlayerDeath()
    {
        Timer.Stop();

        EnemiesList.ForEach((enemy) =>
        {
            PhysicsServer2D.BodySetMode(enemy.BodyRid, PhysicsServer2D.BodyMode.Static);
            Timer timer = new() { Autostart = true, OneShot = true, WaitTime = GameManager.Instance.RNG.RandfRange(0.5f, 1.5f) };
            timer.Timeout += () =>
            {
                FreeEnemyEntityRids(enemy);
                timer.QueueFree();
            };
            AddChild(timer);
        });

        EnemiesList.Clear();

        // update debug label
        DebugTryUpdateField("enemies_count", $"{EnemiesList.Count} / {MAX_ENEMIES}");
    }
}