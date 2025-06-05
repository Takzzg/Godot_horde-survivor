using Godot;
using System.Collections.Generic;

public partial class LevelUpOption : PanelContainer
{
    [Export]
    private Label _title;
    [Export]
    private BoxContainer _effectsCont;
    [Export]
    private RichTextLabel _descriptionRich;

    public void UpdateValues(string title, List<Control> effects, string description)
    {
        _title.Text = title;
        effects.ForEach(effect => _effectsCont.AddChild(effect));
        _descriptionRich.Text = description;
    }
}
