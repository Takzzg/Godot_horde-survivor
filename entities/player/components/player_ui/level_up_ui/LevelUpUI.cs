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

    private LevelUpOption _selectedOption = null;
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
        // ------------- choosing weapon -------------
        if (_selectedOption == null)
        {
            _confirm.Disabled = true;
            return;
        }

        if (_selectedOption.Type == LevelUpOption.OptionTypeEnum.WEAPON)
        {
            if (_selectedWeaponDisplay != null)
            {
                SetSelectedWeapon(null);
                return;
            }

            _confirm.Disabled = false;
            return;
        }

        // ------------- choosing mod -------------
        if (_selectedWeaponDisplay == null)
        {
            _confirm.Disabled = true;
            return;
        }

        if (_selectedOption.Modifier.IncompatibleWeapons.Contains(_selectedWeaponDisplay.Weapon.Type))
        {
            SetSelectedWeapon(null);
            return;
        }

        _confirm.Disabled = false;
    }

    private void OnConfirm()
    {
        _selectedOption.OnConfirm();
        QueueFree();
    }

    // -------------------------------------------- Player --------------------------------------------
    private PlayerScene _player;
    public void SetPlayerReference(PlayerScene player)
    {
        _player = player;
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
            option.UpdateValues(
                mod,
                () => SetSelectedOption(option),
                () => _selectedWeaponDisplay.Weapon.AddModifier(_selectedOption.Modifier)
            );
        });
    }

    public void UpdateOptions(List<BaseWeapon> weapons)
    {
        foreach (Node node in _optionsCont.GetChildren()) { node.QueueFree(); }

        PackedScene optionScene = ResourcePaths.GetPackedSceneFromEnum(ResourcePaths.ScenePathsEnum.LEVEL_UP_OPTION);
        weapons.ForEach(weapon =>
        {
            LevelUpOption option = ResourcePaths.InstantiatePackedScene<LevelUpOption>(optionScene);
            _optionsCont.AddChild(option);
            option.UpdateValues(
                weapon,
                () => SetSelectedOption(option),
                () => _player.PlayerWeapons.CreateWeapon(option.Weapon)
            );
        });
    }

    private void SetSelectedOption(LevelUpOption option)
    {
        if (_selectedOption != null) { _selectedOption.Scale = Vector2.One; }
        if (option == _selectedOption) { _selectedOption = null; }
        else
        {
            _selectedOption = option;
            _selectedOption.PivotOffset = _selectedOption.Size / 2;
            _selectedOption.Scale = new Vector2(1.1f, 1.1f);
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
