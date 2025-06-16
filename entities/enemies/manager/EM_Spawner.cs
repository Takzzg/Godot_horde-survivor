using Godot;

public partial class EM_Spawner(EnemiesManager manager) : BaseComponent<EnemiesManager>(manager)
{
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

    public void SpawnAroundPlayer()
    {
        Vector2 pos = Utils.GetRandomPointOnCircle(GameManager.Instance.Player.Position, GameManager.RENDER_DISTANCE);

        BasicEnemy enemy = new() { Position = pos };
        Parent.SpawnEnemy(enemy);
    }

    public override void DebugCreateSubCategory(DebugCategory category)
    {
        category.CreateDivider("Spawner");
        category.CreateLabelField("timer_running", "Timer", Running ? "ON" : "OFF");
        category.CreateLabelField("timer_delay", "Delay", TimerDelay.ToString());
    }
}