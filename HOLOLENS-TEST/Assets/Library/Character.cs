using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

//TODO MAKE STAT EFFECTS INTO A CLASS

class StatEffects
{
    Dictionary<string, (string statName, int value)> effects;

    public void AddEffect(string effectName, string statName, int value)
    {
        effects.Add(effectName, (statName, value));
    }
}

public class StatFunctor{
    public string op;
    public float value;

    public StatFunctor(string type, float operand)
    {
        this.op = type;
        this.value = operand;
    }
}

public class CharacterStatRelation
{
    public CharacterStat stat;
    public StatFunctor fun;

    public CharacterStatRelation(CharacterStat stat, StatFunctor fun)
    {
        this.stat = stat;
        this.fun = fun;
    }

    public int CalculateRelation()
    {
        float relationAdd = 0;      
        switch (fun.op)
        {
            case "+":
                relationAdd += fun.value * stat.GetCurrentValue();
                break;
            case "-":
                relationAdd -= fun.value * stat.GetCurrentValue();    
                break;
            default:
                Assert.IsTrue(false, "Invalid Operand");
                break;

        }
        return (int)relationAdd;
    }

    public override string ToString()
    {
        string s = "";
        s += fun.op + " | ";
        s += fun.value.ToString()+" | ";
        return s;
    }
}
public class CharacterStat {
    string name;
    //A stat is given by the formula:
    //currentValue = staticValue + statRelations + permanentEffects + currentEffect - currentDamage 
    int staticValue = 0;
    int damage = 0;
    Dictionary<string, CharacterStatRelation> statRelations;
    Dictionary<string, int> permanentEffects;
    Dictionary<string, int> temporalEffects;

    int currentValue;

    public CharacterStat(string name, int staticValue)
    {
        this.name = name;
        this.staticValue = staticValue;

        statRelations = new Dictionary<string, CharacterStatRelation>();
        permanentEffects = new Dictionary<string, int>();
        temporalEffects = new Dictionary<string, int>();
    }

    public void DealDamage(int damage) { this.damage += damage; }

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
        if (statRelations != null && statRelations.Count > 0 )
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
                currentValue += temporalEffects[effectName];
            }
        }
        return sum;
    }
    public void AddTemporalEffect(string effectName, int value)
    {
        temporalEffects.Add(effectName, value);
        currentValue += AssertandGetTemporalEffect(effectName);
    }

    public void RemoveTemporalEffect(string effectName)
    {
        currentValue -= AssertandGetTemporalEffect(effectName);
        temporalEffects.Remove(effectName);
    }

    private int AssertandGetTemporalEffect(string name)
    {
        int value;
        if (temporalEffects.TryGetValue(name, out value))
            return value;
        else
        {
            Assert.IsTrue(false);
            return 0;
        }
    }

    public void CalculateCurrentValue()
    {
        currentValue = staticValue + CalculateStatRelations()+ CalculatePermanentEffects() + CalculateTemporalEffects();
    }

    public int GetCurrentValue()
    {
        return currentValue;
    }

    public string GetName()
    {
        return name;
    }

    public void AddToStaticValue(int val)
    {
        staticValue += val;
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

    public string ToString(string outerTab)
    {
        string tab = outerTab + "    ";

        string s = outerTab + "";
        s += outerTab + name + ": {";
        s += tab + "\n";
        
        //Static Value
        s += tab + "staticValue: " + staticValue + "\n";
        
        //Stat Relations
        s += tab + "statRelations: [ ";
        if (statRelations.Count != 0)
        {
            foreach(string name in statRelations.Keys)
            {
                s += "  '";
                s += statRelations[name].ToString() + name ;
                s += "'";
            }            
        }
        s += " ]\n";

        //Current Value
        s += tab + "currentValue: " + currentValue + "\n";

        s += outerTab + "}\n";

        return s;
    }
} 
//Formulas are statistics that combine various stats but are universal and many 
//abilities may require their entry. For example, spell bonus, attack bonus, spell DC
class Formula
{
    private int staticValue { get; set; } = 0;
    private string name { get; set; }
    private List<CharacterStat> formulaParts;

    public int CalculateFormula()
    {
        int sum = staticValue;
        Assert.IsFalse(formulaParts == null);
        foreach (CharacterStat formulaPart in formulaParts)
        {
            sum += formulaPart.GetCurrentValue();
        }
        return sum;
    }

    public void AddPart(CharacterStat part)
    {
        formulaParts.Add(part);
    }

    public void SetStaticValue(int value) { staticValue = value; }
}
public class CharacterStatistics
{
    Dictionary<string, CharacterStat> statistics = new Dictionary<string, CharacterStat>();

    void RecalculateStatsAfterChange(string statName)
    {
        foreach (string name in statistics.Keys)
        {
            if (statistics[name].HasStatRelationWith(statName))
            {
                statistics[name].CalculateCurrentValue();
                RecalculateStatsAfterChange(name);
            }
        }
    }

    public void AddTemporalEffect(string statName, string effectName, int value)
    {
        statistics[statName].AddTemporalEffect(effectName, value);
        RecalculateStatsAfterChange(statName);
    }

    public void AddPermanentEffect(string statName, string effectName, int value)
    {
        statistics[statName].AddPermanentEffect(effectName, value);
    }
    public Dictionary<string, CharacterStat> GetStatistics() { return statistics; }

    //Returns a stat with name = name or null if it doesn't exist in statistics
    public CharacterStat GetStat(string name) {
        CharacterStat stat;
        if (statistics.TryGetValue(name, out stat))
            return stat;
        else
            return null;
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
            stat.CalculateCurrentValue();
        }
    }
    public override string ToString()
    {
        string s = "statList: [ \n";

        foreach (CharacterStat stat in statistics.Values)
        {
            s += stat.ToString("        ");
        }
        s += "]\n";
        return s;
    }
}
public class Character
{
    List<(string actionTypeName, int numPerTurn)> turnEconomy;
    Dictionary<string, Ability> abilities;
    CharacterStatistics stats;

    public void AddStat(CharacterStat stat) { stats.GetStatistics().Add(stat.GetName(), stat); }

    public CharacterStat GetCharacterStat(string name){
        CharacterStat stat = stats.GetStat(name);
        Assert.IsNotNull(stat, "No stat with name = " + name +" exists.");
        return stats.GetStat(name);
    }

    public CharacterStatistics GetStats() { return stats; }
}
