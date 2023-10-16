using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
public class CharacterStats
{
    Dictionary<string, CharacterStat> statistics = new Dictionary<string, CharacterStat>();
    private HashSet<string> statsWithTemporalEffects = new HashSet<string>();

    public void AddTemporalEffect(string statName, string effectName, int duration, int value)
    {
        statistics[statName].AddTemporalEffect(effectName, duration, value);
        statsWithTemporalEffects.Add(statName);

        statistics[statName].CalculateCurrentValue();
        RecalculateStatsAfterChange(statName);
    }

    //O(n) n === the stats with temporal effects
    public void UpdateTemporalEffects()
    {
        List<string> to_remove = new List<string>();

        foreach(string statName in statsWithTemporalEffects)
        {
            bool noTemporalEffects;
            statistics[statName].UpdateTemporalEffects(out noTemporalEffects);           

            if (noTemporalEffects)
                to_remove.Add(statName);
        }

        foreach(string stat in to_remove)
            statsWithTemporalEffects.Remove(stat);
    }

    public void RemoveTemporalEffect(string statName, string effectName, int duration, int value)
    {
        statistics[statName].RemoveTemporalEffect(effectName);
        RecalculateStatsAfterChange(statName);
    }

    public void AddPermanentEffect(string statName, string effectName, int value)
    {
        statistics[statName].AddPermanentEffect(effectName, value);
    }
    
    public void AddStat(string name, int staticValue)
    {
        statistics.Add(name, new CharacterStat(name, staticValue));
    }

    public void AddStatRelation(string affectedStatName, string affectorStatName, StatFunctor fun)
    {
        if (GetStat(affectorStatName) != null)
            statistics[affectedStatName].AddStatRelation(GetStat(affectorStatName), fun);
        else
            Assert.IsTrue(false, "The stat you are trying to relate to does not exist.");
    }

    public void CalculateAllStats()
    {
        foreach (CharacterStat stat in statistics.Values)
        {
            stat.CalculateCurrentValue_Pedantically();
        }
    }

    public string ToString(string prevTab)
    {
        string tab = "  ";
        string currTab = tab + prevTab;
        string s = prevTab + "statList: [ \n";

        foreach (CharacterStat stat in statistics.Values)
        {
            s += stat.ToString(currTab);
        }
        s += prevTab + "]";
        return s;
    }

    public Dictionary<string, CharacterStat> GetStatistics() { return statistics; }

    //Returns a stat with name = name or null if it doesn't exist in statistics
    public CharacterStat GetStat(string name)
    {
        CharacterStat stat;
        if (statistics.TryGetValue(name, out stat))
            return stat;
        else
            return null;
    }

    //After a stat change all related stats should be updated
    void RecalculateStatsAfterChange(string statName)
    {
        foreach (string name in statistics.Keys)
        {
            if (statistics[name].HasStatRelationWith(statName))
            {
                statistics[name].CalculateCurrentValue_Pedantically();
                RecalculateStatsAfterChange(name);
            }
        }
    }
}

