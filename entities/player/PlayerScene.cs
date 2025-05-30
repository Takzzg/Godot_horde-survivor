using Godot;

public partial class PlayerScene : CharacterBody2D
{
    // components
    public PlayerStats PlayerStats;
    public PlayerHealth PlayerHealth;
    public PlayerMovement PlayerMovement;
    public PlayerDraw PlayerDraw;
    public PlayerExperience PlayerExperience;
    public PlayerUI PlayerUI;
    public Node2D WeaponsContainer;

    private PlayerDebug _debug;

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
        AddChild(PlayerStats);

        PlayerDraw = new PlayerDraw(this);
        AddChild(PlayerDraw);

        PlayerMovement = new PlayerMovement(this);
        AddChild(PlayerMovement);

        PlayerHealth = new PlayerHealth(this);
        AddChild(PlayerHealth);

        PlayerExperience = new PlayerExperience(this);
        AddChild(PlayerExperience);

        PlayerUI = new PlayerUI(this);
        AddChild(PlayerUI);

        _debug = new PlayerDebug(this);
        AddChild(_debug);
    }
}
