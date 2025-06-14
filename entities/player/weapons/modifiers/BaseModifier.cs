using System;
using System.Collections.Generic;
using Godot;
using static Utils;

public abstract partial class BaseModifier
{
    public string ModifierName;
    public string Description;
    public List<EffectLabel> Effects;
    public List<BaseWeapon.TypeEnum> IncompatibleWeapons = [];

    public RarityEnum Rarity;

    public enum ModifierEnum
    {
        TEST_MOD,
        SIZE_MOD,
        PIERCE_MOD,
    }
    public ModifierEnum Type;

    public BaseModifier(RarityEnum rarity, ModifierEnum type)
    {
        Rarity = rarity;
        Type = type;
    }

    public Label GetModifierDisplay()
    {
        Label control = new() { Text = $"{ModifierName} [{Rarity.ToString().Capitalize()}]" };
        return control;
    }


    // -------------------------------------------- Methods --------------------------------------------
    public virtual WeaponEntity OnCreateEntity(WeaponEntity entity)
    {
        return entity;
    }


    protected int GetPercentIntByRarity()
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

        int percent = GameManager.Instance.RNG.RandiRange(minValue, maxValue);
        return percent;
    }
}