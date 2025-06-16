using System;
using System.Collections.Generic;
using Godot;

public partial class EnemiesManager : DebuggerNode
{
    public EM_Spawner Spawner;
    public EM_SharedResources SharedResources;
    public EM_Difficulty Difficulty;

    public EnemiesManager()
    {
        Spawner = new EM_Spawner(this);
        SharedResources = new EM_SharedResources(this);
        Difficulty = new EM_Difficulty(this);

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
            Timer timer = new()
            {
                Autostart = true,
                OneShot = true,
                WaitTime = GameManager.Instance.RNG.RandfRange(0.5f, 1.5f)
            };
            timer.Timeout += timer.QueueFree;
            timer.TreeExiting += enemy.FreeEntityRids;
            AddChild(timer);
        });

        EnemiesList.Clear();

        // update debug label
        DebugTryUpdateField("enemies_count", $"{EnemiesList.Count} / {Difficulty.MaxEnemies}");
    }

    // -------------------------------------------- Enemies --------------------------------------------
    public List<EnemyEntity> EnemiesList = [];

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
        DebugTryUpdateField("enemies_count", $"{EnemiesList.Count} / {Difficulty.MaxEnemies}");
    }

    public void DestroyAllEnemies()
    {
        EnemiesList.ForEach(enemy => enemy.FreeEntityRids());
        EnemiesList.Clear();
        DebugTryUpdateField("enemies_count", $"{EnemiesList.Count} / {Difficulty.MaxEnemies}");
    }

    // -------------------------------------------- DEBUG --------------------------------------------
    public override DebugCategory DebugCreateCategory()
    {
        DebugCategory category = new("Enemies Manager");
        category.CreateLabelField("enemies_count", "Count", $"{EnemiesList.Count}/{Difficulty.MaxEnemies}");

        Spawner.DebugCreateSubCategory(category);
        SharedResources.DebugCreateSubCategory(category);
        Difficulty.DebugCreateSubCategory(category);

        return category;
    }
}