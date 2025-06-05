using Godot;
using Godot.Collections;

public partial class LevelUpUI : Panel
{
    [Export]
    private Array<LevelUpOption> _options;

    [Export]
    private Button _close;

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;
        _close.Pressed += QueueFree;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("back"))
        {
            GetViewport().SetInputAsHandled();
        }
    }
}
