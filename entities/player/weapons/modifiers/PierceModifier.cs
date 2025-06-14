using System;
using static Utils;

public partial class PierceModifier : BaseModifier
{
    public enum ModeEnum { SHARP, BLUNT }
    private readonly ModeEnum _mode;
    private readonly float _percent;
    private readonly int _pierceCount = 1;
    private static readonly int _minimumEntityPierce = 0;

    public PierceModifier(RarityEnum rarity, ModeEnum mode) : base(rarity, ModifierEnum.PIERCE_MOD)
    {
        IncompatibleWeapons = [BaseWeapon.TypeEnum.RELATIVE, BaseWeapon.TypeEnum.STATIONARY];

        _mode = mode;
        _percent = GetPercentIntByRarity();

        ModifierName = $"Pierce Modifier ({_mode.ToString().ToLower()})";

        if (_mode == ModeEnum.SHARP)
        {
            Description = "Bullets pierce one more enemy, and deal a bit more damage";
            Effects = [
                new EffectLabel(EffectLabel.TypeEnum.POSITIVE, $"{_pierceCount} bullet pierce"),
                new EffectLabel(EffectLabel.TypeEnum.POSITIVE, $"{_percent}% bullet damage"),
            ];
        }
        else
        {
            Description = $"Bullets pierce one less enemy, but deal more damage (pierce can't go below {_minimumEntityPierce})";
            Effects = [
                new EffectLabel(EffectLabel.TypeEnum.POSITIVE, $"{_percent}% bullet damage"),
                new EffectLabel(EffectLabel.TypeEnum.NEGATIVE, $"{_pierceCount} bullet pierce"),
            ];
        }
    }

    public override WeaponEntity OnCreateEntity(WeaponEntity entity)
    {
        float percentage = _percent / 100;

        if (_mode == ModeEnum.SHARP)
        {
            entity.MaxPierceCount += _pierceCount;
            entity.Damage += (float)(entity.Damage * Math.Floor(percentage / 4));
        }
        else
        {
            entity.Damage += entity.Damage * percentage;
            if (entity.MaxPierceCount > _minimumEntityPierce) { entity.MaxPierceCount -= _pierceCount; }
        }

        return entity;
    }
}