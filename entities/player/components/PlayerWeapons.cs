using System.Collections.Generic;

public partial class PlayerWeapons(PlayerScene player) : BasePlayerComponent(player)
{
    public readonly List<BaseWeapon> WeaponsList = [];

    public void CreateWeapon(BaseWeapon weapon)
    {
        weapon.SetPlayerReference(_player);
        WeaponsList.Add(weapon);
        AddChild(weapon);

        _player.DebugTryUpdateField("weapon_count", WeaponsList.Count.ToString());
    }

    public void DestroyWeapon(BaseWeapon weapon)
    {
        WeaponsList.Remove(weapon);
        weapon.QueueFree();

        _player.DebugTryUpdateField("weapon_count", WeaponsList.Count.ToString());
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
    }
}