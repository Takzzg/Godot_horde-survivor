using System;
using Godot;

public class WeaponAiming(WeaponAiming.EnumStyle style)
{
    public enum EnumStyle { RANDOM, FACING, FIXED_R }
    public EnumStyle Style = style;
    private Vector2 _lastFacingDirection = Vector2.Right;

    public Vector2 GetTrajectory()
    {
        return Style switch
        {
            EnumStyle.RANDOM => GetRandomTrajectory(),
            EnumStyle.FACING => GetFacingTrajectory(),
            EnumStyle.FIXED_R => Vector2.Right,
            _ => throw new NotImplementedException(),
        };
    }

    private static Vector2 GetRandomTrajectory()
    {
        Vector2 dir = Vector2.One.Rotated(GameManager.Instance.RNG.RandfRange(0, 360));
        return dir.Normalized();
    }

    private Vector2 GetFacingTrajectory()
    {
        Vector2 dir = PlayerMovement.GetInputVector();
        if (dir != Vector2.Zero) _lastFacingDirection = dir;

        return _lastFacingDirection.Normalized();
    }
}