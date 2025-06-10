using System.Collections.Generic;
using Godot;

public partial class TestModifier : BaseModifier
{
    public TestModifier() : base(RarityEnum.TEST, ModifierEnum.TEST_MOD)
    {
        ModifierName = "Test Option";
        Description = "This is the test option. Grants you a [color=lightgreen]positive effect[/color] and a [color=indianred]negative effect[/color]";
        Effects = new List<Control>() {
            { new Effect( Effect.TypeEnum.POSITIVE, "Positive effect") },
            { new Effect( Effect.TypeEnum.NEGATIVE, "Negative effect") }
        };
    }
}