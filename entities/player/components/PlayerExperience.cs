using Godot;
using Godot.Collections;

public partial class PlayerExperience : BasePlayerComponent
{
    public int PlayerLevel = 1;
    public int CurrentExperience = 0;
    public int RequiredExperience => PlayerLevel * 2;

    public int ExperiencePickUpRadius = 20;
    public Shape2D ExperiencePickUpShape;

    public PlayerExperience(PlayerScene player) : base(player)
    {
        ExperiencePickUpShape = new CircleShape2D() { Radius = ExperiencePickUpRadius };
    }

    public override void _PhysicsProcess(double delta)
    {
        PhysicsShapeQueryParameters2D query = new()
        {
            Shape = ExperiencePickUpShape,
            Transform = new(0, _player.Position),
            CollisionMask = 16, // 16 = experience layer
            CollideWithAreas = true,
            CollideWithBodies = false,
        };

        Array<Dictionary> collisions = GetWorld2D().DirectSpaceState.IntersectShape(query);
        foreach (Dictionary col in collisions)
        {
            ExperienceEntity entity = GameManager.Instance.ExperienceManager.FindExperienceEntityFromAreaRid((Rid)col["rid"]);
            GameManager.Instance.ExperienceManager.DestroyExperienceEntity(entity);
            GainExperience(entity.Amount);
        }
    }

    private void GainExperience(int amount)
    {
        int xpRequired = RequiredExperience - CurrentExperience;
        int experienceGained = (amount > xpRequired) ? xpRequired : amount;

        CurrentExperience += experienceGained;
        _player.PlayerStats.IncreaseExperienceGathered(experienceGained);

        // GD.Print($"player gain experience {amount} ({CurrentExperience}/{ExperienceToNextLevel})");
        if (CurrentExperience >= RequiredExperience) LevelUp();

        DebugTryUpdateField("current_xp", $"{CurrentExperience} / {RequiredExperience}");
        _player.PlayerUI.GameplayUI.UpdateExperienceBar(CurrentExperience, RequiredExperience);
    }

    private void LevelUp()
    {
        PlayerLevel += 1;
        CurrentExperience = 0;

        // GD.Print($"player level up {PlayerLevel}");
        DebugTryUpdateField("player_level", PlayerLevel.ToString());
    }

    public override DebugCategory DebugCreateCategory()
    {
        DebugCategory category = new("Player Experience");

        category.CreateLabelField("player_level", "Level", PlayerLevel.ToString());
        category.CreateLabelField("current_xp", "XP", $"{CurrentExperience} / {RequiredExperience}");

        return category;
    }
}