using Godot;

public abstract partial class EM_BaseComponent : Node2D
{
    protected EnemiesManager _manager;

    public EM_BaseComponent(EnemiesManager manager)
    {
        _manager = manager;
        _manager.AddChild(this);
    }
}