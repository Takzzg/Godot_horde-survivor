using System;
using System.Collections.Generic;
using Godot;

public partial class EnemiesManager : Node2D
{
    public List<BasicEnemy> EnemiesList = [];

    public int SharedHitBoxSize;
    public int SharedHurtBoxSize;
    public CircleShape2D SharedHitBox;
    public CircleShape2D SharedHurtBox;
    public Timer Timer;
    public double TimerDelay = 0.125;

    public bool ProcessMovement = true;

    public EnemiesManager(int size)
    {
        SharedHitBoxSize = size;
        SharedHurtBoxSize = size + 4;

        SharedHitBox = new CircleShape2D() { Radius = SharedHitBoxSize };
        SharedHurtBox = new CircleShape2D() { Radius = SharedHurtBoxSize };

        Timer = new Timer() { Autostart = false, OneShot = false, WaitTime = TimerDelay };
        Timer.Timeout += SpawnEnemyAroundPlayer;
        AddChild(Timer);

        GameManager.Instance.Player.PlayerHealth.PlayerDeath += OnPlayerDeath;
    }

    public override void _ExitTree()
    {
        EnemiesList.ForEach(DestroyEnemy);
    }

    public override void _PhysicsProcess(double delta)
    {
        MoveEnemies();
    }

    public void SpawnEnemyAroundPlayer()
    {
        Vector2 pos = Utils.GetRandomPointOnCircle(GameManager.Instance.Player.Position, GameManager.RENDER_DISTANCE);
        BasicEnemy enemy = new(pos, 50, 25, 5);
        SpawnEnemy(enemy);
    }

    public void SpawnEnemy(BasicEnemy enemy)
    {
        if (EnemiesList.Count >= 1500) return;

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
        GameManager.Instance.UI.GameplayUI.UpdateEnemiesCountLabel();
    }

    public void MoveEnemies()
    {
        Vector2 playerPos = GameManager.Instance.Player.Position;

        for (int i = 0; i < EnemiesList.Count; i++)
        {
            BasicEnemy enemy = EnemiesList[i];

            if (ProcessMovement)
            {
                Vector2 dir = enemy.Position.DirectionTo(playerPos).Normalized() * enemy.Speed;
                PhysicsServer2D.BodySetState(enemy.BodyRid, PhysicsServer2D.BodyState.LinearVelocity, dir);
            }

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

    public void EnemyReceiveDamage(BasicEnemy enemy, int damage)
    {
        enemy.Health -= damage;
        if (enemy.Health > 0) return;

        DestroyEnemy(enemy);
        EnemiesList.Remove(enemy);

        GameManager.Instance.UI.GameplayUI.UpdateEnemiesCountLabel();
        GameManager.Instance.ExperienceManager.SpawnExperienceItem(enemy.Position);
    }

    public void DestroyEnemy(BasicEnemy enemy)
    {
        PhysicsServer2D.FreeRid(enemy.BodyRid);
        PhysicsServer2D.FreeRid(enemy.HurtboxRid);
        RenderingServer.FreeRid(enemy.CanvasItemRid);
    }

    private void OnPlayerDeath()
    {
        Timer.Stop();

        EnemiesList.ForEach((enemy) =>
        {
            Timer timer = new() { Autostart = true, OneShot = true, WaitTime = GameManager.Instance.RNG.RandfRange(0.5f, 1.5f) };
            timer.Timeout += () =>
            {
                DestroyEnemy(enemy);
                timer.QueueFree();
            };
            AddChild(timer);
        });

        EnemiesList.Clear();
    }
}