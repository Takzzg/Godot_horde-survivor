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
    private Sprite2D _sprite;
    [Export]
    private Node2D _weapons;

    [Signal]
    public delegate void PlayerReceiveDamageEventHandler(int amount);
    [Signal]
    public delegate void PlayerDeathEventHandler();

    public override void _PhysicsProcess(double delta)
    {
        MovePlayer();
    }

    public void SetUpSignals()
    {
        // PlayerReceiveDamage += OnReceiveDamage;
        PlayerDeath += OnPlayerDeath;

        GameManager.Instance.UI.GameplayUI.UpdateHealthLabel(Health);
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
        _sprite.Rotate((float)(Math.PI / 2));

        Godot.Collections.Array<Node> weapons = _weapons.GetChildren();
        foreach (BasicWeapon weapon in weapons.Cast<BasicWeapon>())
        {
            weapon.Timer.Stop();
        }
    }

    private void MovePlayer()
    {
        if (!Alive) return;
        Vector2 inputDirection = Input.GetVector("left", "right", "up", "down");
        Velocity = inputDirection * Speed;
        MoveAndSlide();
    }
}
