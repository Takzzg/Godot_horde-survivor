using System.Collections.Generic;
using Godot;

public partial class PlayerUI : BasePlayerComponent
{
    private CanvasLayer _layer;

    public GameplayUI GameplayUI;
    public PauseMenu PauseMenu;
    public Control DeathUI;
    public LevelUpUI LevelUpUI;

    public PlayerUI(PlayerScene player) : base(player)
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
            else { _player.PlayerUI.ShowPauseMenu(); }
            GetViewport().SetInputAsHandled();
        }
    }

    public void ShowGameplayUI()
    {
        GameplayUI = ResourcePaths.GetSceneInstanceFromEnum<GameplayUI>(ResourcePaths.ScenePathsEnum.PLAYER_GAMEPLAY_UI);
        GameplayUI.UpdateValues(_player);
        _layer.AddChild(GameplayUI);
    }

    private void ShowPauseMenu()
    {
        GetTree().Paused = true;
        GameplayUI.Visible = false;

        PauseMenu = ResourcePaths.GetSceneInstanceFromEnum<PauseMenu>(ResourcePaths.ScenePathsEnum.PLAYER_PAUSE_MENU);
        PauseMenu.SetPlayerReference(_player);
        _layer.AddChild(PauseMenu);

        PauseMenu.TreeExiting += () =>
        {
            GetTree().Paused = false;
            GameplayUI.Visible = true;
        };
    }

    public void ShowDeathUI()
    {
        GameplayUI.QueueFree();
        DeathUI = ResourcePaths.GetSceneInstanceFromEnum<Control>(ResourcePaths.ScenePathsEnum.PLAYER_DEATH_UI);
        _layer.AddChild(DeathUI);
    }

    public void ShowLevelUpUI()
    {
        // pause game
        GetTree().Paused = true;

        // create ui
        LevelUpUI = ResourcePaths.GetSceneInstanceFromEnum<LevelUpUI>(ResourcePaths.ScenePathsEnum.PLAYER_LEVEL_UP);
        LevelUpUI.SetPlayerReference(_player);
        _layer.AddChild(LevelUpUI);

        // get options
        if (_player.PlayerWeapons.WeaponsList.Count == 0 || _player.PlayerExperience.PlayerLevel % 5 == 0)
        {
            List<BaseWeapon> weapons = [
                new ProjectileWeapon(Utils.RarityEnum.COMMON, BaseWeapon.TrajectoryStyleEnum.RANDOM),
                new StationaryWeapon(Utils.RarityEnum.COMMON, BaseWeapon.TrajectoryStyleEnum.RANDOM),
                new RelativeWeapon(Utils.RarityEnum.COMMON, BaseWeapon.TrajectoryStyleEnum.RANDOM),
            ];
            LevelUpUI.UpdateOptions(weapons);
        }
        else
        {
            List<BaseModifier> mods = _player.PlayerModifierGenerator.GetModifierOptions();
            LevelUpUI.UpdateOptions(mods);
        }

        // show current weapons
        LevelUpUI.UpdateWeapons(_player.PlayerWeapons.WeaponsList);

        GameplayUI.Visible = false;
        LevelUpUI.TreeExiting += () =>
        {
            GetTree().Paused = false;
            GameplayUI.Visible = true;
        };
    }
}
