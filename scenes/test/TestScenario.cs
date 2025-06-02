using System;
using Godot;

public partial class TestScenario : Node2D
{
    private Vector2 _worldCenterSize = new(64, 64);

    public TestScenario()
    {
        GD.Print($"TestScenario ready!");

        // world center ref
        ColorRect worldCenter = new() { Color = Colors.DimGray, Size = _worldCenterSize, Position = -_worldCenterSize / 2 };
        AddChild(worldCenter);

        // create player
        GameManager.Instance.Player = new PlayerScene();
        AddChild(GameManager.Instance.Player);

        // create EnemiesManager
        GameManager.Instance.EnemiesManager = new EnemiesManager(6);
        AddChild(GameManager.Instance.EnemiesManager);

        // create ExperienceManager
        GameManager.Instance.ExperienceManager = new ExperienceManager();
        AddChild(GameManager.Instance.ExperienceManager);

        // weapon switching
        CreateWeaponTypeOptions();

        // spawn enemies
        Ready += TEST_SpawnEnemies;
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
            BasicEnemy enemy = new(pos, 50, 0, 1, 1);
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

    public void CreateWeaponTypeOptions()
    {
        Theme theme = new() { DefaultFontSize = 6 };
        Vector2 titleSize = new(24, 16);
        int separation = 25;

        (string title, Func<BaseWeapon> createWeapon)[] types = [
            new("Projectile", () => new ProjectileWeapon(BaseWeapon.TrajectoryStyleEnum.FACING, true)),
            new("Stationary", () => new StationaryWeapon(BaseWeapon.TrajectoryStyleEnum.NONE, true)),
            new("Relative", () => new RelativeWeapon(BaseWeapon.TrajectoryStyleEnum.NONE, true)),
        ];

        static void OnCreateWeapon(PlayerScene player, Func<BaseWeapon> createWeapon)
        {
            if (player.PlayerWeapons.WeaponsList.Count > 0) player.PlayerWeapons.DestroyWeapon(player.PlayerWeapons.WeaponsList[0]);
            player.PlayerWeapons.CreateWeapon(createWeapon());
        }

        // parent node
        Vector2 parentPosition = new(-(titleSize.X + (separation * (types.Length + 1)) + (_worldCenterSize.X / 2)), -separation);
        Node2D parent = new() { Position = parentPosition };
        AddChild(parent);

        // title
        WorldTextCentered label = new("Weapon", theme) { Position = new Vector2(titleSize.X / 2, 0) };
        parent.AddChild(label);

        // options
        Vector2 nextPos = new(titleSize.X + separation / 2, 0);
        foreach ((string title, Func<BaseWeapon> createWeapon) in types)
        {
            PlayerTrigger weapon = new(title, (player) => OnCreateWeapon(player, createWeapon)) { Position = nextPos };
            parent.AddChild(weapon);
            nextPos.X += separation;
        }
    }
}