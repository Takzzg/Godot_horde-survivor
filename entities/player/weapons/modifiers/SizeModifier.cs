using System;

public partial class SizeModifier : BaseModifier
{
    public enum ModeEnum { GROW, SHRINK }
    private ModeEnum _mode;
    private float _percent;

    public SizeModifier(ModeEnum mode, float percent)
    {
        _mode = mode;
        _percent = percent;

        ModifierName = $"Size Modifier ({_mode.ToString().ToLower()})";

        if (_mode == ModeEnum.GROW)
        {
            Description = "Bigger but slower bullets";
            Effects = [
                new Effect(Effect.TypeEnum.POSITIVE, $"{_percent}% bullet size"),
                new Effect(Effect.TypeEnum.NEGATIVE, $"{_percent}% bullet speed"),
            ];
        }
        else
        {
            Description = "Smaller but faster bullets";
            Effects = [
                new Effect(Effect.TypeEnum.POSITIVE, $"{_percent}% bullet speed"),
                new Effect(Effect.TypeEnum.NEGATIVE, $"{_percent}% bullet size"),
            ];
        }
    }

    public override WeaponEntity OnCreateEntity(WeaponEntity entity)
    {
        float percentage = _percent / 100;

        if (_mode == ModeEnum.GROW)
        {
            entity.Radius += entity.Radius * percentage;
            entity.Speed -= entity.Speed * percentage;
        }
        else
        {
            entity.Speed += entity.Speed * percentage;
            entity.Radius -= entity.Radius * percentage;
        }

        return entity;
    }
}