using Godot;

public partial class GameManager : Node
{
    public static GameManager Instance { get; private set; }

    public RandomNumberGenerator RNG;

    public PlayerScene Player;
    public MainUI UI;

    public override void _Ready()
    {
        GD.Print($"GameManager singleton ready!");
        Instance = this;

        RNG = new RandomNumberGenerator();
    }
}