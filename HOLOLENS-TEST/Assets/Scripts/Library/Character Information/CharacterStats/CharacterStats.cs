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
        bool tempEffectsWithSameNameStack = true;

        if (tempEffectsWithSameNameStack)
        {
            //apply the effect with a different name
            if (statistics[statName].EffectAlreadyExists(effectName))
            {
                effectName = effectName + "_";
                AddTemporalEffect(statName, effectName, duration, value);
                return;
            }

            //Apply Effect
            statistics[statName].AddTemporalEffect(effectName, duration, value);
            statsWithTemporalEffects.Add(statName);

            statistics[statName].CalculateCurrentValue();
            RecalculateStatsAfterChange(statName);
        }
        //Do not apply effect
        else
        {
            //refresh the effect's duration
            if (statistics[statName].EffectAlreadyExists(effectName))
            {
                statistics[statName].SetTemporalEffectDuration(effectName, duration);
                return;
            }

            //Apply Effect
            statistics[statName].AddTemporalEffect(effectName, duration, value);
            statsWithTemporalEffects.Add(statName);

            statistics[statName].CalculateCurrentValue();
            RecalculateStatsAfterChange(statName);
        }

    }

    //O(n) n === the stats with temporal effects
    public void UpdateTemporalEffects()
    {
        List<string> to_remove = new List<string>();

        //Move the timer of all the Temporal Effects and find which to remove
        foreach(string statName in statsWithTemporalEffects)
        {
            bool noTemporalEffects;
            statistics[statName].UpdateTemporalEffects(out noTemporalEffects);           

            if (noTemporalEffects)
                to_remove.Add(statName);
        }

        //Remove the Temporal Effects that expired and recalculate all stats with relations to them.
        foreach(string stat in to_remove)
        {
            statsWithTemporalEffects.Remove(stat);
            RecalculateStatsAfterChange(stat);
        }
            


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

    public void AddStatRelation(Character c, string affectedStatName, string affectorStatName, StatFunctor fun)
    {
        if (GetStat(affectorStatName) != null)
            statistics[affectedStatName].AddStatRelation(c, affectorStatName, fun);
        else
            Assert.IsTrue(false, "The stat you are trying to relate to does not exist.");
    }

    public void SetCharacterInCharacterRelations(Character c)
    {
        foreach(CharacterStat stat in statistics.Values)
        {
            stat.SetCharacterInStatRelations(c);
        }
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
        if (statistics.ContainsKey(name))
            return statistics[name];
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

