using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class CharacterStat{
    string name;
    //A stat is given by the formula:
    //currentValue = staticValue + statRelations + permanentEffects + currentEffect 
    int staticValue = 0;
    Dictionary<string, CharacterStat> statRelations;
    Dictionary<string, int> permanentEffects;
    Dictionary<string, int> temporalEffects;

    int currentValue;

    //Stat Relations:
    //  -a stat can be affected by the value of another stat
    void AddStatRelation(CharacterStat stat)
    {
        statRelations.Add(stat.GetName(), stat);
    }

    int CalculateStatRelations()
    {
        int sum = 0;
        if(statRelations != null)
        {
            foreach(string statName in statRelations.Keys)
            {
                sum += statRelations[statName].GetCurrentValue();
            }
        }
        return sum;
    }

    public bool HasStatRelationWith(string statName)
    {
        return statRelations.ContainsKey(statName);
    }

    //Permanent Effects:
    //  -Cannot be removed
    //  -Come from rules such as class features, feats, stat increases
    void AddPermanentEffect(string name, int value)
    {
        permanentEffects.Add(name, value);
        currentValue += permanentEffects[name];
    }
    int CalculatePermanentEffects()
    {
        int sum = 0;
        if (permanentEffects != null)
        {
            foreach (string effectName in permanentEffects.Keys)
            {
                currentValue += permanentEffects[effectName];
            }
        }
        return sum;
    }

    //Temporal Effects:
    //  -Represent abilities or other effects that affect a specific statistic.
    //  -Each effect has a duration and can be removed.
    
    int CalculateTemporalEffects()
    {
        int sum = 0;
        if(temporalEffects != null)
        {
            foreach (string effectName in temporalEffects.Keys)
            {
                currentValue += temporalEffects[effectName];
            }
        }
        return sum;
    }
    public void AddTemporalEffect(string effectName, int value)
    {
        temporalEffects.Add(effectName, value);
        currentValue += temporalEffects[effectName];   
    }

    public void RemoveTemporalEffect(string effectName)
    {
        currentValue -= temporalEffects[effectName];
        temporalEffects.Remove(effectName);
    }

    public void CalculateStatValue()
    {
        currentValue = staticValue + CalculatePermanentEffects() + CalculateTemporalEffects();
    }

    public int GetCurrentValue()
    {
        return currentValue;
    }

    public string GetName()
    {
        return name;
    }
}

class CharacterStatistics
{
    Dictionary<string, CharacterStat> statistics = new Dictionary<string, CharacterStat>();

    void RecalculateStatsAfterChange(string statName)
    {
        foreach (string name in statistics.Keys)
        {
            if (statistics[name].HasStatRelationWith(statName))
            {
                statistics[name].CalculateStatValue();
                RecalculateStatsAfterChange(name);
            }
        }
    }

    void AddTemporalEffect(string statName, string effectName, int value)
    {
        statistics[statName].AddTemporalEffect(effectName, value);
        RecalculateStatsAfterChange(statName);
    }   
}
public class Character
{
    List<(string actionTypeName, int numPerTurn)> turnEconomy;
    Dictionary<string, Ability> abilities;
    Dictionary<string, CharacterStat> statistics = new Dictionary<string, CharacterStat>();
}
