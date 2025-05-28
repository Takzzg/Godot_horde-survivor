using Godot;

public partial class MainGame : Node2D
{
    public CanvasLayer Layer;

    public override void _Ready()
    {
        GD.Print($"MainGame ready!");
        TextureFilter = TextureFilterEnum.Nearest;

        // world center reference
        Vector2 squareSize = new(64, 64);
        ColorRect worldCenter = new() { Color = Colors.DimGray, Size = squareSize, Position = -squareSize / 2 };
        AddChild(worldCenter);

        // create player
        GameManager.Instance.Player = new PlayerScene();
        AddChild(GameManager.Instance.Player);

        // create basic weapon
        ProjectileWeapon weapon = new(WeaponShooting.EnumStyle.TIMER, WeaponAiming.EnumStyle.RANDOM);
        GameManager.Instance.Player.WeaponsContainer.AddChild(weapon);

        // create EnemiesManager
        GameManager.Instance.EnemiesManager = new EnemiesManager(6);
        AddChild(GameManager.Instance.EnemiesManager);

        // create ExperienceManager
        GameManager.Instance.ExperienceManager = new ExperienceManager();
        AddChild(GameManager.Instance.ExperienceManager);

        // create ui
        Layer = new CanvasLayer();
        AddChild(Layer);
        GameManager.Instance.UI = SceneManager.Instance.GetInstanceFromEnum<MainUI>(SceneManager.EnumPathsDictionary.MAIN_UI);
        Layer.AddChild(GameManager.Instance.UI);

        // spawn enemies
        GameManager.Instance.EnemiesManager.Timer.Start();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsAction("back"))
        {
            SceneManager.Instance.ChangeScene(SceneManager.EnumScenes.MAIN_MENU);
        }
    }
}
