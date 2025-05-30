using Godot;

public partial class PlayerScene : CharacterBody2D
{
    // components
    public PlayerUI PlayerUI;
    public PlayerStats PlayerStats;
    public PlayerHealth PlayerHealth;
    public PlayerMovement PlayerMovement;
    public PlayerDraw PlayerDraw;
    public PlayerExperience PlayerExperience;
    public Node2D WeaponsContainer;

    public PlayerScene()
    {
        // create weapons cont
        WeaponsContainer = new();
        AddChild(WeaponsContainer);

        // create camera
        Camera2D camera = new() { Zoom = new Vector2(2, 2), TextureFilter = TextureFilterEnum.Nearest };
        AddChild(camera);

        // create components        
        PlayerStats = new PlayerStats(this);
        PlayerDraw = new PlayerDraw(this);
        PlayerMovement = new PlayerMovement(this);
        PlayerHealth = new PlayerHealth(this);
        PlayerExperience = new PlayerExperience(this);
        PlayerUI = new PlayerUI(this);
    }
}
