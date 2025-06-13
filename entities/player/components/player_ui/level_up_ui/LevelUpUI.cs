using System.Collections.Generic;
using Godot;

public partial class LevelUpUI : Panel
{
    [Export]
    private BoxContainer _optionsCont;
    [Export]
    private BoxContainer _weaponsCont;
    [Export]
    private Button _confirm;

    [Export]
    private Button _close;

    private LevelUpOption _selectedModOption = null;
    private WeaponDisplay _selectedWeaponDisplay = null;

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;

        _close.Pressed += QueueFree;
        _confirm.Pressed += OnConfirm;

        CheckSelection();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("back"))
        {
            GetViewport().SetInputAsHandled();
        }
    }

    private void CheckSelection()
    {
        if (_selectedModOption == null || _selectedWeaponDisplay == null)
        {
            _confirm.Disabled = true;
            return;
        }

        if (_selectedModOption.Modifier.IncompatibleWeapons.Contains(_selectedWeaponDisplay.Weapon.Type))
        {
            SetSelectedWeapon(null);
            return;
        }

        _confirm.Disabled = false;
    }

    private void OnConfirm()
    {
        _selectedWeaponDisplay.Weapon.AddModifier(_selectedModOption.Modifier);
        QueueFree();
    }

    // -------------------------------------------- Options --------------------------------------------
    public void UpdateOptions(List<BaseModifier> modifiers)
    {
        foreach (Node node in _optionsCont.GetChildren()) { node.QueueFree(); }

        PackedScene optionScene = ResourcePaths.GetPackedSceneFromEnum(ResourcePaths.ScenePathsEnum.LEVEL_UP_OPTION);
        modifiers.ForEach(mod =>
        {
            LevelUpOption option = ResourcePaths.InstantiatePackedScene<LevelUpOption>(optionScene);
            _optionsCont.AddChild(option);
            option.UpdateValues(mod, SetSelectedOption);
        });
    }

    private void SetSelectedOption(LevelUpOption option)
    {
        if (_selectedModOption != null) { _selectedModOption.Scale = Vector2.One; }
        if (option == _selectedModOption) { _selectedModOption = null; }
        else
        {
            _selectedModOption = option;
            _selectedModOption.PivotOffset = _selectedModOption.Size / 2;
            _selectedModOption.Scale = new Vector2(1.1f, 1.1f);
        }

        CheckSelection();
    }

    // -------------------------------------------- Weapons --------------------------------------------
    public void UpdateWeapons(List<BaseWeapon> list)
    {
        foreach (Node node in _weaponsCont.GetChildren()) { node.QueueFree(); }

        list.ForEach(weapon =>
        {
            _weaponsCont.AddChild(weapon.GetWeaponDisplay(SetSelectedWeapon));
        });
    }

    private void SetSelectedWeapon(WeaponDisplay weapon)
    {
        if (_selectedWeaponDisplay != null) { _selectedWeaponDisplay.Scale = Vector2.One; }
        if (weapon == _selectedWeaponDisplay || weapon == null) { _selectedWeaponDisplay = null; }
        else
        {
            _selectedWeaponDisplay = weapon;
            _selectedWeaponDisplay.PivotOffset = _selectedWeaponDisplay.Size / 2;
            _selectedWeaponDisplay.Scale = new Vector2(1.1f, 1.1f);
        }

        CheckSelection();
    }
}
