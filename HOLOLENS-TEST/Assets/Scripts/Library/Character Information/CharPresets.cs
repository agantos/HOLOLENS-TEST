using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPreset
{
    protected string name;
    protected List<string> presetAbilities = new List<string>();
    public CharacterStats stats = new CharacterStats();

    public void AddPresetToCharacter(Character character)
    {
        AddStatisticsToCharacter(character);
        AddAbilitiesToCharacter(character);
    }

    void AddStatisticsToCharacter(Character character) {
        Debug.Log("Base Class");
    }

    protected void AddAbilitiesToCharacter(Character character){
        foreach(string ability in presetAbilities)
        {
            if(!character.abilities.ContainsKey(ability))
                character.abilities.Add(ability, ability);
        }
    }

}
public class BaseCharacterPreset : CharacterPreset
{
    public new void AddPresetToCharacter(Character character)
    {
        AddStatisticsToCharacter(character);
        AddAbilitiesToCharacter(character);
    }


    //For Statistics: 
    // - If the statistic of the preset already exists, do not add it
    //
    // - If the statistic of the preset does not exist, add it to the stats
    //   of the character.
    void AddStatisticsToCharacter(Character character)
    {
        if (stats.GetStatistics() != null)
        {
            foreach (string statName in this.stats.GetStatistics().Keys)
            {
                //Add the stat if it isno already contained in the dictionary.
                if(!character.GetStats().GetStatistics().ContainsKey(statName))
                   character.AddStat(stats.GetStatistics()[statName]);
            }
                
        }
    }

    public void SetStats(CharacterStats s){ stats = s; }
    public void SetName(string name) { this.name = name; }

    public void SetAbilities(string[] abilities)
    {
        foreach(string ability in abilities)
        {
            presetAbilities.Add(ability);
        }
    }

    public void AddAbility(string ability)
    {
        presetAbilities.Add(ability);
    }

    public string ToString(string prevTab)
    {
        string tab = "  ";
        string currTab = tab + prevTab;
        string s = prevTab + "Preset " + name + "\n";
        s += stats.ToString(currTab) + "\n";
        s += prevTab + "Abilities: [  ";
        foreach(string ability in presetAbilities)
        {
            s += ability + "  |  ";
        }
        s += "]";
        return s;
    }
}

public class AdditionalCharacterPreset : CharacterPreset
{
    public new void AddPresetToCharacter(Character character)
    {
        AddStatisticsToCharacter(character);
        AddAbilitiesToCharacter(character);
    }

    //For Statistics: 
    // - If the statistic of the preset already exists,
    //   just add its static value to the existing stat as a permanent effect.
    //
    // - If the statistic of the preset does not exist, add it to the stats
    //   of the character.
    void AddStatisticsToCharacter(Character character)
    {
        foreach(CharacterStat stat in this.stats.GetStatistics().Values)
        {
            if(character.GetStat_NotExists(name) == null)
            {
                character.AddStat(stat);
            }
            else
            {
                character.GetStats().AddPermanentEffect(
                    stat.GetName(),
                    this.name,
                    stat.GetStaticValue()
                );
            }

        }
    }
}