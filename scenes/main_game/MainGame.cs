using Godot;

public partial class MainGame : Node2D
{
    public MainGame()
    {
        GD.Print($"MainGame ready!");

        // world center reference
        Vector2 squareSize = new(64, 64);
        ColorRect worldCenter = new() { Color = Colors.DimGray, Size = squareSize, Position = -squareSize / 2 };
        AddChild(worldCenter);

        // create player
        GameManager.Instance.Player = new PlayerScene();
        AddChild(GameManager.Instance.Player);

        // create basic weapon
        ProjectileWeapon weapon = new(BaseWeapon.TrajectoryStyleEnum.RANDOM);
        GameManager.Instance.Player.PlayerWeapons.CreateWeapon(weapon);

        // create EnemiesManager
        GameManager.Instance.EnemiesManager = new EnemiesManager();
        GameManager.Instance.EnemiesManager.SetEnemyStats(6, 10, 25, 1, 1);
        AddChild(GameManager.Instance.EnemiesManager);

        // create ExperienceManager
        GameManager.Instance.ExperienceManager = new ExperienceManager();
        AddChild(GameManager.Instance.ExperienceManager);

        GameManager.Instance.EnemiesManager.StartSpawnerTimer();
    }
}
