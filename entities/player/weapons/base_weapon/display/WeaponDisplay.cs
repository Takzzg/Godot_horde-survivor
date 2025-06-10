using Godot;
using System;

public partial class WeaponDisplay : PanelContainer
{
    public BaseWeapon Weapon;
    private Action<WeaponDisplay> _onClick;

    [Export]
    private Label _weaponName;

    [Export]
    private Label _trajectoryLabel;
    [Export]
    private Label _delayLabel;
    [Export]
    private Label _collisionsLabel;

    [Export]
    private Label _damageLabel;
    [Export]
    private Label _radiusLabel;
    [Export]
    private Label _speedLabel;
    [Export]
    private Label _pierceLabel;
    [Export]
    private Label _maxDistLabel;
    [Export]
    private Label _lifespanLabel;

    [Export]
    private BoxContainer _modsCont;
    [Export]
    private Label _modsCountLabel;


    public void UpdateValues(BaseWeapon weapon, Action<WeaponDisplay> onClick = null)
    {
        Weapon = weapon;
        _onClick = onClick;

        // weapon
        _weaponName.Text = $"{weapon.Type.ToString().Capitalize()} weapon";

        _trajectoryLabel.Text = weapon.Trajectory.ToString();
        _delayLabel.Text = weapon.TimerDelay.ToString();
        _collisionsLabel.Text = weapon.MaxCollisionsPerFrame.ToString();

        // create entity
        WeaponEntity baseEntity = weapon.CreateEntity();
        // apply modifiers
        weapon.Modifiers.ForEach(mod => mod.OnCreateEntity(baseEntity));

        _damageLabel.Text = baseEntity.Damage.ToString();
        _radiusLabel.Text = baseEntity.Radius.ToString();
        _speedLabel.Text = baseEntity.Speed.ToString();
        _pierceLabel.Text = baseEntity.MaxPierceCount.ToString();
        _maxDistLabel.Text = baseEntity.MaxDistance.ToString();
        _lifespanLabel.Text = baseEntity.LifeTime.ToString();

        // modifiers
        _modsCountLabel.Text = weapon.Modifiers.Count.ToString();
        weapon.Modifiers.ForEach(mod =>
        {
            _modsCont.AddChild(mod.GetModifierDisplay());
        });
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
                    AcceptEvent();
                }
            }
        }
    }
}
