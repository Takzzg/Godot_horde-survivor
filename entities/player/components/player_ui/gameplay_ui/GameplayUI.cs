using System;
using Godot;
public partial class GameplayUI : Control
{
    [Export]
    private HealthBar _healthBar;
    [Export]
    private Label _timeLabel;
    [Export]
    private Label _killCountLabel;
    [Export]
    private ProgressBar _experienceBar;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        UpdateExperienceBar();
        UpdateKillCount(GameManager.Instance.Player.PlayerStats.KillsCount);

        GameManager.Instance.Player.PlayerExperience.PlayerExperienceGain += UpdateExperienceBar;
        GameManager.Instance.Player.PlayerExperience.PlayerLevelUp += UpdateExperienceBar;
    }

    public override void _ExitTree()
    {
        GameManager.Instance.Player.PlayerExperience.PlayerExperienceGain -= UpdateExperienceBar;
        GameManager.Instance.Player.PlayerExperience.PlayerLevelUp -= UpdateExperienceBar;
    }

    public void UpdateHealthBar(int current, int max)
    {
        _healthBar.UpdateValue(current, max);
    }

    public void UpdateExperienceBar()
    {
        int CurrentXP = GameManager.Instance.Player.PlayerExperience.CurrentExperience;
        int requiredXP = GameManager.Instance.Player.PlayerExperience.ExperienceToNextLevel;
        double percentage = CurrentXP * 100 / requiredXP;

        GD.Print($"updating experience bar {percentage}");
        _experienceBar.Value = percentage;
    }

    public void UpdateTimeLabel(double time)
    {
        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);
        _timeLabel.Text = $"{minutes:00}:{seconds:00}";
    }

    public void UpdateKillCount(int count)
    {
        _killCountLabel.Text = count.ToString();
    }
}
