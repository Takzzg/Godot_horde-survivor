using Godot;

public partial class PlayerStats(PlayerScene player) : BasePlayerComponent(player)
{
    public double TimeAlive { get; private set; } = 0;
    public float DamageReceived { get; private set; } = 0;
    public float DamageDealt { get; private set; } = 0;
    public int KillCount { get; private set; } = 0;
    public float DistanceTraveled { get; private set; } = 0;
    public float ExperienceGathered { get; private set; } = 0;

    public void IncreaseTimeAlive(double delta)
    {
        TimeAlive += delta;
        _player.PlayerUI.GameplayUI.UpdateTimeLabel(TimeAlive);

        int minutes = (int)(TimeAlive / 60);
        int seconds = (int)(TimeAlive % 60);
        _player.DebugTryUpdateField("time_alive", $"{minutes:00}:{seconds:00}");
    }

    public void IncreaseDamageReceived(int count)
    {
        DamageReceived += count;
        _player.DebugTryUpdateField("damage_received", DamageReceived.ToString());
    }

    public void IncreaseDamageDealt(float count)
    {
        DamageDealt += count;
        _player.DebugTryUpdateField("damage_dealt", DamageDealt.ToString());
    }

    public void IncreaseKillCount(int count)
    {
        KillCount += count;
        _player.PlayerUI.GameplayUI.UpdateKillCount(KillCount);
        _player.DebugTryUpdateField("kill_count", KillCount.ToString());
    }

    public void IncreaseDistanceTraveled(float distance)
    {
        DistanceTraveled += distance;
        _player.DebugTryUpdateField("dist_traveled", DistanceTraveled.ToString("0.0"));
    }

    public void IncreaseExperienceGathered(float distance)
    {
        ExperienceGathered += distance;
        _player.DebugTryUpdateField("exp_gained", ExperienceGathered.ToString());
    }

    // -------------------------------------------- Debug --------------------------------------------
    public override void DebugCreateSubCategory(DebugCategory category)
    {
        category.CreateDivider("Stats");
        category.CreateLabelField("time_alive", "Time alive", TimeAlive.ToString("00:00"));
        category.CreateLabelField("damage_received", "Dmg received", DamageReceived.ToString());
        category.CreateLabelField("damage_dealt", "Dmg dealt", DamageDealt.ToString());
        category.CreateLabelField("kill_count", "Kill count", KillCount.ToString());
        category.CreateLabelField("dist_traveled", "Dist. traveled", DistanceTraveled.ToString("0.0"));
        category.CreateLabelField("exp_gained", "XP gained", ExperienceGathered.ToString());
    }
}