using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class PlayerScene : CharacterBody2D
{
    public bool Alive = true;
    public int Health = 50;
    public int Speed = 50;
    public int PlayerSize = 5;

    [Signal]
    public delegate void PlayerMoveEventHandler();
    [Signal]
    public delegate void PlayerReceiveDamageEventHandler(int amount);
    [Signal]
    public delegate void PlayerDeathEventHandler();

    // components
    public PlayerMovement PlayerMovement;
    public PlayerDraw PlayerDraw;
    public Node2D WeaponsContainer;
    public CollisionShape2D HurtboxShape;

    public PlayerScene()
    {
        CollisionLayer = 1; // 1 = player layer
        CollisionMask = 3; // 1 = player layer, 2 = enemy layer

        // create hurtbox
        HurtboxShape = new CollisionShape2D() { Shape = new CircleShape2D() { Radius = PlayerSize } };
        AddChild(HurtboxShape);

        // create weapons cont
        WeaponsContainer = new();
        AddChild(WeaponsContainer);

        // create camera
        Camera2D camera = new() { Zoom = new Vector2(3, 3), TextureFilter = TextureFilterEnum.Nearest };
        AddChild(camera);

        // create components
        PlayerMovement = new(this);
        AddChild(PlayerMovement);

        PlayerDraw = new();
        AddChild(PlayerDraw);

        // bind signals
        PlayerReceiveDamage += OnReceiveDamage;
        PlayerDeath += OnPlayerDeath;
    }

    private void OnReceiveDamage(int amount)
    {
        if (!Alive) return;

        // GD.Print($"player received {amount} damage");
        Health -= amount;
        if (Health < 0) Health = 0;

        if (Health > 0) return;
        EmitSignal(SignalName.PlayerDeath);
    }

    private void OnPlayerDeath()
    {
        GD.Print($"PlayerScene OnPlayerDeath()");

        Alive = false;
        PlayerMovement.SetPhysicsProcess(false);

        Godot.Collections.Array<Node> weapons = WeaponsContainer.GetChildren();
        foreach (ProjectileWeapon weapon in weapons.Cast<ProjectileWeapon>()) { weapon.WeaponShooting.TimedAttackSetRunning(false); }
    }

    public void OnCollision(Dictionary collision)
    {
        GD.Print($"player OnBodyEntered");
        GD.Print($"collision: {collision}");
    }
}
