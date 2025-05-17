using Godot;
using System;

public partial class LoadingScreen : Control
{
    [Export]
    public ProgressBar Bar;
    [Export]
    public double TweenSpeed = 0.25;

    public Tween Tween;

    public void TweenProgressBar(double percentage, Action callback = null)
    {
        GD.Print($"TweenProgressBar()");

        GD.Print($"Bar: {Bar.Value}, percent: {percentage}");
        if (Bar.Value == percentage) return;

        if (Tween != null)
        {
            GD.Print($"tween running: {Tween.IsRunning()}");
            if (Tween.IsRunning()) return;
        }

        GD.Print($"creating tween");
        Tween = GetTree().CreateTween();
        Tween.TweenProperty(Bar, "value", percentage, TweenSpeed);

        if (callback != null)
        {
            GD.Print($"callback: {callback}");
            Tween.TweenCallback(Callable.From(callback));
        }
    }
}
