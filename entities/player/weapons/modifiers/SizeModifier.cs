using static Utils;

public partial class SizeModifier : BaseModifier
{
    public enum ModeEnum { GROW, SHRINK }
    private readonly ModeEnum _mode;
    private readonly float _percent;
    private static readonly int _minimumEntitySpeed = 10;

    public SizeModifier(RarityEnum rarity, ModeEnum mode) : base(rarity, ModifierEnum.SIZE_MOD)
    {
        _mode = mode;
        _percent = GetPercentIntByRarity();

        ModifierName = $"Size Modifier ({_mode.ToString().ToLower()})";

        if (_mode == ModeEnum.GROW)
        {
            Description = $"Bullets are bigger but slower (speed can't go below {_minimumEntitySpeed})";
            Effects = [
                new EffectLabel(EffectLabel.TypeEnum.POSITIVE, $"{_percent}% bullet size"),
                new EffectLabel(EffectLabel.TypeEnum.NEGATIVE, $"{_percent}% bullet speed"),
            ];
        }
        else
        {
            Description = "Bullets are smaller but faster";
            Effects = [
                new EffectLabel(EffectLabel.TypeEnum.POSITIVE, $"{_percent}% bullet speed"),
                new EffectLabel(EffectLabel.TypeEnum.NEGATIVE, $"{_percent}% bullet size"),
            ];
        }
    }

    public override WeaponEntity OnCreateEntity(WeaponEntity entity)
    {
        float percentage = _percent / 100;

        if (_mode == ModeEnum.GROW)
        {
            entity.Radius += entity.Radius * percentage;
            if (entity.Speed > _minimumEntitySpeed) { entity.Speed -= entity.Speed * percentage; }
        }
        else
        {
            entity.Speed += entity.Speed * percentage;
            entity.Radius -= entity.Radius * percentage;
        }

        return entity;
    }
}