using Godot;
using System;

public partial class PlayerMovement : CharacterBody2D
{
    [Export]
    private int Speed = 100;

    public void GetInput()
    {
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
