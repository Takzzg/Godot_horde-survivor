using Godot;

public partial class PlayerUI(PlayerScene player) : BasePlayerComponent(player)
{
    [Export]
    public Control DeathUI;
    [Export]
    public GameplayUI GameplayUI;

    public override void _Ready()
    {
        CanvasLayer layer = new();
        AddChild(layer);

        DeathUI = ResourcePaths.GetInstanceFromEnum<Control>(ResourcePaths.EnumPathsDictionary.PLAYER_DEATH_UI);
        layer.AddChild(DeathUI);

        GameplayUI = ResourcePaths.GetInstanceFromEnum<GameplayUI>(ResourcePaths.EnumPathsDictionary.PLAYER_GAMEPLAY_UI);
        layer.AddChild(GameplayUI);

        DeathUI.Visible = false;
        GameplayUI.UpdateHealthBar(_player.PlayerHealth.Health, _player.PlayerHealth.MaxHealth);

        _player.PlayerHealth.PlayerDeath += OnPlayerDeath;
        _player.PlayerHealth.PlayerReceiveDamage += (_) => GameplayUI.UpdateHealthBar(_player.PlayerHealth.Health, _player.PlayerHealth.MaxHealth);
    }

    public void OnPlayerDeath()
    {
        DeathUI.Visible = true;
        GameplayUI.Visible = false;
    }
}
