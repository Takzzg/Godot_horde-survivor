using Godot;
using System;
using System.Collections.Generic;
using static Utils;

public partial class LevelUpOption : SelectablePanel
{
    public enum OptionTypeEnum { WEAPON, MODIFIER }
    public OptionTypeEnum Type;
    public BaseWeapon Weapon;
    public BaseModifier Modifier;

    [Export]
    private Label _title;

    [Export]
    private Label _rarity;
    [Export]
    private BoxContainer _effectsCont;
    [Export]
    private RichTextLabel _descriptionRich;

    public Action OnConfirm;

    public override void _Ready()
    {
        _descriptionRich.MouseFilter = MouseFilterEnum.Ignore;
    }

    private void SetUpOption(OptionTypeEnum type, Action onSelect, Action onConfirm, string title, RarityEnum rarity, List<EffectLabel> effects, string desc)
    {
        Type = type;
        _onSelect = onSelect;
        OnConfirm = onConfirm;

        foreach (Node node in _effectsCont.GetChildren()) { node.QueueFree(); }

        _title.Text = title;
        _rarity.Text = rarity.ToString().Capitalize();
        effects.ForEach(effect => _effectsCont.AddChild(effect));
        _descriptionRich.Text = desc;
    }

    public void UpdateValues(BaseModifier mod, Action onSelect, Action onConfirm)
    {
        Modifier = mod;

        SetUpOption(
            OptionTypeEnum.MODIFIER,
            onSelect,
            onConfirm,
            mod.ModifierName,
            mod.Rarity,
            mod.Effects,
            mod.Description
        );
    }

    public void UpdateValues(BaseWeapon weapon, Action onSelect, Action onConfirm)
    {
        Weapon = weapon;

        SetUpOption(
            OptionTypeEnum.WEAPON,
            onSelect,
            onConfirm,
            weapon.Type.ToString().Capitalize(),
            weapon.Rarity,
            [new EffectLabel(EffectLabel.TypeEnum.POSITIVE, "New weapon!")],
            weapon.Description
        );
    }
}
