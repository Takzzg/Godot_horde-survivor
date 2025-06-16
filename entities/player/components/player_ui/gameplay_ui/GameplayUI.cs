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

    public void UpdateHealthBar(float current, int max)
    {
        _healthBar.UpdateValue(current, max);
    }

    public void UpdateExperienceBar(int current, int required)
    {
        double percentage = current * 100 / required;
        _experienceBar.Value = percentage;
    }

    public void UpdateTimeLabel(double time)
    {
        _timeLabel.Text = Utils.DeltaToTimeString(time);
    }

    public void UpdateKillCount(int count)
    {
        _killCountLabel.Text = count.ToString();
    }

    public void UpdateValues(PlayerScene player)
    {
        UpdateHealthBar(player.PlayerHealth.Health, player.PlayerHealth.MaxHealth);
        UpdateExperienceBar(player.PlayerExperience.CurrentExperience, player.PlayerExperience.RequiredExperience);
        UpdateKillCount(player.PlayerStats.KillCount);
    }
}
