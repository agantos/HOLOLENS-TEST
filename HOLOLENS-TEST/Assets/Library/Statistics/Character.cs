using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Character
{
    public string name;
    List<(string actionTypeName, int numPerTurn)> turnEconomy;
    public Dictionary<string, string> abilities = new Dictionary<string, string>();
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

    public Ability GetCharacterAbility(string name)
    {
        return AbilityManager.abilityPool[name];
    }

    public void ActivateOwnedAbility(string abilityName, Character defender = null, Character attacker = null)
    {
        if(abilities.TryGetValue(abilityName, out abilityName))
        {
            AbilityManager.ActivateAbilityEffect(abilityName, defender, attacker);
        }
        else
        {
            Assert.IsFalse(true, "Character " + name + " does not have the ability " + abilityName);
        }
    }

    public void LoadCharacterBasicPresetsFromPool(Dictionary<string, BaseCharacterPreset> basePresetsPool)
    {
        foreach(string presetName in basePresets)
        {
            basePresetsPool[presetName].AddPresetToCharacter(this);
        }
    }

    public string ToString(string prevTab)
    {
        string tab = "  ";
        string currTab = tab + prevTab;
        string s = prevTab + "Character: " + this.name + "\n";
        
        s += stats.ToString(currTab) + "\n";
        
        s += currTab + "Abilities: [  ";
        foreach (string ability in abilities.Values)
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