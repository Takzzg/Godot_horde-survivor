using Godot;

public partial class EM_Spawner(EnemiesManager manager) : EM_BaseComponent(manager)
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
        DebugTryUpdateField("timer_running", Running ? "ON" : "OFF");
    }

    public void SpawnAroundPlayer()
    {
        Vector2 pos = Utils.GetRandomPointOnCircle(GameManager.Instance.Player.Position, GameManager.RENDER_DISTANCE);

        BasicEnemy enemy = new() { Position = pos };
        _manager.SpawnEnemy(enemy);
    }

    public override DebugCategory DebugCreateCategory()
    {
        DebugCategory category = new("EM spawner");
        category.CreateLabelField("timer_running", "Timer", Running ? "ON" : "OFF");
        return category;
    }
}