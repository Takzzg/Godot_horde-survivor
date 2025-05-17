using Godot;
using System;

public partial class MainMenu : Control
{

    [Export]
    private Button _start;

    [Export]
    private Button _quit;

    public override void _Ready()
    {
        GD.Print($"MainMenu ready!");
        _start.Pressed += StartGame;
        _quit.Pressed += QuitGame;
    }

    private void StartGame()
    {
        SceneManager.Instance.ChangeScene(SceneManager.SceneEnum.MAIN_GAME);
    }

    private void QuitGame()
    {
        GetTree().Quit();
    }
}
