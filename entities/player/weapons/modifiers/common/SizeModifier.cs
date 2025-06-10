using System;

public partial class SizeModifier : BaseModifier
{
    public enum ModeEnum { GROW, SHRINK }
    private ModeEnum _mode;
    private float _percent;

    public SizeModifier(RarityEnum rarity, ModeEnum mode) : base(rarity, ModifierEnum.SIZE_MOD)
    {
        _mode = mode;
        GetPercentageByRarity();

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

    private void GetPercentageByRarity()
    {
        int minValue;
        int maxValue;

        switch (Rarity)
        {
            case RarityEnum.COMMON:
                minValue = 1;
                maxValue = 2;
                break;

            case RarityEnum.UNCOMMON:
                minValue = 2;
                maxValue = 5;
                break;

            case RarityEnum.RARE:
                minValue = 5;
                maxValue = 8;
                break;

            case RarityEnum.EPIC:
                minValue = 8;
                maxValue = 12;
                break;

            case RarityEnum.LEGENDARY:
                minValue = 12;
                maxValue = 16;
                break;

            case RarityEnum.UNIQUE:
                minValue = 16;
                maxValue = 20;
                break;

            default:
                throw new NotImplementedException();
        }

        _percent = GameManager.Instance.RNG.RandiRange(minValue, maxValue);
    }
}