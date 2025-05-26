using Godot;

public partial class PlayerMovement(PlayerScene player) : Node
{
    private PlayerScene _player = player;

    public override void _PhysicsProcess(double delta)
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector2 inputDirection = GetInputVector();
        if (inputDirection.Equals(Vector2.Zero)) return;

        _player.Velocity = inputDirection * _player.Speed;
        _player.EmitSignal(PlayerScene.SignalName.PlayerMove);
        _player.MoveAndSlide();
    }

    public Vector2 GetInputVector()
    {
        Vector2 inputDirection = Input.GetVector("left", "right", "up", "down");
        return inputDirection;
    }
}