public abstract partial class BasePlayerComponent : DebuggerNode
{
    protected PlayerScene _player;

    public BasePlayerComponent(PlayerScene player, bool useDebug = true) : base(useDebug)
    {
        _player = player;
        _player.AddChild(this);
    }
}