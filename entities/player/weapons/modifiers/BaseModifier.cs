using System.Collections.Generic;
using Godot;

public abstract partial class BaseModifier : DebuggerNode
{
    public string ModifierName;
    public string Description;
    public List<Control> Effects;

    public Label GetModifierDisplay()
    {
        Label control = new() { Text = ModifierName };
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