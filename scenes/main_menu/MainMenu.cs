using Godot;

public partial class MainMenu : DebuggerNode
{
    [Export]
    private Button _start;
    [Export]
    private Button _options;
    [Export]
    private Button _quit;
    [Export]
    private Label _versionLabel;

    public override void _Ready()
    {
        GD.Print($"MainMenu ready!");
        _start.Pressed += StartGame;
        _options.Pressed += SceneManager.Instance.OpenOptionsMenu;
        _quit.Pressed += QuitGame;
        _versionLabel.Text = GameManager.GAME_VERSION;
    }

    private void StartGame() { SceneManager.Instance.ChangeScene(SceneManager.EnumScenes.MAIN_GAME); }
    private void TestScenario() { SceneManager.Instance.ChangeScene(SceneManager.EnumScenes.TEST_SCENARIO); }
    private void TestLoad() { SceneManager.Instance.ChangeScene(SceneManager.EnumScenes.MAIN_MENU); }
    private void QuitGame() { GetTree().Quit(); }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("enter"))
        {
            if (DebugManager.Instance.DebugEnabled) TestScenario();
            else StartGame();
        }
        if (@event.IsActionPressed("back")) { QuitGame(); }
    }

    public override DebugCategory DebugCreateCategory()
    {
        DebugCategory category = new("Main Menu");

        category.CreateButtonField("test_scenario", "Test Scenario", "Test", TestScenario);
        category.CreateButtonField("test_load", "Test Loading", "Test", TestLoad);

        return category;
    }
}
