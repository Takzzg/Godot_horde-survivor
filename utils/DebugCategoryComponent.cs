using System;
using Godot;

public partial class DebugCategoryComponent : Node2D
{
    // debug
    private DebugManager.DebugCategory _debug;
    private Action<DebugCategoryComponent> CreateDebugCategory;

    public DebugCategoryComponent(Action<DebugCategoryComponent> createCat)
    {
        CreateDebugCategory = createCat;
        DebugManager.Instance.DebugStateToggled += SetDebug;

        SetDebug(DebugManager.Instance.DebugEnabled);
    }

    public override void _ExitTree()
    {
        DebugManager.Instance.DebugStateToggled -= SetDebug;
        if (DebugManager.Instance.DebugEnabled) _debug?.QueueFree();
    }

    private void SetDebug(bool state)
    {
        if (state == false)
        {
            _debug?.QueueFree();
            return;
        }

        CreateDebugCategory(this);
    }

    public void TryCreateCategory(DebugManager.DebugCategory category)
    {
        if (DebugManager.Instance.DebugEnabled == false) return;
        _debug = category;
        DebugManager.Instance.RenderNode(_debug);
    }

    public void TryCreateField(string id, string title, string value)
    {
        if (DebugManager.Instance.DebugEnabled == false) return;
        if (_debug == null) return;
        _debug.CreateField(id, title, value);
    }

    public void TryUpdateField(string id, string value)
    {
        if (DebugManager.Instance.DebugEnabled == false) return;
        if (_debug == null) return;
        _debug.UpdateFieldValue(id, value);
    }
}