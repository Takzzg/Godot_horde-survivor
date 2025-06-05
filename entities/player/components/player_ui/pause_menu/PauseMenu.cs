using Godot;

public partial class PauseMenu : Panel
{
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
    }

    private static void ClosePauseMenu()
    {
        GameManager.Instance.Player.PlayerUI.TogglePauseMenu();
    }

    private void QuitToMenu()
    {
        GetTree().Paused = false;
        SceneManager.Instance.ChangeScene(SceneManager.EnumScenes.MAIN_MENU);
    }
}
