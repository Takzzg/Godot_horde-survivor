using System;
using System.Linq;
using Godot;

public partial class TestScenario : Node2D
{
    private static Vector2 _worldCenterSize = new(64, 64);
    private static readonly int _enemyRadius = 6;
    private int _gap = 25;

    public TestScenario()
    {
        GD.Print($"TestScenario ready!");

        // world center ref
        ColorRect worldCenter = new()
        {
            Color = new Color("#161616"),
            Size = _worldCenterSize,
            Position = -_worldCenterSize / 2
        };
        AddChild(worldCenter);

        // create player
        GameManager.Instance.Player = new PlayerScene();
        AddChild(GameManager.Instance.Player);

        // create EnemiesManager
        GameManager.Instance.EnemiesManager = new EnemiesManager();
        AddChild(GameManager.Instance.EnemiesManager);

        // create ExperienceManager
        GameManager.Instance.ExperienceManager = new ExperienceManager();
        AddChild(GameManager.Instance.ExperienceManager);

        // ------------- triggers -------------
        // level up
        Vector2 lvlUpPos = new(-_gap, -(_worldCenterSize.Y / 2 + _gap));
        CreateTriggerLevelUp(lvlUpPos);

        // spawn enemies
        Vector2 spawnPos = new(_gap, -(_worldCenterSize.Y / 2 + _gap));
        CreateTriggerSpawnEnemies(spawnPos);

        // chage weapon
        Vector2 weaponTypesPos = new(-((_worldCenterSize.X / 2) + _gap), -_gap);
        CreateOptionsWeaponType(weaponTypesPos);

        Vector2 weaponAimingPos = new(-((_worldCenterSize.X / 2) + _gap), _gap);
        CreateOptionsWeaponAiming(weaponAimingPos);
    }

    private void CreateTriggerLevelUp(Vector2 pos)
    {
        PlayerTrigger levelUp = new("Level up", (player) => player.PlayerExperience.TriggerLevelUp()) { Position = pos };
        AddChild(levelUp);
    }

    private void CreateTriggerSpawnEnemies(Vector2 pos)
    {
        PlayerTrigger spawnEnemies = new("Spawn Enemies", (_) => CallDeferred(MethodName.TEST_SpawnEnemies)) { Position = pos };
        AddChild(spawnEnemies);
    }

    private static void TEST_SpawnEnemies()
    {
        GameManager.Instance.EnemiesManager.DestroyAllEnemies();

        int enemyRows = 4;
        int enemiesPerRow = 8;
        Vector2 pos = new(_worldCenterSize.X / 2 + 16, -(enemyRows * _enemyRadius));

        for (int i = 0; i < enemyRows; i++)
        {
            for (int j = 0; j < enemiesPerRow; j++)
            {
                BasicEnemy enemy = new() { Speed = 0, Position = pos };
                GameManager.Instance.EnemiesManager.Spawner.SpawnEnemy(enemy);
                pos.X += _enemyRadius * 2;
            }
            pos.X = _worldCenterSize.X / 2 + 16;
            pos.Y += _enemyRadius * 2;
        }
    }

    private void CreateOptionsWeaponType(Vector2 pos)
    {
        (string title, Func<BaseWeapon> createWeapon)[] types = [
            new("Projectile", () => new ProjectileWeapon(Utils.RarityEnum.COMMON ,BaseWeapon.TrajectoryStyleEnum.RANDOM, true)),
            new("Stationary", () => new StationaryWeapon(Utils.RarityEnum.COMMON ,BaseWeapon.TrajectoryStyleEnum.RANDOM, true)),
            new("Relative", () => new RelativeWeapon(Utils.RarityEnum.COMMON ,BaseWeapon.TrajectoryStyleEnum.RANDOM, true)),
        ];

        // parent node
        Vector2 parentPosition = pos with { X = pos.X + -(_gap * (types.Length + 1)) };
        Node2D parent = new() { Position = parentPosition };
        AddChild(parent);

        // title
        Label label = new() { Text = "Weapon", Scale = Vector2.One / 2.5f };
        parent.AddChild(label);

        // options
        static void OnCreateWeapon(PlayerScene player, Func<BaseWeapon> createWeapon)
        {
            if (player.PlayerWeapons.WeaponsList.Count > 0) player.PlayerWeapons.DestroyWeapon(player.PlayerWeapons.WeaponsList[0]);
            player.PlayerWeapons.CreateWeapon(createWeapon());
        }

        Vector2 nextPos = new(_gap * 1.5f, 0);
        foreach ((string title, Func<BaseWeapon> createWeapon) in types)
        {
            PlayerTrigger weapon = new(title, (player) => OnCreateWeapon(player, createWeapon)) { Position = nextPos };
            parent.AddChild(weapon);
            nextPos.X += _gap;
        }
    }

    private void CreateOptionsWeaponAiming(Vector2 pos)
    {
        (string title, Action<BaseWeapon> changeAiming)[] types = [
            new("None", (weapon) => weapon.SetTrajectoryStyle(BaseWeapon.TrajectoryStyleEnum.NONE)),
            new("Random", (weapon) => weapon.SetTrajectoryStyle(BaseWeapon.TrajectoryStyleEnum.RANDOM)),
            new("Facing", (weapon) => weapon.SetTrajectoryStyle(BaseWeapon.TrajectoryStyleEnum.FACING)),
        ];

        // parent node
        Vector2 parentPosition = pos with { X = pos.X + -(_gap * (types.Length + 1)) };
        Node2D parent = new() { Position = parentPosition };
        AddChild(parent);

        // title
        Label label = new() { Text = "Aiming", Scale = Vector2.One / 2.5f };
        parent.AddChild(label);

        // options
        static void OnChangeWeaponAiming(PlayerScene player, Action<BaseWeapon> changeAiming)
        {
            BaseWeapon weapon = player.PlayerWeapons.WeaponsList.First();
            changeAiming(weapon);
        }

        Vector2 nextPos = new(_gap * 1.5f, 0);
        foreach ((string title, Action<BaseWeapon> changeAiming) in types)
        {
            PlayerTrigger weapon = new(title, (player) => OnChangeWeaponAiming(player, changeAiming)) { Position = nextPos };
            parent.AddChild(weapon);
            nextPos.X += _gap;
        }
    }
}