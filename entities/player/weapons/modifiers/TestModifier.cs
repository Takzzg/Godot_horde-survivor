using System.Collections.Generic;
using Godot;
using static Utils;

public partial class TestModifier : BaseModifier
{
    public TestModifier() : base(RarityEnum.TEST, ModifierEnum.TEST_MOD)
    {
        ModifierName = "Test Option";
        Description = "This is the test option. Grants you a [color=lightgreen]positive effect[/color] and a [color=indianred]negative effect[/color]";
        Effects = new List<EffectLabel>() {
            { new EffectLabel(EffectLabel.TypeEnum.POSITIVE, "Positive effect") },
            { new EffectLabel(EffectLabel.TypeEnum.NEGATIVE, "Negative effect") }
        };
    }
}