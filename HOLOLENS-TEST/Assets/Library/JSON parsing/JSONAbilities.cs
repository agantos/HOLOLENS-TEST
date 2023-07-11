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
public class JSONAbilityAnimationTypes
{
    public string attacker;
    public string defender_AbilitySucceeds;
    public string defender_AbilityFails;

    AnimationType StringToAnimationType(string s)
    {
        AnimationType type;
        if (Enum.TryParse(s, out type) == false)
            Assert.IsFalse(true);

        return type;
    }

    public AnimationType GetAttackerAnimationType()
    {
        return StringToAnimationType(attacker);
    }

    public AnimationType GetDefender_AbilitySucceeds()
    {
        return StringToAnimationType(defender_AbilitySucceeds);
    }

    public AnimationType GetDefender_AbilityFails()
    {
        return StringToAnimationType(defender_AbilityFails);
    }
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
}

public class JSONAbilities
{
    public JSONAbility[] abilities;
}

