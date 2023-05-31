using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JSONAbility
{
    public string name;
    public string description;
    public JSONEffect[] effects;
}

public class JSONAbilities
{
    public JSONAbility[] abilities;
}
