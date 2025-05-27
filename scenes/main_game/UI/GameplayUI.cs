using Godot;
using System;

public partial class GameplayUI : Control
{
    [Export]
    public Label HealthLabel;
    [Export]
    public Label EnemiesLabel;
    [Export]
    public Label PosLabel;
    [Export]
    public Label LevelLabel;
    [Export]
    public Label CurrentXPLabel;
    [Export]
    public Label NextLevelXPLabel;

    public override void _Ready()
    {
        UpdateHealthLabel();
        UpdatePosLabel();
        UpdateEnemiesCountLabel();
        UpdateLevelLabel();
        UpdateCurrentXPLabel();

        GameManager.Instance.Player.PlayerMovement.PlayerMove += UpdatePosLabel;
        GameManager.Instance.Player.PlayerHealth.PlayerReceiveDamage += (_) => UpdateHealthLabel();
        GameManager.Instance.Player.PlayerExperience.PlayerLevelUp += UpdateLevelLabel;
        GameManager.Instance.Player.PlayerExperience.PlayerExperienceGain += UpdateCurrentXPLabel;
    }

    public void UpdateHealthLabel()
    {
        HealthLabel.Text = GameManager.Instance.Player.PlayerHealth.Health.ToString();
    }

    public void UpdatePosLabel()
    {
        Vector2 pos = GameManager.Instance.Player.Position;
        PosLabel.Text = $"({float.Round(pos.X, 1)}, {float.Round(pos.Y, 1)})";
    }

    public void UpdateEnemiesCountLabel()
    {
        EnemiesLabel.Text = GameManager.Instance.EnemiesManager.EnemiesList.Count.ToString();
    }

    public void UpdateLevelLabel()
    {
        LevelLabel.Text = GameManager.Instance.Player.PlayerExperience.PlayerLevel.ToString();
        NextLevelXPLabel.Text = GameManager.Instance.Player.PlayerExperience.ExperienceToNextLevel.ToString();
        UpdateCurrentXPLabel();
    }

    public void UpdateCurrentXPLabel()
    {
        CurrentXPLabel.Text = GameManager.Instance.Player.PlayerExperience.CurrentExperience.ToString();
    }
}
