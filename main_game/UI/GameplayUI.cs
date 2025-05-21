using Godot;
using System;

public partial class GameplayUI : Control
{
    [Export]
    public Label HealthLabel;
    [Export]
    public Label EnemiesLabel;

    public void UpdateHealthLabel(int _)
    {
        HealthLabel.Text = GameManager.Instance.Player.Health.ToString();
    }
    public void UpdateEnemiesCountLabel()
    {
        EnemiesLabel.Text = GameManager.Instance.Spawner.EnemiesContainer.GetChildCount().ToString();
    }

    public void SetUpSignals()
    {
        GameManager.Instance.Player.PlayerReceiveDamage += UpdateHealthLabel;
    }
}
