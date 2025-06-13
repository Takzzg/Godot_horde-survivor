using System;
using Godot;

public partial class TestScenario : Node2D
{
    private static Vector2 _worldCenterSize = new(64, 64);
    private static readonly int _enemyRadius = 6;

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
        GameManager.Instance.EnemiesManager = new EnemiesManager();
        GameManager.Instance.EnemiesManager.SetEnemyStats(_enemyRadius, 10, 0, 1, 1);
        AddChild(GameManager.Instance.EnemiesManager);

        // create ExperienceManager
        GameManager.Instance.ExperienceManager = new ExperienceManager();
        AddChild(GameManager.Instance.ExperienceManager);

        // weapon switching
        CreateWeaponTypeOptions();

        // level up
        CreateLevelUpTrigger();

        // spawn enemies
        Ready += TEST_SpawnEnemies;
    }

    public static void TEST_SpawnEnemies()
    {
        int enemyRows = 4;
        int enemiesPerRow = 8;
        Vector2 pos = new(_worldCenterSize.X / 2 + 16, -(enemyRows * _enemyRadius));

        for (int i = 0; i < enemyRows; i++)
        {
            for (int j = 0; j < enemiesPerRow; j++)
            {
                BasicEnemy enemy = EnemiesManager.GetNewEnemyEntity(pos);
                GameManager.Instance.EnemiesManager.SpawnEnemy(enemy);
                pos.X += _enemyRadius * 2;
            }
            pos.X = _worldCenterSize.X / 2 + 16;
            pos.Y += _enemyRadius * 2;
        }
    }

    public void CreateWeaponTypeOptions()
    {
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
        Vector2 parentPosition = new(-(separation * (types.Length + 1)) / 2, -(_worldCenterSize.Y / 2 + 20));
        Node2D parent = new() { Position = parentPosition };
        AddChild(parent);

        // title
        Label label = new() { Text = "Weapon", Scale = Vector2.One / 2.5f };
        parent.AddChild(label);

        // options
        Vector2 nextPos = new(separation * 1.5f, 0);
        foreach ((string title, Func<BaseWeapon> createWeapon) in types)
        {
            PlayerTrigger weapon = new(title, (player) => OnCreateWeapon(player, createWeapon)) { Position = nextPos };
            parent.AddChild(weapon);
            nextPos.X += separation;
        }
    }

    private void CreateLevelUpTrigger()
    {
        PlayerTrigger levelUp = new("Level up", (player) => player.PlayerExperience.TriggerLevelUp())
        {
            Position = new Vector2(-((_worldCenterSize.X / 2) + 20), 0)
        };
        AddChild(levelUp);
    }
}