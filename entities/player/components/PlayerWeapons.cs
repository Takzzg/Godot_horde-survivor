using System.Collections.Generic;

public partial class PlayerWeapons(PlayerScene player, bool useDebug = true) : BasePlayerComponent(player, useDebug)
{
    private readonly List<BaseWeapon> _weaponsList = [];

    public void CreateWeapon(BaseWeapon weapon)
    {
        weapon.SetPlayerReference(_player);
        _weaponsList.Add(weapon);
        AddChild(weapon);
    }

    public void OnPlayerDeath()
    {
        _weaponsList.ForEach(weapon => weapon.OnPlayerDeath());
    }

    public override DebugCategory DebugCreateCategory()
    {
        DebugCategory category = new("Player Weapons");
        for (int i = 0; i < _weaponsList.Count; i++)
        {
            BaseWeapon weapon = _weaponsList[i];
            category.CreateLabelField($"weapon_{i}", $"Weapon #{i}", weapon.DebugGetCategoryTitle());
        }
        return category;
    }
}