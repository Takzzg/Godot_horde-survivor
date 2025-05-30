using System;

public partial class PlayerStats : BasePlayerComponent
{
    private DebugCategoryComponent _debug;

    public double TimeAlive { get; private set; } = 0;
    public int DamageReceived { get; private set; } = 0;
    public int DamageDealt { get; private set; } = 0;
    public int KillsCount { get; private set; } = 0;
    public float DistanceTraveled { get; private set; } = 0;
    public float ExperienceGathered { get; private set; } = 0;

    public PlayerStats(PlayerScene player) : base(player)
    {
        _debug = new DebugCategoryComponent((instance) =>
        {
            // create debug cat
            instance.TryCreateCategory(new DebugManager.DebugCategory("player_stats", "Stats"));

            // create debug fields
            instance.TryCreateField("time_alive", "Time alive", TimeAlive.ToString("00:00"));
            instance.TryCreateField("damage_received", "Dmg received", DamageReceived.ToString());
            instance.TryCreateField("damage_dealt", "Dmg dealt", DamageDealt.ToString());
            instance.TryCreateField("kills_count", "Kills", KillsCount.ToString());
            instance.TryCreateField("dist_traveled", "Dist. traveled", DistanceTraveled.ToString("0.0"));
            instance.TryCreateField("exp_gained", "XP gained", ExperienceGathered.ToString());
        });
        AddChild(_debug);
    }

    public override void _ExitTree()
    {
        _debug.QueueFree();
    }

    public void IncreaseTimeAlive(double delta)
    {
        TimeAlive += delta;
        GameManager.Instance.Player.PlayerUI.GameplayUI.UpdateTimeLabel(TimeAlive);

        int minutes = (int)(TimeAlive / 60);
        int seconds = (int)(TimeAlive % 60);
        _debug.TryUpdateField("time_alive", $"{minutes:00}:{seconds:00}");
    }

    public void IncreaseDamageReceived(int count)
    {
        DamageReceived += count;
        _debug.TryUpdateField("damage_received", DamageReceived.ToString());
    }

    public void IncreaseDamageDealt(int count)
    {
        DamageDealt += count;
        _debug.TryUpdateField("damage_dealt", DamageDealt.ToString());
    }

    public void IncreaseKillsCount(int count)
    {
        KillsCount += count;
        _debug.TryUpdateField("kills_count", KillsCount.ToString());
        GameManager.Instance.Player.PlayerUI.GameplayUI.UpdateKillCount(KillsCount);
    }

    public void IncreaseDistanceTraveled(float distance)
    {
        DistanceTraveled += distance;
        _debug.TryUpdateField("dist_traveled", DistanceTraveled.ToString("0.0"));
    }

    public void IncreaseExperienceGathered(float distance)
    {
        ExperienceGathered += distance;
        _debug.TryUpdateField("exp_gained", ExperienceGathered.ToString());
    }
}