using System;
using Godot;

public class WeaponAiming
{
    public enum EnumAimingStyle { RANDOM, FACING, FIXED_R }
    public EnumAimingStyle AimingStyle;
    private Vector2 _lastFacingDirection = Vector2.Right;

    public WeaponAiming(EnumAimingStyle style)
    {
        AimingStyle = style;
    }

    public Vector2 GetTrajectory()
    {
        return AimingStyle switch
        {
            EnumAimingStyle.RANDOM => GetRandomTrajectory(),
            EnumAimingStyle.FACING => GetFacingTrajectory(),
            EnumAimingStyle.FIXED_R => Vector2.Right,
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
        Vector2 dir = GameManager.Instance.Player.PlayerMovement.GetInputVector();
        if (dir != Vector2.Zero) _lastFacingDirection = dir;

        return _lastFacingDirection.Normalized();
    }
}