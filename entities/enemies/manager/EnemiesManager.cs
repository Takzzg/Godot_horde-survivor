using System;
using System.Collections.Generic;
using Godot;

public partial class EnemiesManager : DebuggerNode
{
    public EM_Spawner Spawner;
    public EM_SharedResources SharedResources;

    public EnemiesManager()
    {
        Spawner = new EM_Spawner(this);
        SharedResources = new EM_SharedResources(this);

        TreeExiting += () => { EnemiesList.ForEach(enemy => enemy.FreeEntityRids()); };
    }

    public override void _PhysicsProcess(double delta)
    {
        MoveEnemies();
    }

    // -------------------------------------------- Player --------------------------------------------
    public void OnPlayerDeath()
    {
        Spawner.Timer?.Stop();

        EnemiesList.ForEach((enemy) =>
        {
            PhysicsServer2D.BodySetMode(enemy.BodyRid, PhysicsServer2D.BodyMode.Static);
            Timer timer = new() { Autostart = true, OneShot = true, WaitTime = GameManager.Instance.RNG.RandfRange(0.5f, 1.5f) };
            timer.Timeout += () =>
            {
                enemy.FreeEntityRids();
                timer.QueueFree();
            };
            AddChild(timer);
        });

        EnemiesList.Clear();

        // update debug label
        DebugTryUpdateField("enemies_count", $"{EnemiesList.Count} / {MAX_ENEMIES}");
    }

    // -------------------------------------------- Enemies --------------------------------------------
    public const int MAX_ENEMIES = 1500;
    public List<EnemyEntity> EnemiesList = [];

    public void SpawnEnemy(EnemyEntity enemy)
    {
        if (EnemiesList.Count >= MAX_ENEMIES) return;

        // create shapes if needed
        CircleShape2D hitbox = SharedResources.RegisterCircleShape(enemy.HitboxRadius);
        CircleShape2D hurtbox = SharedResources.RegisterCircleShape(enemy.HurtboxRadius);

        // setup entity environment
        Transform2D posTransform = new(0, enemy.Position);
        Rid space = GetWorld2D().Space;
        Rid canvasItem = GetCanvasItem();

        // create physics server objects
        enemy.RegisterBody(posTransform, space, hitbox.GetRid());
        enemy.RegisterHurtbox(posTransform, space, hurtbox.GetRid());

        // create rendering server objects
        enemy.RegisterSprite(posTransform, canvasItem);

        EnemiesList.Add(enemy);
        DebugTryUpdateField("enemies_count", $"{EnemiesList.Count} / {MAX_ENEMIES}");
    }

    public void MoveEnemies()
    {
        Vector2 playerPos = GameManager.Instance.Player.Position;
        EnemiesList.ForEach(enemy => enemy.MoveTowardsTaget(playerPos));
    }

    public EnemyEntity FindEnemyByAreaRid(Rid areaRid)
    {
        foreach (EnemyEntity enemy in EnemiesList) { if (enemy.HurtboxRid.Equals(areaRid)) return enemy; }
        throw new Exception($"no enemy found with HurtBoxRid = {areaRid}");
    }

    public EnemyEntity FindEnemyByBodyRid(Rid bodyRid)
    {
        foreach (EnemyEntity enemy in EnemiesList) { if (enemy.BodyRid.Equals(bodyRid)) return enemy; }
        throw new Exception($"no enemy found with BodyRid = {bodyRid}");
    }

    public EnemyEntity EnemyReceiveDamage(Rid areaRid, float amount)
    {
        EnemyEntity enemy = FindEnemyByAreaRid(areaRid);

        enemy.ReceiveDamage(amount);
        if (!enemy.Alive) DestroyEnemy(enemy);

        return enemy;
    }

    private void DestroyEnemy(EnemyEntity enemy)
    {
        EnemiesList.Remove(enemy);
        DebugTryUpdateField("enemies_count", $"{EnemiesList.Count} / {MAX_ENEMIES}");
    }

    public void DestroyAllEnemies()
    {
        EnemiesList.ForEach(enemy => enemy.FreeEntityRids());
        EnemiesList.Clear();
        DebugTryUpdateField("enemies_count", $"{EnemiesList.Count} / {MAX_ENEMIES}");
    }

    // -------------------------------------------- DEBUG --------------------------------------------
    public override DebugCategory DebugCreateCategory()
    {
        DebugCategory category = new("Enemies Manager");
        category.CreateLabelField("enemies_count", "Count", $"{EnemiesList.Count}/{MAX_ENEMIES}");

        Spawner.DebugCreateSubCategory(category);
        SharedResources.DebugCreateSubCategory(category);

        return category;
    }
}