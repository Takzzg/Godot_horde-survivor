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

    public override void _Ready()
    {
        PlayerReceiveDamage += ReceiveDamage;
    }

    public void ReceiveDamage(int amount)
    {
        GD.Print($"player received {amount} damage");
        Health -= amount;
        if (Health < 0) Health = 0;

        GameManager.Instance.UI.UpdateHealthLabel(Health);
        if (Health > 0) return;

        Alive = false;
        Velocity = new Vector2(0, 0);
        _sprite.Rotate(1);
    }

    public void GetInput()
    {
        if (!Alive) return;
        Vector2 inputDirection = Input.GetVector("left", "right", "up", "down");
        Velocity = inputDirection * Speed;
    }

    public override void _PhysicsProcess(double delta)
    {
        GetInput();
        MoveAndSlide();

        // for (int i = 0; i < GetSlideCollisionCount(); i++)
        // {
        //     var collision = GetSlideCollision(i);
        //     GD.Print("I collided with ", ((Node)collision.GetCollider()).Name);
        // }
    }
}
