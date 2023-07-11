using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JSONCost
{
    public string name;
    public int cost;
}

[System.Serializable]
public class JSONAbility
{
    public string name;
    public string description;
    public string animationType;
    public JSONCost[] turnEconomyCost;
    public JSONCost[] statCost;
    public JSONEffect[] effects;
}

public class JSONAbilities
{
    public JSONAbility[] abilities;
}

