using Godot;
using System;

public partial class MainGame : Node2D
{
    [Export]
    private PlayerScene _player;
    [Export]
    private MainUI _mainUI;
    [Export]
    private EnemySpawner _spawner;

    public override void _Ready()
    {
        GD.Print($"MainGame ready!");

        GameManager.Instance.Player = _player;
        GameManager.Instance.UI = _mainUI;

        _player.SetUpSignals();
        _mainUI.SetUpSignals();
        _spawner.SetUpSignals();
    }
    public override void _Process(double delta)
    {
        if (Input.IsActionPressed("open_menu"))
        {
            GD.Print($"open_menu key pressed");
            SceneManager.Instance.ChangeScene(SceneManager.SceneEnum.MAIN_MENU);
        }
    }

}
