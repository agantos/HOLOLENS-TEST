using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JSONTurnEconomy
{
    public string name;
    public int number;
}

[System.Serializable]
public class JSONCharacter
{
    public string name;
    public int player;
    public JSONTurnEconomy[] turnEconomy;
    public string[] abilities;
    public string[] basePresets;
    public string[] additionalPresets;
}

[System.Serializable]
public class JSONCharacters
{
    public JSONCharacter[] characters;
}