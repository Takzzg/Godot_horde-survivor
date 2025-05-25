using Godot;

public partial class MainGame : Node2D
{
    [Export]
    public PlayerScene Player;
    [Export]
    public MainUI MainUI;
    [Export]
    public EnemySpawner EnemySpawner;

    public EnemyManager EnemyManager;

    public override void _Ready()
    {
        GD.Print($"MainGame ready!");

        // setup main game
        GameManager.Instance.MainGame = this;
        TextureFilter = TextureFilterEnum.Nearest;

        // setup EnemyManager
        Texture2D enemyTexture = GD.Load<Texture2D>("res://main_game/enemy/enemy_sphere.png");
        EnemyManager = new(6, enemyTexture);
        AddChild(EnemyManager);

        // create weapon
        Player.WeaponsContainer.AddChild(new BasicWeapon());

        // setup node signals
        Player.SetUpSignals();
        MainUI.SetUpSignals();
        // EnemySpawner.SetUpSignals();

        EnemyManager.Timer.Start();
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
