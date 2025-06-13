using System.Collections.Generic;

public partial class PlayerWeapons(PlayerScene player, bool useDebug = true) : BasePlayerComponent(player, useDebug)
{
    public readonly List<BaseWeapon> WeaponsList = [];

    public void CreateWeapon(BaseWeapon weapon)
    {
        weapon.SetPlayerReference(_player);
        WeaponsList.Add(weapon);
        AddChild(weapon);
        DebugTryUpdateField("weapon_count", WeaponsList.Count.ToString());
    }

    public void DestroyWeapon(BaseWeapon weapon)
    {
        WeaponsList.Remove(weapon);
        weapon.QueueFree();
        DebugTryUpdateField("weapon_count", WeaponsList.Count.ToString());
    }

    public void OnPlayerDeath()
    {
        WeaponsList.ForEach(weapon => weapon.OnPlayerDeath());
    }

    public override DebugCategory DebugCreateCategory()
    {
        DebugCategory category = new("Player Weapons");
        category.CreateLabelField("weapon_count", "Count", WeaponsList.Count.ToString());
        return category;
    }
}