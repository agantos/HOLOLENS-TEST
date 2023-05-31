using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPreset
{
    protected string name;
    protected List<string> presetAbilities = new List<string>();

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
            character.abilities.Add(ability);
        }
    }

}

//BaseCharacterPreset
//  add their statistics to the character statitstics
//  add their abilities to the character abilities
public class BaseCharacterPreset : CharacterPreset
{
    public CharacterStatistics stats = new CharacterStatistics();
    public new void AddPresetToCharacter(Character character)
    {
        AddStatisticsToCharacter(character);
        AddAbilitiesToCharacter(character);
    }

    void AddStatisticsToCharacter(Character character)
    {
        if (stats.GetStatistics() != null)
        {
            foreach (string statName in stats.GetStatistics().Keys)
                character.AddStat(stats.GetStatistics()[statName]);
        }
    }

    public void SetStats(CharacterStatistics s){ stats = s; }
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

//AdditionalCharacterPresets
//  add the values of their statistics to the existing character statitstics
//  add their abilities to the character abilities
public class AdditionalCharacterPreset : CharacterPreset
{
    Dictionary<string, (string statName, int value)> permanentEffects;
    void AddStatisticsToCharacter(Character character)
    {
        if (permanentEffects != null)
        {
            foreach (string effectName in permanentEffects.Keys)
            {
                character.GetStats().
                    AddPermanentEffect( 
                        permanentEffects[effectName].statName, 
                        effectName, 
                        permanentEffects[effectName].value
                    );
            }
                
        }
    }
}