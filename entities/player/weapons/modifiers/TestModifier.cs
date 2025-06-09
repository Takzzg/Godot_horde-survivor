using System.Collections.Generic;
using Godot;

public partial class TestModifier : BaseModifier
{
    public TestModifier()
    {
        ModifierName = "Test Option";
        Description = "This is the test option. Grants you a [color=lightgreen]positive effect[/color] and a [color=indianred]negative effect[/color]";
        Effects = new List<Control>() {
            { new PositiveEffect("Positive effect") },
            { new NegativeEffect("Negative effect") }
        };
    }
}