using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JSONEffectTargetting
{
    public int number;
    public string type;
}

[System.Serializable]
public class JSONEffectAreaOfEffect
{
    public int range;
    public string shape;
    public int radius;
}

[System.Serializable]
public class JSONEffectComparisonStat
{
    public string name;
    public float multiplier;
    public string bonus;
}

[System.Serializable]
public class JSONEffectComparisonStats
{
    public JSONEffectComparisonStat defenderStat;
    public JSONEffectComparisonStat attackerStat;
}

[System.Serializable]
public class JSONEffectSucceedsOn
{
    public string type;
    public string againstStatic;
    public float onSavedMultiplier;
    public JSONEffectComparisonStats comparisonStats;
}

[System.Serializable]
public class JSONEffectDamageAmount
{
    public string baseAmount;
    public string[] statsAffecting;
}

[System.Serializable]
public class JSONEffectDamage
{
    public string damagedStat;
    public JSONEffectDamageAmount amount;
    public float onSavedMultiplier;
}

[System.Serializable]
public class JSONEffect
{
    public bool isPrimary;
    public bool appliesIfPrimaryFailed;
    public JSONEffectTargetting targetting;
    public JSONEffectAreaOfEffect areaOfEffect;
    public JSONEffectSucceedsOn succeedsOn;
    public JSONEffectDamage damage;
}