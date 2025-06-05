using Godot;

public partial class PauseMenu : Panel
{
    private PlayerScene _player;

    [Export]
    private Button _quitBtn;
    [Export]
    private Button _continueBtn;

    [Export]
    private BoxContainer _weaponsCont;

    public override void _Ready()
    {
        _quitBtn.Pressed += QuitToMenu;
        _continueBtn.Pressed += ClosePauseMenu;

        RenderWeapons();
    }

    public void SetPlayerReference(PlayerScene player)
    {
        _player = player;
    }

    private void ClosePauseMenu()
    {
        _player.PlayerUI.TogglePauseMenu();
    }

    private void QuitToMenu()
    {
        GetTree().Paused = false;
        SceneManager.Instance.ChangeScene(SceneManager.EnumScenes.MAIN_MENU);
    }

    public void RenderWeapons()
    {
        _player.PlayerWeapons.WeaponsList.ForEach(weapon =>
        {
            _weaponsCont.AddChild(weapon.GetWeaponPanel());
        });
    }
}
