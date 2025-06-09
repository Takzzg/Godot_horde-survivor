using System.Collections.Generic;
using Godot;

public static class Modifiers
{
    public static readonly BaseModifier TestMod = new(
        "Test Option",
        "This is the test option. Grants you a [color=lightgreen]positive effect[/color] and a [color=indianred]negative effect[/color]",
        new List<Control>() {
            { new BaseModifier.PositiveEffect("Positive effect") },
            { new BaseModifier.NegativeEffect("Negative effect") }
        }
    );
}