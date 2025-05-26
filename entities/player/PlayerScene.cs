using Godot;
using System;
using System.Linq;

public partial class PlayerScene : CharacterBody2D
{
    [Export]
    public bool Alive = true;
    [Export]
    public int Health = 50;
    [Export]
    public int Speed = 50;

    [Export]
    public Node2D WeaponsContainer;

    [Signal]
    public delegate void PlayerMoveEventHandler();
    [Signal]
    public delegate void PlayerReceiveDamageEventHandler(int amount);
    [Signal]
    public delegate void PlayerDeathEventHandler();

    // components
    public PlayerMovement PlayerMovement;
    public PlayerDraw PlayerDraw;

    public PlayerScene()
    {
        // bind signals
        // PlayerReceiveDamage += OnReceiveDamage;
        PlayerDeath += OnPlayerDeath;

        // create components
        PlayerMovement = new(this);
        AddChild(PlayerMovement);

        PlayerDraw = new();
        AddChild(PlayerDraw);
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
        Velocity = new Vector2(0, 0);

        PlayerMovement.SetPhysicsProcess(false);

        Godot.Collections.Array<Node> weapons = WeaponsContainer.GetChildren();
        foreach (ProjectileWeapon weapon in weapons.Cast<ProjectileWeapon>()) { weapon.WeaponShooting.TimedAttackSetRunning(false); }
    }
}
