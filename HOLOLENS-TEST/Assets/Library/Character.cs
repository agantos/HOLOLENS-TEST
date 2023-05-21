using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Character
{
    List<(string actionTypeName, int numPerTurn)> turnEconomy;
    Dictionary<string, Ability> abilities;
    CharacterStatistics stats;

    public void AddStat(CharacterStat stat) { stats.GetStatistics().Add(stat.GetName(), stat); }

    public CharacterStat GetCharacterStat(string name)
    {
        CharacterStat stat = stats.GetStat(name);
        Assert.IsNotNull(stat, "No stat with name = " + name + " exists.");
        return stats.GetStat(name);
    }

    public CharacterStatistics GetStats() { return stats; }
}