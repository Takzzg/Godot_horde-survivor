using Godot;
using System;

public partial class MainUI : Control
{
    [Export]
    public GameplayUI GameplayUI;
    [Export]
    public Control DeathUI;

    public void OnPlayerDeath()
    {
        GameplayUI.Visible = false;
        DeathUI.Visible = true;
    }

    public void SetUpSignals()
    {
        DeathUI.Visible = false;
        GameManager.Instance.Player.PlayerDeath += OnPlayerDeath;

        GameplayUI.SetUpSignals();
    }
}
