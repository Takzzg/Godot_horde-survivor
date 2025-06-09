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

    public partial class PositiveEffect : Label
    {
        public PositiveEffect(string text) : base()
        {
            Text = $"+ {text}";
            AddThemeColorOverride("font_color", Colors.LightGreen);
            HorizontalAlignment = HorizontalAlignment.Center;
        }
    }
    public partial class NegativeEffect : Label
    {
        public NegativeEffect(string text) : base()
        {
            Text = $"- {text}";
            AddThemeColorOverride("font_color", Colors.IndianRed);
            HorizontalAlignment = HorizontalAlignment.Center;
        }
    }

    // -------------------------------------------- Methods --------------------------------------------
    public virtual WeaponEntity OnCreateEntity(WeaponEntity entity)
    {
        return entity;
    }
}