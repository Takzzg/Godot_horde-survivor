using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using static BaseModifier;

public partial class PlayerModifierGenerator(PlayerScene player) : BasePlayerComponent(player, false)
{
    public Dictionary<RarityEnum, int> RarityWeights = new()
    {
        {RarityEnum.COMMON, 9},
        {RarityEnum.UNCOMMON, 2},
        {RarityEnum.RARE, 1},
        {RarityEnum.EPIC, 0},
        {RarityEnum.LEGENDARY, 0},
        {RarityEnum.UNIQUE, 0},
    };

    public Dictionary<RarityEnum, List<ModifierEnum>> RarityPools = new()
    {
        {RarityEnum.COMMON, [ModifierEnum.SIZE_MOD ]},
        {RarityEnum.UNCOMMON, [ModifierEnum.SIZE_MOD ]},
        {RarityEnum.RARE, [ModifierEnum.SIZE_MOD ]},
        {RarityEnum.EPIC, [ModifierEnum.SIZE_MOD ]},
        {RarityEnum.LEGENDARY, [ModifierEnum.SIZE_MOD ]},
        {RarityEnum.UNIQUE, [ModifierEnum.SIZE_MOD ]},
    };

    public static BaseModifier CreateModifier(ModifierEnum type, RarityEnum rarity)
    {
        return type switch
        {
            ModifierEnum.SIZE_MOD => GetSizeModifier(rarity),
            _ => throw new NotImplementedException(),
        };
    }

    public List<BaseModifier> GetModifierOptions()
    {
        // get weighted mods from rarity pool
        List<(ModifierEnum type, RarityEnum rarity)> options = [];

        foreach (RarityEnum rarity in RarityWeights.Keys)
        {
            for (int i = 0; i < RarityWeights[rarity]; i++)
            {
                int randIdx = GameManager.Instance.RNG.RandiRange(0, RarityPools[rarity].Count - 1);
                ModifierEnum type = RarityPools[rarity].ElementAt(randIdx);

                options.Add((type, rarity));
            }
        }
        // options.ForEach(opt => { GD.Print($"opt: {opt}"); });

        // get random mods from weighted list
        int maxOptions = 3;
        List<BaseModifier> picked = [];

        for (int i = 0; i < maxOptions; i++)
        {
            int randIdx = GameManager.Instance.RNG.RandiRange(0, options.Count - 1);
            (ModifierEnum type, RarityEnum rarity) = options.ElementAt(randIdx);
            picked.Add(CreateModifier(type, rarity));
            options.RemoveAt(randIdx);
        }

        // picked.ForEach(pick => { GD.Print($"pick: {pick}"); });
        return picked;
    }

    private static SizeModifier GetSizeModifier(RarityEnum rarity)
    {
        Array values = Enum.GetValues(typeof(SizeModifier.ModeEnum));
        int randIdx = GameManager.Instance.RNG.RandiRange(0, values.Length - 1);
        SizeModifier.ModeEnum mode = (SizeModifier.ModeEnum)values.GetValue(randIdx);

        float percent = GameManager.Instance.RNG.RandiRange(5, 10);

        SizeModifier mod = new(rarity, mode);
        return mod;
    }
}