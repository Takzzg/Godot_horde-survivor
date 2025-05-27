using Godot;

public partial class GameManager : Node
{
    public static GameManager Instance { get; private set; }

    public RandomNumberGenerator RNG;
    public const string GAME_VERSION = "v1.0.0";
    public const int RENDER_DISTANCE = 200;

    public PlayerScene Player;
    public MainUI UI;
    public EnemiesManager EnemiesManager;
    public ExperienceManager ExperienceManager;

    public override void _Ready()
    {
        GD.Print($"GameManager singleton ready!");
        Instance = this;

        RNG = new RandomNumberGenerator();
    }
}