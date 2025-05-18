using Godot;
using System;

public partial class PlayerScene : CharacterBody2D
{
    [Export]
    public bool Alive = true;
    [Export]
    public int Health = 100;
    [Export]
    public int Speed = 100;

    [Export]
    private Sprite2D _sprite;

    [Signal]
    public delegate void PlayerReceiveDamageEventHandler(int amount);
    [Signal]
    public delegate void PlayerDeathEventHandler();

    public void SetUpSignals()
    {
        PlayerReceiveDamage += OnReceiveDamage;
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
        Alive = false;
        Velocity = new Vector2(0, 0);
        _sprite.Rotate((float)(Math.PI / 2));
    }

    private void GetInput()
    {
        if (!Alive) return;
        Vector2 inputDirection = Input.GetVector("left", "right", "up", "down");
        Velocity = inputDirection * Speed;
    }

    public override void _PhysicsProcess(double delta)
    {
        GetInput();
        MoveAndSlide();
    }
}
