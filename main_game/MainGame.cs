using Godot;

public partial class MainGame : Node2D
{
    public CanvasLayer Layer;

    public override void _Ready()
    {
        GD.Print($"MainGame ready!");
        TextureFilter = TextureFilterEnum.Nearest;

        ColorRect worldCenter = new() { Color = Colors.DimGray, Size = new Vector2(64, 64), Position = new Vector2(-32, -32) };
        AddChild(worldCenter);

        // create player
        PlayerScene player = new();
        AddChild(player);

        // create EnemyManager
        Texture2D enemyTexture = GD.Load<Texture2D>("res://main_game/enemy/enemy_sphere.png");
        EnemyManager enemyManager = new(6, enemyTexture);
        AddChild(enemyManager);

        // create ui
        Layer = new CanvasLayer();
        AddChild(Layer);
        MainUI ui = new();
        Layer.AddChild(ui);

        // setup Game Manager
        GameManager.Instance.Player = player;
        GameManager.Instance.UI = ui;
        GameManager.Instance.EnemyManager = enemyManager;

        // connetc signals
        ui.SetUpSignals();
        player.SetUpSignals();

        GameManager.Instance.EnemyManager.Timer.Start();
    }
}
