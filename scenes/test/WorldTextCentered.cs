using Godot;

public partial class WorldTextCentered : CenterContainer
{
    public WorldTextCentered(string title, Theme theme)
    {
        GrowHorizontal = GrowDirection.Both;
        GrowVertical = GrowDirection.Both;

        Label weaponTypesLabel = new() { Text = title, Theme = theme };
        AddChild(weaponTypesLabel);
    }
}