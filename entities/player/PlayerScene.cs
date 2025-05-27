using Godot;

public partial class PlayerScene : CharacterBody2D
{
    // components
    public PlayerHealth PlayerHealth;
    public PlayerMovement PlayerMovement;
    public PlayerDraw PlayerDraw;
    public Node2D WeaponsContainer;

    public PlayerScene()
    {
        // create weapons cont
        WeaponsContainer = new();
        AddChild(WeaponsContainer);

        // create camera
        Camera2D camera = new() { Zoom = new Vector2(3, 3), TextureFilter = TextureFilterEnum.Nearest };
        AddChild(camera);

        // create components
        PlayerHealth = new(this);
        AddChild(PlayerHealth);

        PlayerMovement = new(this);
        AddChild(PlayerMovement);

        PlayerDraw = new();
        AddChild(PlayerDraw);
    }
}
