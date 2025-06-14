using System;
using Godot;

public abstract partial class BasePlayerComponent : Node2D
{
    protected PlayerScene _player;

    public BasePlayerComponent(PlayerScene player)
    {
        _player = player;
        _player.AddChild(this);
    }

    public virtual void DebugCreateSubCategory(DebugCategory category)
    {
        throw new NotImplementedException();
    }
}