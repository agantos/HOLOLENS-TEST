using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Character
{
    public string name;
    public Dictionary<string, int> turnEconomy;
    public Dictionary<string, int> currentTurnEconomy;

    public Dictionary<string, string> abilities = new Dictionary<string, string>();
    public List<string> basePresets = new List<string>();
    public List<string> additionalPresets = new List<string>();
    CharacterStatistics stats = new CharacterStatistics();

    public void LoadCharacterBasicPresetsFromPool(Dictionary<string, BaseCharacterPreset> basePresetsPool)
    {
        foreach (string presetName in basePresets)
        {
            basePresetsPool[presetName].AddPresetToCharacter(this);
        }
    }
    public void AddStat(CharacterStat stat) { stats.GetStatistics().Add(stat.GetName(), stat); }   
    public Ability GetCharacterAbility(string name)
    {
        return AbilityManager.abilityPool[name];
    }
    public void ActivateOwnedAbility(string abilityName, List<Character> defenders = null, Character attacker = null)
    {
        if(abilities.TryGetValue(abilityName, out abilityName))
        {
            foreach(Character defender in defenders)
                AbilityManager.Activate_PerformEffect(abilityName, defender, attacker);

            AbilityManager.Activate_ApplyCost(abilityName, attacker);
        }
        else
        {
            Assert.IsFalse(true, "Character " + name + " does not have the ability " + abilityName);
        }
    }
    public void RefreshTurnEconomy()
    {
        foreach(string name in turnEconomy.Keys)
        {
            currentTurnEconomy[name] = turnEconomy[name];
        }
    }
    public string ToString(string prevTab)
    {
        string tab = "  ";
        string currTab = tab + prevTab;
        string s = prevTab + "Character: " + this.name + "\n";
        
        s += stats.ToString(currTab) + "\n";

        s += currTab + "TurnEconomy: [  ";
        foreach(string actionName in turnEconomy.Keys)
            s += currTab + tab + actionName + " " + turnEconomy[actionName] +"\n";

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
    public CharacterStat GetCharacterStat(string name)
    {
        CharacterStat stat = stats.GetStat(name);
        Assert.IsNotNull(stat, "No stat with name = " + name + " exists.");
        return stats.GetStat(name);
    }
}