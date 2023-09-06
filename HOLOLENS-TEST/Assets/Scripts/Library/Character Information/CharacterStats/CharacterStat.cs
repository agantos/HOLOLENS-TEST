using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CharacterStat
{
    string name;
    //A stat is given by the formula:
    //currentValue = staticValue + statRelations + permanentEffects + currentEffect - currentDamage 
    int staticValue = 0;
    int damage = 0;

    Dictionary<string, CharacterStatRelation> statRelations;
    Dictionary<string, int> permanentEffects;
    Dictionary<string, StatTemporalEffect> temporalEffects;

    int currentValue;
    int maxValue;

    public CharacterStat()
    {
        statRelations = new Dictionary<string, CharacterStatRelation>();
        permanentEffects = new Dictionary<string, int>();
        temporalEffects = new Dictionary<string, StatTemporalEffect>();
    }

    public CharacterStat(string name, int staticValue)
    {
        this.name = name;
        this.staticValue = staticValue;

        statRelations = new Dictionary<string, CharacterStatRelation>();
        permanentEffects = new Dictionary<string, int>();
        temporalEffects = new Dictionary<string, StatTemporalEffect>();
    }
    //Clone Stat;
    public CharacterStat Clone()
    {
        CharacterStat clone = new CharacterStat
        {
            name = this.name,
            staticValue = this.staticValue,
            damage = this.damage,
            currentValue = this.currentValue
        };

        //Stat relation are shared between different character stats.
        clone.statRelations = new Dictionary<string, CharacterStatRelation>(this.statRelations);
        clone.permanentEffects = new Dictionary<string, int>(this.permanentEffects);
        clone.temporalEffects = new Dictionary<string, StatTemporalEffect>(this.temporalEffects);

        return clone;
    }


    //Stat Damage
    //  -damage is a seperate counter that gets added to the stats current value
    public void DealDamage(int damage)
    {
        this.damage += damage;
    }

    public void HealDamage(int damage)
    {
        this.damage -= damage;
        if (damage < 0) damage = 0;
    }

    //Stat Relations:
    //  -a stat can be affected by the value of another stat
    public void AddStatRelation(CharacterStat stat, StatFunctor fun)
    {
        statRelations.Add(stat.name, new CharacterStatRelation(stat, fun));
    }

    int CalculateStatRelations()
    {
        int sum = 0;
        if (statRelations != null && statRelations.Count > 0)
        {
            foreach (CharacterStatRelation relation in statRelations.Values)
            {
                sum += relation.CalculateRelation();
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
    public void AddPermanentEffect(string name, int value)
    {
        permanentEffects.Add(name, value);
        currentValue += permanentEffects[name];
    }
    int CalculatePermanentEffects()
    {
        int sum = 0;
        if (permanentEffects != null && permanentEffects.Count > 0)
        {
            foreach (string effectName in permanentEffects.Keys)
            {
                currentValue += permanentEffects[effectName];
            }
        }
        return sum;
    }
    private int AssertandGetPermanentEffect(string name)
    {
        int value;
        if (permanentEffects.TryGetValue(name, out value))
            return value;
        else
        {
            Assert.IsTrue(false);
            return 0;
        }
    }

    //Temporal Effects:
    //  -Represent abilities or other effects that affect a specific statistic.
    //  -Each effect has a duration and can be removed.
    int CalculateTemporalEffects()
    {
        int sum = 0;
        if (temporalEffects != null && temporalEffects.Count > 0)
        {
            foreach (string effectName in temporalEffects.Keys)
            {
                sum += temporalEffects[effectName].value;
            }
        }
        return sum;
    }
    public void AddTemporalEffect(string effectName, int duration, int value)
    {
        temporalEffects.Add(effectName, new StatTemporalEffect(effectName, value, duration));
    }

    public void RemoveTemporalEffect(string effectName)
    {
        temporalEffects.Remove(effectName);
    }

    //Moves the duration of all temporal effects in a stat.
    public void UpdateTemporalEffects(out bool noTemporalEffects)
    {
        bool shouldRecalculate = false;
        List<string> to_remove = new List<string>();

        foreach (StatTemporalEffect temp in temporalEffects.Values)
        {
            //Advance duration and add effects for removal 
            temp.duration -= 1;
            if (temp.duration == 0)
            {
                to_remove.Add(temp.name);
                shouldRecalculate = true;
            }
        }

        //Remove expired effects
        foreach (string effectName in to_remove)
            temporalEffects.Remove(effectName);

        //Recalculate stat if needed
        if (shouldRecalculate)
        {
            CalculateCurrentValue();
        }

        noTemporalEffects = temporalEffects.Count == 0;
    }

    private StatTemporalEffect AssertandGetTemporalEffect(string name)
    {
        StatTemporalEffect temp;
        if (temporalEffects.TryGetValue(name, out temp))
            return temp;
        else
        {
            Assert.IsTrue(false);
            return null;
        }
    }

    public void CalculateCurrentValue()
    {
        currentValue = staticValue + CalculateStatRelations() + CalculatePermanentEffects() + CalculateTemporalEffects() - damage;
        CalculateMaxValue();
    }

    public void CalculateMaxValue()
    {
        maxValue = staticValue + CalculateStatRelations() + CalculatePermanentEffects() + CalculateTemporalEffects();
    }

    //Getters
    public int GetCurrentValue()
    {
        return currentValue;
    }

    public int GetMaxValue()
    {
        return maxValue;
    }

    public string GetName()
    {
        return name;
    }

    public int GetDamage()
    {
        return damage;
    }

    public void AddToStaticValue(int val)
    {
        staticValue += val;
    }

    public string ToString(string prevTab)
    {
        string tab = "  ";
        string currTab = tab + prevTab;

        string s = currTab + name + ": {\n";

        //Static Value
        s += currTab + tab + "staticValue: " + staticValue + "\n";

        //Stat Relations
        s += currTab + tab + "statRelations: [ ";
        if (statRelations.Count != 0)
        {
            foreach (string name in statRelations.Keys)
            {
                s += "  '";
                s += statRelations[name].ToString() + name;
                s += "'";
            }
        }
        s += " ]\n";

        //Current Value
        s += currTab + tab + "currentValue: " + currentValue + "\n";

        s += currTab + "}\n";
        return s;
    }
}
