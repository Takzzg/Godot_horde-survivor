using Godot;
using System;

public partial class MainUI : Control
{
    [Export]
    public Control DeathUI;

    public override void _Ready()
    {
        DeathUI.Visible = false;
        GameManager.Instance.Player.PlayerHealth.PlayerDeath += OnPlayerDeath;
    }

    public void OnPlayerDeath()
    {
        DeathUI.Visible = true;
    }
}
