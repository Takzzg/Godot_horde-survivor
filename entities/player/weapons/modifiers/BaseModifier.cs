using System.Collections.Generic;
using Godot;

public abstract partial class BaseModifier
{
    public string ModifierName;
    public string Description;
    public List<Control> Effects;

    public enum RarityEnum { TEST, COMMON, UNCOMMON, RARE, EPIC, LEGENDARY, UNIQUE }
    public RarityEnum Rarity;

    public enum ModifierEnum
    {
        TEST_MOD,
        SIZE_MOD,
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

    public partial class Effect : Label
    {
        public enum TypeEnum { NEGATIVE, POSITIVE }
        public Effect(TypeEnum type, string text) : base()
        {
            Text = $"{(type == TypeEnum.POSITIVE ? "+" : "-")} {text}";
            AddThemeColorOverride("font_color", type == TypeEnum.POSITIVE ? Colors.LightGreen : Colors.IndianRed);
            HorizontalAlignment = HorizontalAlignment.Center;
        }
    }

    // -------------------------------------------- Methods --------------------------------------------
    public virtual WeaponEntity OnCreateEntity(WeaponEntity entity)
    {
        return entity;
    }
}