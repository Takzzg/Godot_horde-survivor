using System;
using Godot;

public partial class PlayerTrigger : Node2D
{
    private Area2D _area;
    private readonly Action<PlayerScene> _playerCallback;
    private Vector2 _rectSize = new(16, 16);
    private Theme _theme = new() { DefaultFontSize = 4 };

    public PlayerTrigger(string title, Action<PlayerScene> callback)
    {
        _playerCallback = callback;

        // create area
        _area = new Area2D() { CollisionMask = 1, /* player layer */};
        _area.BodyEntered += OnPlayerBodyEntered;
        AddChild(_area);

        // create collision shape
        CircleShape2D circle = new() { Radius = _rectSize.X / 2 };
        CollisionShape2D shape = new() { Shape = circle };
        _area.AddChild(shape);

        // create color rect
        ColorRect rect = new()
        {
            Size = _rectSize,
            Position = -_rectSize / 2,
            Color = new Color(Colors.Black, 0.25f)
        };
        AddChild(rect);

        // create label
        WorldTextCentered label = new(title, _theme);
        AddChild(label);
    }

    private void OnPlayerBodyEntered(Node2D body)
    {
        PlayerScene player = body as PlayerScene;
        _playerCallback(player);
    }
}