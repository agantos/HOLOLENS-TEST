using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JSONCharacter
{
    public string name;
    public string[] abilities;
    public string[] basePresets;
    public string[] additionalPresets;
}

[System.Serializable]
public class JSONCharacters
{
    public JSONCharacter[] characters;
}