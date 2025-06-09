using System;

public partial class SizeModifier : BaseModifier
{
    public enum ModeEnum { GROW, SHRINK }
    private ModeEnum _mode;

    private int _sizeOffset = 1;
    private int _speedOffset = 5;

    public SizeModifier(ModeEnum mode)
    {
        _mode = mode;

        ModifierName = "Size Modifier";

        if (_mode == ModeEnum.GROW)
        {
            Description = "Bigger but slower bullets";
            Effects = [
                new PositiveEffect($"{_sizeOffset} bullet size"),
                new NegativeEffect($"{_speedOffset} bullet speed"),
            ];
        }
        else
        {
            Description = "Smaller but faster bullets";
            Effects = [
                new PositiveEffect($"{_speedOffset} bullet speed"),
                new NegativeEffect($"{_sizeOffset} bullet size"),
            ];
        }
    }

    public override WeaponEntity OnCreateEntity(WeaponEntity entity)
    {
        if (_mode == ModeEnum.GROW)
        {
            entity.Radius += _sizeOffset;
            entity.Speed -= Math.Max(_speedOffset, 5);
        }
        else
        {
            entity.Speed += _speedOffset;
            entity.Radius -= Math.Max(_sizeOffset, 1);
        }

        return entity;
    }
}