using Godot;
using System;

public partial class MainUI : Control
{
    [Export]
    public GameplayUI GameplayUI;
    [Export]
    public Control DeathUI;

    public override void _Ready()
    {
        DeathUI.Visible = false;
        GameManager.Instance.Player.PlayerHealth.PlayerDeath += OnPlayerDeath;
    }

    public void OnPlayerDeath()
    {
        GameplayUI.Visible = false;
        DeathUI.Visible = true;
    }
}
