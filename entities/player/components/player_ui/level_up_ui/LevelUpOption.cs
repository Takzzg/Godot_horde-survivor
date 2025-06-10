using Godot;
using System;

public partial class LevelUpOption : PanelContainer
{
    public BaseModifier Modifier;
    private Action<LevelUpOption> _onClick;

    [Export]
    private Label _title;

    [Export]
    private Label _rarity;
    [Export]
    private BoxContainer _effectsCont;
    [Export]
    private RichTextLabel _descriptionRich;


    public override void _Ready()
    {
        _descriptionRich.MouseFilter = MouseFilterEnum.Ignore;
    }

    public void UpdateValues(BaseModifier mod, Action<LevelUpOption> onClick = null)
    {
        Modifier = mod;
        _onClick = onClick;

        foreach (Node node in _effectsCont.GetChildren()) { node.QueueFree(); }

        _title.Text = mod.ModifierName;
        _rarity.Text = mod.Rarity.ToString().Capitalize();
        mod.Effects.ForEach(effect => _effectsCont.AddChild(effect));
        _descriptionRich.Text = mod.Description;
    }

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.ButtonIndex == MouseButton.Left && mouseButton.Pressed)
            {
                if (_onClick != null)
                {
                    _onClick(this);
                    GetViewport().SetInputAsHandled();
                }
            }
        }
    }
}
