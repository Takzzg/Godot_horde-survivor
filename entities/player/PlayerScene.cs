using Godot;

public partial class PlayerScene : CharacterBody2D
{
    // components
    public PlayerHealth PlayerHealth;
    public PlayerMovement PlayerMovement;
    public PlayerDraw PlayerDraw;
    public PlayerExperience PlayerExperience;
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
        PlayerDraw = new PlayerDraw(this);
        AddChild(PlayerDraw);

        PlayerMovement = new PlayerMovement(this);
        AddChild(PlayerMovement);

        PlayerHealth = new PlayerHealth(this);
        AddChild(PlayerHealth);

        PlayerExperience = new PlayerExperience(this);
        AddChild(PlayerExperience);

        _debug = new PlayerDebug(this);
        AddChild(_debug);
    }
}
