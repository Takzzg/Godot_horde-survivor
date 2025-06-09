using System.Collections.Generic;
using Godot;

public partial class BaseModifier(string name, string desc, List<Control> effects) : DebuggerNode
{
    public string ModifierName = name;
    public string Description = desc;
    public List<Control> Effects = effects;

    public void BeforeTrigger()
    {
        GD.Print($"Modifier BeforeTrigger");
    }

    public void AfterTrigger(WeaponEntity entity)
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