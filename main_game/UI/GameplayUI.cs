using Godot;
using System;

public partial class GameplayUI : Control
{
    [Export]
    public Label HealthLabel;

    public void UpdateHealthLabel(int amount)
    {
        HealthLabel.Text = amount.ToString();
    }

    public void SetUpSignals()
    {
        GameManager.Instance.Player.PlayerReceiveDamage += (amount) => UpdateHealthLabel(GameManager.Instance.Player.Health);
    }
}
