using Godot;
using System;

public partial class MainGame : Node2D
{
    [Export]
    public PlayerScene Player;
    [Export]
    public MainUI MainUI;
    [Export]
    public EnemySpawner EnemySpawner;

    public BulletsManager BulletsManager;

    public override void _Ready()
    {
        GD.Print($"MainGame ready!");

        // setup main game
        GameManager.Instance.MainGame = this;
        TextureFilter = TextureFilterEnum.Nearest;

        // create bullet manager
        BulletsManager = new();
        AddChild(BulletsManager);

        // setup node signals
        Player.SetUpSignals();
        MainUI.SetUpSignals();
        EnemySpawner.SetUpSignals();

        // create weapon
        BasicWeapon weapon1 = new() { };
        Player.WeaponsContainer.AddChild(weapon1);
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
