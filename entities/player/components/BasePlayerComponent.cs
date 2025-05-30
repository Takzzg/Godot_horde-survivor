public abstract partial class BasePlayerComponent : DebugNode2D
{
    protected PlayerScene _player;

    public BasePlayerComponent(PlayerScene player, bool useDebug = true) : base(useDebug)
    {
        _player = player;
        _player.AddChild(this);
    }
}