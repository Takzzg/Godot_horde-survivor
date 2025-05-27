using Godot;

public partial class PlayerScene : CharacterBody2D
{
    // components
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
        PlayerHealth = new PlayerHealth(this);
        AddChild(PlayerHealth);

        PlayerMovement = new PlayerMovement(this);
        AddChild(PlayerMovement);

        PlayerDraw = new PlayerDraw(this);
        AddChild(PlayerDraw);

        PlayerExperience = new PlayerExperience(this);
        AddChild(PlayerExperience);
    }
}
