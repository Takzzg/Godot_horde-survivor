using Godot;

public partial class MainMenu : Control
{

    [Export]
    private Button _start;
    [Export]
    private Button _quit;
    [Export]
    private Button _testScenario;
    [Export]
    private Button _testLoading;
    [Export]
    private Label _versionLabel;

    public override void _Ready()
    {
        GD.Print($"MainMenu ready!");
        _start.Pressed += StartGame;
        _quit.Pressed += QuitGame;

        _versionLabel.Text = GameManager.GAME_VERSION;
        _testScenario.Pressed += () => SceneManager.Instance.ChangeScene(SceneManager.EnumScenes.TEST_SCENARIO);
        _testLoading.Pressed += () => SceneManager.Instance.ChangeScene(SceneManager.EnumScenes.MAIN_MENU);
    }

    private void StartGame() { SceneManager.Instance.ChangeScene(SceneManager.EnumScenes.MAIN_GAME); }
    private void QuitGame() { GetTree().Quit(); }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsAction("enter")) { StartGame(); }
        if (@event.IsAction("back")) { QuitGame(); }
    }
}
