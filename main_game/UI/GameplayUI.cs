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
        HealthLabel.Text = GameManager.Instance.MainGame.Player.Health.ToString();
    }
    public void UpdateEnemiesCountLabel()
    {
        EnemiesLabel.Text = GameManager.Instance.MainGame.EnemySpawner.EnemiesContainer.GetChildCount().ToString();
    }

    public void SetUpSignals()
    {
        GameManager.Instance.MainGame.Player.PlayerReceiveDamage += UpdateHealthLabel;
    }
}
