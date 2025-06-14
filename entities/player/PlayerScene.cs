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
    public PlayerModifierGenerator PlayerModifierGenerator;
    public PlayerUI PlayerUI;

    public PlayerScene()
    {
        // create camera
        Camera2D camera = new() { Zoom = new Vector2(4, 4) };
        AddChild(camera);

        // create components        
        PlayerStats = new PlayerStats(this);
        PlayerDraw = new PlayerDraw(this);
        PlayerMovement = new PlayerMovement(this);
        PlayerHealth = new PlayerHealth(this);
        PlayerExperience = new PlayerExperience(this);
        PlayerWeapons = new PlayerWeapons(this);
        PlayerModifierGenerator = new PlayerModifierGenerator(this);
        PlayerUI = new PlayerUI(this);

        // debug cat
        Ready += DebugRenderNode;
    }

    // -------------------------------------------- DEBUG --------------------------------------------
    private DebugCategory _debug;

    public DebugCategory DebugCreateCategory()
    {
        DebugCategory category = new("Player");
        category.CreateLabelField("player_pos", "Pos", Position.ToString("0.0"));

        PlayerHealth.DebugCreateSubCategory(category);
        PlayerExperience.DebugCreateSubCategory(category);
        PlayerMovement.DebugCreateSubCategory(category);
        PlayerWeapons.DebugCreateSubCategory(category);
        PlayerStats.DebugCreateSubCategory(category);

        return category;
    }

    public void DebugRenderNode()
    {
        DebugSetState(DebugManager.Instance.DebugEnabled);
        DebugManager.Instance.DebugStateToggled += DebugSetState;

        TreeExiting += () =>
        {
            if (DebugManager.Instance.DebugEnabled) { DebugSetState(false); }
            DebugManager.Instance.DebugStateToggled -= DebugSetState;
        };
    }

    private void DebugSetState(bool state)
    {
        if (state == false)
        {
            _debug?.QueueFree();
            return;
        }

        _debug = DebugCreateCategory();
        DebugManager.Instance.RenderNode(_debug);
    }

    public void DebugTryUpdateField(string id, string value)
    {
        if (DebugManager.Instance.DebugEnabled == false) return;
        if (_debug == null) return;
        _debug.UpdateLabelField(id, value);
    }
}
