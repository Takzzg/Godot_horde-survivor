using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class PlayerWeapons(PlayerScene player) : BasePlayerComponent(player)
{
    public readonly List<BaseWeapon> WeaponsList = [];

    public void CreateWeapon(BaseWeapon weapon)
    {
        weapon.SetPlayerReference(_player);
        WeaponsList.Add(weapon);
        AddChild(weapon);

        _player.DebugTryUpdateField("weapon_count", WeaponsList.Count.ToString());
        DebugUpdateWeaponTypes();
    }

    public void DestroyWeapon(BaseWeapon weapon)
    {
        WeaponsList.Remove(weapon);
        weapon.QueueFree();

        _player.DebugTryUpdateField("weapon_count", WeaponsList.Count.ToString());
        DebugUpdateWeaponTypes();
    }

    public void OnPlayerDeath()
    {
        WeaponsList.ForEach(weapon => weapon.OnPlayerDeath());
    }

    // -------------------------------------------- Debug --------------------------------------------

    public override void DebugCreateSubCategory(DebugCategory category)
    {
        category.CreateDivider("Weapons");
        category.CreateLabelField("weapon_count", "Count", WeaponsList.Count.ToString());

        List<string> types = [];
        WeaponsList.ForEach(weapon => types.Add(weapon.Type.ToString().Capitalize()));
        category.CreateLabelField("weapon_types", "Types", types.ToArray().Join(", "));
    }

    private void DebugUpdateWeaponTypes()
    {
        List<string> types = [];
        WeaponsList.ForEach(weapon => types.Add(weapon.Type.ToString().Capitalize()));
        _player.DebugTryUpdateField("weapon_types", types.ToArray().Join(", "));
    }
}