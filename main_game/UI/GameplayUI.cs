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

    public void UpdateHealthLabel(int _)
    {
        HealthLabel.Text = GameManager.Instance.MainGame.Player.Health.ToString();
    }
    public void UpdateEnemiesCountLabel(int count)
    {
        EnemiesLabel.Text = count.ToString();
    }
    public void UpdatePosLabel()
    {
        Vector2 pos = GameManager.Instance.MainGame.Player.Position;
        PosLabel.Text = $"({float.Round(pos.X, 2)}, {float.Round(pos.Y, 2)})";
    }

    public void SetUpSignals()
    {
        GameManager.Instance.MainGame.Player.PlayerMove += UpdatePosLabel;
        GameManager.Instance.MainGame.Player.PlayerReceiveDamage += UpdateHealthLabel;
    }
}
