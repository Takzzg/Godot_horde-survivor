using System;
using Godot;

public partial class WeaponShooting(WeaponShooting.EnumStyle style, Action onShoot) : Node
{
    public enum EnumStyle { TIMER, MANUAL }
    public EnumStyle Style = style;
    private readonly Action _onShoot = onShoot;

    private Timer _timer = new() { Autostart = false, OneShot = false, WaitTime = 0.25 };

    public override void _Ready()
    {
        if (Style != EnumStyle.TIMER) return;

        // bind shoot timer
        _timer.Timeout += _onShoot;
        AddChild(_timer);

        _timer.Start();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (Style != EnumStyle.MANUAL) return;
        if (@event.IsActionPressed("enter")) { _onShoot(); }
    }

    public void TimedAttackSetRunning(bool state)
    {
        if (Style != EnumStyle.TIMER) return;

        if (state) _timer.Start();
        else _timer.Stop();
    }
}