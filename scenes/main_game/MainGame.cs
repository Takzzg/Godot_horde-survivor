using Godot;

public partial class MainGame : Node2D
{
    public MainGame()
    {
        GD.Print($"MainGame ready!");

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
