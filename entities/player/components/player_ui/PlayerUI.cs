using Godot;

public partial class PlayerUI : BasePlayerComponent
{
    [Export]
    public Control DeathUI;
    [Export]
    public GameplayUI GameplayUI;

    public PlayerUI(PlayerScene player) : base(player, false)
    {
        CanvasLayer layer = new();
        AddChild(layer);

        DeathUI = ResourcePaths.GetInstanceFromEnum<Control>(ResourcePaths.EnumPathsDictionary.PLAYER_DEATH_UI);
        layer.AddChild(DeathUI);

        GameplayUI = ResourcePaths.GetInstanceFromEnum<GameplayUI>(ResourcePaths.EnumPathsDictionary.PLAYER_GAMEPLAY_UI);
        layer.AddChild(GameplayUI);

        DeathUI.Visible = false;

        GameplayUI.UpdateHealthBar(_player.PlayerHealth.Health, _player.PlayerHealth.MaxHealth);
        GameplayUI.UpdateExperienceBar(_player.PlayerExperience.CurrentExperience, _player.PlayerExperience.RequiredExperience);
        GameplayUI.UpdateKillCount(_player.PlayerStats.KillCount);
    }

    public void ShowDeathUI()
    {
        DeathUI.Visible = true;
        GameplayUI.Visible = false;
    }
}
