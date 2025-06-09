using System.Collections.Generic;
using Godot;

public abstract partial class BaseModifier : DebuggerNode
{
    public string ModifierName;
    public string Description;
    public List<Control> Effects;

    public virtual void BeforeTrigger()
    {
        GD.Print($"Modifier BeforeTrigger");
    }

    public virtual void AfterTrigger(WeaponEntity entity)
    {
        GD.Print($"Modifier AfterTrigger {entity}");
    }

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
}