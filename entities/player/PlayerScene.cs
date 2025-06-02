using Godot;

public partial class PlayerScene : CharacterBody2D
{
    // components
    public PlayerStats PlayerStats;
    public PlayerHealth PlayerHealth;
    public PlayerMovement PlayerMovement;
    public PlayerDraw PlayerDraw;
    public PlayerExperience PlayerExperience;
    public PlayerWeapons PlayerWeapons;
    public PlayerUI PlayerUI;

    public PlayerScene()
    {
        // create camera
        Camera2D camera = new() { Zoom = new Vector2(2, 2) };
        AddChild(camera);

        // create components        
        PlayerStats = new PlayerStats(this);
        PlayerDraw = new PlayerDraw(this);
        PlayerMovement = new PlayerMovement(this);
        PlayerHealth = new PlayerHealth(this);
        PlayerExperience = new PlayerExperience(this);
        PlayerWeapons = new PlayerWeapons(this);
        PlayerUI = new PlayerUI(this);
    }
}
