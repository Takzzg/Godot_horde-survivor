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
        GameManager.Instance.Player = SceneManager.Instance.GetInstanceFromEnum<PlayerScene>(SceneManager.EnumPathsDictionary.PLAYER_SCENE);
        AddChild(GameManager.Instance.Player);
        // create basic weapon
        GameManager.Instance.Player.WeaponsContainer.AddChild(new ProjectileWeapon(WeaponShooting.EnumShootingStyle.TIMER, WeaponAiming.EnumAimingStyle.FACING));

        // create EnemyManager
        GameManager.Instance.EnemyManager = new EnemyManager(6);
        AddChild(GameManager.Instance.EnemyManager);

        // create ui
        Layer = new CanvasLayer();
        AddChild(Layer);
        GameManager.Instance.UI = SceneManager.Instance.GetInstanceFromEnum<MainUI>(SceneManager.EnumPathsDictionary.MAIN_UI);
        Layer.AddChild(GameManager.Instance.UI);

        GameManager.Instance.EnemyManager.Timer.Start();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsAction("back"))
        {
            SceneManager.Instance.ChangeScene(SceneManager.EnumScenes.MAIN_MENU);
        }
    }
}
