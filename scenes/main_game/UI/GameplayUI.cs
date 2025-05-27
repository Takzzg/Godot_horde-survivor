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

    public override void _Ready()
    {
        UpdateHealthLabel();
        UpdatePosLabel();
        UpdateEnemiesCountLabel();

        GameManager.Instance.Player.PlayerMovement.PlayerMove += UpdatePosLabel;
        GameManager.Instance.Player.PlayerHealth.PlayerReceiveDamage += (_) => UpdateHealthLabel();
    }

    public void UpdateHealthLabel()
    {
        HealthLabel.Text = GameManager.Instance.Player.PlayerHealth.Health.ToString();
    }

    public void UpdatePosLabel()
    {
        Vector2 pos = GameManager.Instance.Player.Position;
        PosLabel.Text = $"({float.Round(pos.X, 2)}, {float.Round(pos.Y, 2)})";
    }

    public void UpdateEnemiesCountLabel()
    {
        EnemiesLabel.Text = GameManager.Instance.EnemiesManager.EnemiesList.Count.ToString();
    }
}
