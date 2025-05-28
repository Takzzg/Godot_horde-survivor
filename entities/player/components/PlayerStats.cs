using System;

public partial class PlayerStats : BasePlayerComponent
{
    private DebugCategoryComponent _debug;

    public int DamageReceived { get; private set; } = 0;
    public int DamageDealt { get; private set; } = 0;
    public int KOsCount { get; private set; } = 0;
    public float DistanceTraveled { get; private set; } = 0;
    public float ExperienceGathered { get; private set; } = 0;

    public PlayerStats(PlayerScene player) : base(player)
    {
        // create debug cat
        _debug = new DebugCategoryComponent((instance) => { instance.TryCreateCategory(new DebugManager.DebugCategory("player_stats", "Stats")); });

        // create debug fields
        _debug.TryCreateField("damage_received", "Dmg received", DamageReceived.ToString());
        _debug.TryCreateField("damage_dealt", "Dmg dealt", DamageDealt.ToString());
        _debug.TryCreateField("kos_count", "KOs count", KOsCount.ToString());
        _debug.TryCreateField("dist_traveled", "Dist. traveled", DistanceTraveled.ToString("0.0"));
        _debug.TryCreateField("exp_gained", "XP gained", ExperienceGathered.ToString());

        AddChild(_debug);
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

    public void IncreaseKOsCount(int count)
    {
        KOsCount += count;
        _debug.TryUpdateField("kos_count", KOsCount.ToString());
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