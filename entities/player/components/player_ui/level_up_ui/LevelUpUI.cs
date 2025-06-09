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
    private WeaponDisplay _selectedWeapon = null;

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
        _confirm.Disabled = _selectedOption == null || _selectedWeapon == null;
    }

    private void OnConfirm()
    {
        _selectedWeapon.Weapon.AddModifier(_selectedOption.Modifier);
        QueueFree();
    }

    // -------------------------------------------- Options --------------------------------------------
    public void UpdateOptions()
    {
        foreach (Node node in _optionsCont.GetChildren()) { node.QueueFree(); }

        PackedScene optionScene = ResourcePaths.GetPackedSceneFromEnum(ResourcePaths.ScenePathsEnum.LEVEL_UP_OPTION);

        LevelUpOption testOption = ResourcePaths.InstantiatePackedScene<LevelUpOption>(optionScene);
        _optionsCont.AddChild(testOption);
        testOption.UpdateValues(new TestModifier(), SetSelectedOption);

        LevelUpOption growOption = ResourcePaths.InstantiatePackedScene<LevelUpOption>(optionScene);
        _optionsCont.AddChild(growOption);
        growOption.UpdateValues(new SizeModifier(SizeModifier.ModeEnum.GROW, 5), SetSelectedOption);

        LevelUpOption shrinkOption = ResourcePaths.InstantiatePackedScene<LevelUpOption>(optionScene);
        _optionsCont.AddChild(shrinkOption);
        shrinkOption.UpdateValues(new SizeModifier(SizeModifier.ModeEnum.SHRINK, 5), SetSelectedOption);
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
        if (_selectedWeapon != null) { _selectedWeapon.Scale = Vector2.One; }
        if (weapon == _selectedWeapon) { _selectedWeapon = null; }
        else
        {
            _selectedWeapon = weapon;
            _selectedWeapon.PivotOffset = _selectedWeapon.Size / 2;
            _selectedWeapon.Scale = new Vector2(1.1f, 1.1f);
        }

        CheckSelection();
    }
}
