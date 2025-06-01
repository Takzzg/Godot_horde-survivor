using Godot;

public partial class PlayerStats(PlayerScene player) : BasePlayerComponent(player)
{
    public double TimeAlive { get; private set; } = 0;
    public int DamageReceived { get; private set; } = 0;
    public int DamageDealt { get; private set; } = 0;
    public int KillCount { get; private set; } = 0;
    public float DistanceTraveled { get; private set; } = 0;
    public float ExperienceGathered { get; private set; } = 0;

    public override DebugCategory DebugCreateCategory()
    {
        DebugCategory category = new("Player Stats");

        category.CreateLabelField("time_alive", "Time alive", TimeAlive.ToString("00:00"));
        category.CreateLabelField("damage_received", "Dmg received", DamageReceived.ToString());
        category.CreateLabelField("damage_dealt", "Dmg dealt", DamageDealt.ToString());
        category.CreateLabelField("kill_count", "Kill count", KillCount.ToString());
        category.CreateLabelField("dist_traveled", "Dist. traveled", DistanceTraveled.ToString("0.0"));
        category.CreateLabelField("exp_gained", "XP gained", ExperienceGathered.ToString());

        return category;
    }

    public void IncreaseTimeAlive(double delta)
    {
        TimeAlive += delta;
        _player.PlayerUI.GameplayUI.UpdateTimeLabel(TimeAlive);

        int minutes = (int)(TimeAlive / 60);
        int seconds = (int)(TimeAlive % 60);
        DebugTryUpdateField("time_alive", $"{minutes:00}:{seconds:00}");
    }

    public void IncreaseDamageReceived(int count)
    {
        DamageReceived += count;
        DebugTryUpdateField("damage_received", DamageReceived.ToString());
    }

    public void IncreaseDamageDealt(int count)
    {
        DamageDealt += count;
        DebugTryUpdateField("damage_dealt", DamageDealt.ToString());
    }

    public void IncreaseKillCount(int count)
    {
        KillCount += count;
        _player.PlayerUI.GameplayUI.UpdateKillCount(KillCount);
        DebugTryUpdateField("kill_count", KillCount.ToString());
    }

    public void IncreaseDistanceTraveled(float distance)
    {
        DistanceTraveled += distance;
        DebugTryUpdateField("dist_traveled", DistanceTraveled.ToString("0.0"));
    }

    public void IncreaseExperienceGathered(float distance)
    {
        ExperienceGathered += distance;
        DebugTryUpdateField("exp_gained", ExperienceGathered.ToString());
    }
}