using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Character
{
    public string name;
    List<(string actionTypeName, int numPerTurn)> turnEconomy;
    public List<string> abilities = new List<string>();
    public List<string> basePresets = new List<string>();
    public List<string> additionalPresets = new List<string>();
    CharacterStatistics stats = new CharacterStatistics();

    public void AddStat(CharacterStat stat) { stats.GetStatistics().Add(stat.GetName(), stat); }

    public CharacterStat GetCharacterStat(string name)
    {
        CharacterStat stat = stats.GetStat(name);
        Assert.IsNotNull(stat, "No stat with name = " + name + " exists.");
        return stats.GetStat(name);
    }

    public Ability GetCharacterAbility(string name, Dictionary<string, Ability> globalAbilityPool)
    {
        return globalAbilityPool[name];
    }

    public void LoadCharacterBasicPresets(Dictionary<string, BaseCharacterPreset> presets)
    {
        foreach(string presetName in basePresets)
        {
            presets[presetName].AddPresetToCharacter(this);
        }
    }

    public string ToString(string prevTab)
    {
        string tab = "  ";
        string currTab = tab + prevTab;
        string s = prevTab + "Character: " + this.name + "\n";
        
        s += stats.ToString(currTab) + "\n";
        
        s += currTab + "Abilities: [  ";
        foreach (string ability in abilities)
        {
            s += ability + "  |  ";
        }
        s += "]\n";

        s += currTab + "BasePresets: [  ";
        foreach (string preset in basePresets)
        {
            s += preset + "  |  ";
        }
        s += "]";

        return s;
    }

    public CharacterStatistics GetStats() { return stats; }
}