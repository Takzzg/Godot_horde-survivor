using Godot;
using Godot.Collections;

public partial class PlayerExperience : BasePlayerComponent
{
    public int PlayerLevel = 1;
    public int CurrentExperience = 0;
    public int ExperienceToNextLevel;

    public int ExperiencePickUpRadius = 20;
    public Shape2D ExperiencePickUpShape;

    [Signal]
    public delegate void PlayerExperienceGainEventHandler();
    [Signal]
    public delegate void PlayerLevelUpEventHandler();

    public PlayerExperience(PlayerScene player) : base(player)
    {
        ExperienceToNextLevel = GetExperienceToNextLevel();
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

    public int GetExperienceToNextLevel()
    {
        return PlayerLevel * 2;
    }

    public void GainExperience(int amount)
    {
        CurrentExperience += amount;

        if (CurrentExperience < ExperienceToNextLevel)
        {
            // GD.Print($"player gain experience {amount} ({CurrentExperience}/{ExperienceToNextLevel})");
            EmitSignal(SignalName.PlayerExperienceGain);
            return;
        }

        PlayerLevel += 1;
        ExperienceToNextLevel = GetExperienceToNextLevel();
        CurrentExperience = 0;

        // GD.Print($"player level up {PlayerLevel}");
        EmitSignal(SignalName.PlayerLevelUp);
    }
}