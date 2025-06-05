using System.Collections.Generic;
using Godot;

public partial class PlayerWeapons(PlayerScene player, bool useDebug = true) : BasePlayerComponent(player, useDebug)
{
    public readonly List<BaseWeapon> WeaponsList = [];

    public void CreateWeapon(BaseWeapon weapon)
    {
        weapon.SetPlayerReference(_player);
        WeaponsList.Add(weapon);
        AddChild(weapon);
    }

    public void DestroyWeapon(BaseWeapon weapon)
    {
        WeaponsList.Remove(weapon);
        weapon.QueueFree();
    }

    public void OnPlayerDeath()
    {
        WeaponsList.ForEach(weapon => weapon.OnPlayerDeath());
    }

    public override DebugCategory DebugCreateCategory()
    {
        DebugCategory category = new("Player Weapons");
        for (int i = 0; i < WeaponsList.Count; i++)
        {
            BaseWeapon weapon = WeaponsList[i];
            category.CreateLabelField($"weapon_{i}", $"Weapon #{i}", weapon.GetWeaponType());
        }
        return category;
    }
}