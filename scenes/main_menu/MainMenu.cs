using Godot;

public partial class MainMenu : Control
{
    [Export]
    private Button _start;
    [Export]
    private Button _options;
    [Export]
    private Button _quit;
    [Export]
    private Label _versionLabel;

    [ExportCategory("Debug")]
    [Export]
    private BoxContainer _debugContainer;
    [Export]
    private Button _testScenario;
    [Export]
    private Button _testLoading;

    public override void _Ready()
    {
        GD.Print($"MainMenu ready!");
        _start.Pressed += StartGame;
        _options.Pressed += SceneManager.Instance.OpenOptionsMenu;
        _quit.Pressed += QuitGame;
        _versionLabel.Text = GameManager.GAME_VERSION;

        // debug
        _debugContainer.Visible = DebugManager.Instance.DebugEnabled;
        DebugManager.Instance.DebugStateToggled += SetDebugContainerVisible;
        TreeExiting += () => { DebugManager.Instance.DebugStateToggled -= SetDebugContainerVisible; };

        _testScenario.Pressed += () => SceneManager.Instance.ChangeScene(SceneManager.EnumScenes.TEST_SCENARIO);
        _testLoading.Pressed += () => SceneManager.Instance.ChangeScene(SceneManager.EnumScenes.MAIN_MENU);
    }

    private void SetDebugContainerVisible(bool state)
    {
        GD.Print($"_debugContainer: {_debugContainer}");
        _debugContainer.Visible = state;
    }

    private void StartGame() { SceneManager.Instance.ChangeScene(SceneManager.EnumScenes.MAIN_GAME); }
    private void QuitGame() { GetTree().Quit(); }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("enter"))
        {
            SceneManager.Instance.ChangeScene(DebugManager.Instance.DebugEnabled ? SceneManager.EnumScenes.TEST_SCENARIO : SceneManager.EnumScenes.MAIN_GAME);
        }
        if (@event.IsActionPressed("back")) { QuitGame(); }
    }

}
