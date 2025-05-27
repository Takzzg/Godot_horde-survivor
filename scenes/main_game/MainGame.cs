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
        GameManager.Instance.Player = new PlayerScene();
        AddChild(GameManager.Instance.Player);

        // create basic weapon
        ProjectileWeapon weapon = new(WeaponShooting.EnumShootingStyle.TIMER, WeaponAiming.EnumAimingStyle.RANDOM);
        GameManager.Instance.Player.WeaponsContainer.AddChild(weapon);

        // create EnemiesManager
        GameManager.Instance.EnemiesManager = new EnemiesManager(6);
        AddChild(GameManager.Instance.EnemiesManager);

        // create ui
        Layer = new CanvasLayer();
        AddChild(Layer);
        GameManager.Instance.UI = SceneManager.Instance.GetInstanceFromEnum<MainUI>(SceneManager.EnumPathsDictionary.MAIN_UI);
        Layer.AddChild(GameManager.Instance.UI);

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
