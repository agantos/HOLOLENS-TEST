using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPreset
{
    protected string name;
    protected Dictionary<string, Ability> presetAbilities;

    void AddPresetToCharacter(Character character)
    {
        AddStatisticsToCharacter(character);
        AddAbilitiesToCharacter(character);
    }

    void AddStatisticsToCharacter(Character character) {}

    void AddAbilitiesToCharacter(Character character){}

}

//PreliminaryCharacterPresets
//  add their statistics to the character statitstics
//  add their abilities to the character abilities
public class BaseCharacterPreset : CharacterPreset
{
    public CharacterStatistics stats;
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

    public override string ToString()
    {
        string s = "Preset " + this.name + "\n";
        s += stats.ToString();

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