using Godot;

public partial class EM_Spawner(EnemiesManager manager) : BaseComponent<EnemiesManager>(manager)
{
    // -------------------------------------------- Timer --------------------------------------------
    public bool Running { get; private set; } = false;
    public Timer Timer;
    public double TimerDelay = 0.125;

    public void StartTimer()
    {
        Timer = new Timer() { Autostart = true, OneShot = false, WaitTime = TimerDelay };
        Timer.Timeout += SpawnAroundPlayer;
        AddChild(Timer);

        Running = true;
        Parent.DebugTryUpdateField("timer_running", Running ? "ON" : "OFF");
    }

    // -------------------------------------------- Spawn --------------------------------------------
    public void SpawnEnemy(EnemyEntity enemy)
    {
        if (Parent.EnemiesList.Count >= EM_Difficulty.ENEMY_COUNT_HARD_CAP) return;

        // create shapes if needed
        CircleShape2D hitbox = Parent.SharedResources.RegisterCircleShape(enemy.HitboxRadius);
        CircleShape2D hurtbox = Parent.SharedResources.RegisterCircleShape(enemy.HurtboxRadius);

        // scale values with difficulty
        enemy.Health *= Parent.Difficulty.HealthMultiplierBloated / 10;
        enemy.Damage *= Parent.Difficulty.DamageMultiplierBloated / 10;

        // setup entity environment
        Transform2D posTransform = new(0, enemy.Position);
        Rid space = GetWorld2D().Space;
        Rid canvasItem = GetCanvasItem();

        // create physics server objects
        enemy.RegisterBody(posTransform, space, hitbox.GetRid());
        enemy.RegisterHurtbox(posTransform, space, hurtbox.GetRid());

        // create rendering server objects
        enemy.RegisterSprite(posTransform, canvasItem);

        Parent.EnemiesList.Add(enemy);
        Parent.DebugTryUpdateField("enemies_count", $"{Parent.EnemiesList.Count} / {Parent.Difficulty.MaxEnemies}");
    }

    public void SpawnAroundPlayer()
    {
        if (Parent.EnemiesList.Count >= Parent.Difficulty.MaxEnemies) return;

        Vector2 pos = Utils.GetRandomPointOnCircle(GameManager.Instance.Player.Position, GameManager.RENDER_DISTANCE);
        SpawnEnemy(new BasicEnemy() { Position = pos });
    }

    // -------------------------------------------- Debug --------------------------------------------
    public override void DebugCreateSubCategory(DebugCategory category)
    {
        category.CreateDivider("Spawner");
        category.CreateLabelField("timer_running", "Timer", Running ? "ON" : "OFF");
        category.CreateLabelField("timer_delay", "Delay", TimerDelay.ToString());
    }
}