using System;
using System.Collections.Generic;
using System.Linq;
using static BaseModifier;
using static Utils;

public partial class PlayerModifierGenerator(PlayerScene player) : BasePlayerComponent(player)
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

    private static readonly List<ModifierEnum> _allMods = [
        // ModifierEnum.TEST_MOD,
        ModifierEnum.SIZE_MOD,
        ModifierEnum.PIERCE_MOD,
    ];

    public Dictionary<RarityEnum, List<ModifierEnum>> RarityPools = new()
    {
        {RarityEnum.COMMON, _allMods},
        {RarityEnum.UNCOMMON, _allMods},
        {RarityEnum.RARE, _allMods},
        {RarityEnum.EPIC, _allMods},
        {RarityEnum.LEGENDARY, _allMods},
        {RarityEnum.UNIQUE, _allMods},
    };

    public static BaseModifier CreateModifier(ModifierEnum type, RarityEnum rarity)
    {
        return type switch
        {
            ModifierEnum.SIZE_MOD => GetSizeModifier(rarity),
            ModifierEnum.PIERCE_MOD => GetPierceModifier(rarity),
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

    private static T GetRandomEnumValue<T>()
    {
        Array allValues = Enum.GetValues(typeof(T));
        int randIdx = GameManager.Instance.RNG.RandiRange(0, allValues.Length - 1);
        return (T)allValues.GetValue(randIdx);
    }

    // ------------- Size -------------
    private static SizeModifier GetSizeModifier(RarityEnum rarity)
    {
        SizeModifier.ModeEnum mode = GetRandomEnumValue<SizeModifier.ModeEnum>();
        SizeModifier mod = new(rarity, mode);
        return mod;
    }

    // ------------- Pierce -------------
    private static PierceModifier GetPierceModifier(RarityEnum rarity)
    {
        PierceModifier.ModeEnum mode = GetRandomEnumValue<PierceModifier.ModeEnum>();
        PierceModifier mod = new(rarity, mode);
        return mod;
    }

}