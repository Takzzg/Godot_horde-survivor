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
    public Sprite2D Sprite;

    [Export]
    public Node2D WeaponsContainer;

    [Signal]
    public delegate void PlayerMoveEventHandler();
    [Signal]
    public delegate void PlayerReceiveDamageEventHandler(int amount);
    [Signal]
    public delegate void PlayerDeathEventHandler();

    public override void _Ready()
    {
        // PlayerReceiveDamage += OnReceiveDamage;
        PlayerDeath += OnPlayerDeath;

        // create basic weapon
        WeaponsContainer.AddChild(new BasicWeapon());
    }

    public override void _PhysicsProcess(double delta)
    {
        MovePlayer();
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
        Sprite.Rotate((float)(Math.PI / 2));

        Godot.Collections.Array<Node> weapons = WeaponsContainer.GetChildren();
        foreach (BasicWeapon weapon in weapons.Cast<BasicWeapon>()) { weapon.Timer.Stop(); }
    }

    private void MovePlayer()
    {
        if (!Alive) return;

        Vector2 inputDirection = Input.GetVector("left", "right", "up", "down");
        if (inputDirection.Equals(Vector2.Zero)) return;

        Velocity = inputDirection * Speed;
        EmitSignal(SignalName.PlayerMove);
        MoveAndSlide();
    }
}
