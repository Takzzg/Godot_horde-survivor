using Godot;

public partial class PlayerUI : BasePlayerComponent
{
    private CanvasLayer _layer;

    public GameplayUI GameplayUI;
    public PauseMenu PauseMenu;
    public Control DeathUI;

    public PlayerUI(PlayerScene player) : base(player, false)
    {
        ProcessMode = ProcessModeEnum.Always;

        _layer = new CanvasLayer();
        AddChild(_layer);

        ShowGameplayUI();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("back"))
        {
            if (!_player.PlayerHealth.Alive) { SceneManager.Instance.ChangeScene(SceneManager.EnumScenes.MAIN_MENU); }
            else { _player.PlayerUI.TogglePauseMenu(); }
            GetViewport().SetInputAsHandled();
        }
    }

    public void ShowGameplayUI()
    {
        GameplayUI = ResourcePaths.GetSceneInstanceFromEnum<GameplayUI>(ResourcePaths.ScenePathsEnum.PLAYER_GAMEPLAY_UI);
        GameplayUI.UpdateValues(_player);
        _layer.AddChild(GameplayUI);
    }

    public void TogglePauseMenu()
    {
        if (GetTree().Paused == true)
        {
            GetTree().Paused = false;
            GameplayUI.Visible = true;
            PauseMenu.QueueFree();
            return;
        }

        GetTree().Paused = true;
        GameplayUI.Visible = false;
        PauseMenu = ResourcePaths.GetSceneInstanceFromEnum<PauseMenu>(ResourcePaths.ScenePathsEnum.PLAYER_PAUSE_MENU);
        PauseMenu.SetPlayerReference(_player);
        _layer.AddChild(PauseMenu);
    }

    public void ShowDeathUI()
    {
        GameplayUI.QueueFree();
        DeathUI = ResourcePaths.GetSceneInstanceFromEnum<Control>(ResourcePaths.ScenePathsEnum.PLAYER_DEATH_UI);
        _layer.AddChild(DeathUI);
    }
}
