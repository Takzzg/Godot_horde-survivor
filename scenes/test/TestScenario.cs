using Godot;

public partial class TestScenario : Node2D
{
    public CanvasLayer Layer;

    public override void _Ready()
    {
        GD.Print($"TestScenario ready!");

        ColorRect worldCenter = new() { Color = Colors.DimGray, Size = new Vector2(64, 64), Position = new Vector2(-32, -32) };
        AddChild(worldCenter);

        // create player
        GameManager.Instance.Player = new PlayerScene();
        AddChild(GameManager.Instance.Player);

        // create basic weapon
        ProjectileWeapon weapon = new(WeaponShooting.EnumShootingStyle.MANUAL, WeaponAiming.EnumAimingStyle.FIXED_R);
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

        TEST_SpawnEnemies();
    }

    public static void TEST_SpawnEnemies()
    {
        Vector2[] positions = [
            new Vector2(100, 0),
            new Vector2(100, 50),
            new Vector2(110, 50),
            new Vector2(120, 50),
            new Vector2(100, 100),
            new Vector2(110, 100),
            new Vector2(120, 100),
            new Vector2(130, 100),
            new Vector2(140, 100),
        ];

        foreach (Vector2 pos in positions)
        {
            BasicEnemy enemy = new(pos, 50, 0, 0, 1);
            GameManager.Instance.EnemiesManager.SpawnEnemy(enemy);
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsAction("back"))
        {
            SceneManager.Instance.ChangeScene(SceneManager.EnumScenes.MAIN_MENU);
        }
    }
}