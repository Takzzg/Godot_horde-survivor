using System;
using Godot;

public abstract partial class DebuggerNode : Control
{
    private DebugCategory _debug;

    public virtual DebugCategory DebugCreateCategory() => throw new NotImplementedException();

    public DebuggerNode(bool useDebug = true)
    {
        if (!useDebug) return;
        Ready += DebugRenderNode;
    }

    public void DebugRenderNode()
    {
        DebugSetState(DebugManager.Instance.DebugEnabled);
        DebugManager.Instance.DebugStateToggled += DebugSetState;

        TreeExiting += () =>
        {
            if (DebugManager.Instance.DebugEnabled) { DebugSetState(false); }
            DebugManager.Instance.DebugStateToggled -= DebugSetState;
        };
    }

    private void DebugSetState(bool state)
    {
        if (state == false)
        {
            _debug?.QueueFree();
            return;
        }

        _debug = DebugCreateCategory();
        DebugManager.Instance.RenderNode(_debug);
    }

    public void DebugTryUpdateField(string id, string value)
    {
        if (DebugManager.Instance.DebugEnabled == false) return;
        if (_debug == null) return;
        _debug.UpdateLabelField(id, value);
    }
}