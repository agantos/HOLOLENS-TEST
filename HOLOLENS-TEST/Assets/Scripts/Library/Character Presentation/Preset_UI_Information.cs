using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character_UI_Bar
{
    public string statName;
    public string color;
}

[System.Serializable]
public class Character_UI_StatCategory
{
    public string   categoryName;
    public string[] statNames;
}

[System.Serializable]
public class Preset_UI_Information
{
    public string                           preset;
    public Character_UI_Bar[]               bars;
    public string[]                         baseStats;
    public string[]                         keyAbilities;
    public Character_UI_StatCategory[]      statCategories;
    
}

[System.Serializable]
public class Preset_UI_InformationTable
{
    public Preset_UI_Information[] UIInfo;
}

//OPTIMIZATION IDEA:
// Make all character's same Character UI Information to avoid duplicates.
// For example only one Character_UI_Information for all Human Wizards
public class Character_UI_Information
{
    public Dictionary<string, Character_UI_Bar> bars = new Dictionary<string, Character_UI_Bar>();    
    public Dictionary <string,string> baseStats = new Dictionary<string, string>();
    public Dictionary<string, string> keyAbilities = new Dictionary<string, string>();
    public Dictionary<string, Character_UI_StatCategory> statCategories = new Dictionary<string, Character_UI_StatCategory>();

    // Character's unique UI information includes the union of
    // the preset's UI information that describe the character
    public void AddPresetInformation(Preset_UI_Information characterInformationUI)
    {
        AddBars(characterInformationUI.bars);
        AddBaseStats(characterInformationUI.baseStats);
        AddKeyAbilities(characterInformationUI.keyAbilities);
        AddStatCategory(characterInformationUI.statCategories);
    }

    void AddKeyAbilities(string[] abilities)
    {
        foreach(string ability in abilities)
        {
            if (!keyAbilities.ContainsKey(ability))
            {
                keyAbilities.Add(ability, ability);
            }
        }
    }

    void AddBaseStats(string[] stats)
    {
        foreach (string stat in stats)
        {
            if (!baseStats.ContainsKey(stat))
            {
                baseStats.Add(stat,stat);
            }
        }
    }

    void AddBars(Character_UI_Bar[] bars)
    {        
        foreach (Character_UI_Bar bar in bars)
        {
            if (!this.bars.ContainsKey(bar.statName))
            {
                this.bars.Add(bar.statName, bar);
            }
        }
    }

    void AddStatCategory(Character_UI_StatCategory[] statCategories)
    {
        foreach (Character_UI_StatCategory statCategory in statCategories)
        {
            if (!this.statCategories.ContainsKey(statCategory.categoryName))
            {
                this.statCategories.Add(statCategory.categoryName, statCategory);
            }
        }
    }
}
