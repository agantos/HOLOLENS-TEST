using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Assertions;

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
    public JSONAbilityAnimationTypes animationTypes;
    public JSONCost[] turnEconomyCost;
    public JSONCost[] statCost;
    public JSONEffect[] effects;
    public string[] tags;
}

public class JSONAbilities
{
    public JSONAbility[] abilities;
}

