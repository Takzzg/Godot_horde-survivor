using System;
using Godot;

public partial class SelectablePanel : PanelContainer
{
    protected Action _onSelect;

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.ButtonIndex == MouseButton.Left && mouseButton.Pressed)
            {
                if (_onSelect != null)
                {
                    _onSelect();
                    GetViewport().SetInputAsHandled();
                }
            }
        }
    }
}