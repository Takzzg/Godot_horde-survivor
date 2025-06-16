using System;
using Godot;

public abstract partial class BaseComponent<T> : Node2D where T : Node
{
    protected T Parent { get; private set; }

    public BaseComponent(T parent)
    {
        Parent = parent;
        Parent.AddChild(this);
    }

    public virtual void DebugCreateSubCategory(DebugCategory category)
    {
        throw new NotImplementedException();
    }
}