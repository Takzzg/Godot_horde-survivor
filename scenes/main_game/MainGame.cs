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

        // create EnemiesManager
        GameManager.Instance.EnemiesManager = new EnemiesManager();
        AddChild(GameManager.Instance.EnemiesManager);

        // create ExperienceManager
        GameManager.Instance.ExperienceManager = new ExperienceManager();
        AddChild(GameManager.Instance.ExperienceManager);

        Ready += () =>
        {
            GameManager.Instance.EnemiesManager.Spawner.StartTimer();
            GameManager.Instance.Player.PlayerExperience.TriggerLevelUp();
        };
    }
}
