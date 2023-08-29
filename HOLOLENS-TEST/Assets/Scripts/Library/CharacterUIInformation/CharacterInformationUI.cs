using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterUIBar
{
    public string statName;
    public string color;
}

[System.Serializable]
public class CharacterUIStatCategory
{
    public string   categoryName;
    public string[] statNames;
}

[System.Serializable]
public class CharacterInformationUI
{
    public string                           preset;
    public CharacterUIBar[]                 bars;
    public string[]                         baseStats;
    public string[]                         keyAbilities;
    public CharacterUIStatCategory[]    statCategories;
    
}

[System.Serializable]
public class CharacterInformationUI_Table
{
    public CharacterInformationUI[] UIInfo;
}
