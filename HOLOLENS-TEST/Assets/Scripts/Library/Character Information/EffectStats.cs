using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using UnityEngine;
using UnityEngine.Assertions;

public enum TargetType { SELF, ALLY, ENEMY, ALL, ALL_NOT_SELF, AREA, TYPED }
enum TargetNumber {NUMBERED, IN_RADIUS}
public enum AreaShape { CUBE, CONE, SPHERE, LINE, SELECT, CIRCLE }
public enum EffectSuccessCondition {AUTOMATIC, ATTACKER_ROLLS, DEFENDER_ROLLS, COMPARISON}

public enum EffectType { DAMAGE, HEALING, TEMPORAL }

public class TargettingStats
{
    TargetNumber numberType;
    public TargetType targetType;
    public int numberOfTargets;

    public TargettingStats(string type, int targets)
    {
        numberOfTargets = targets;
        switch (type)
        {
            case "self":
                this.targetType = TargetType.SELF;
                break;
            case "ally":
                this.targetType = TargetType.ALLY;
                break;
            case "enemy":
                this.targetType = TargetType.ENEMY;
                break;
            case "all":
                this.targetType = TargetType.ALL;
                break;
            case "all_not_self":
                this.targetType = TargetType.ALL_NOT_SELF;
                break;
            case "area":
                this.targetType = TargetType.AREA;
                break;
            case "typed":
                this.targetType = TargetType.TYPED;
                break;
            default:
                Assert.IsTrue(false);
                break;
        }
    }

    public string ToString(string prevTab)
    {
        string tab = "  ";
        string currTab = tab + prevTab;
        string s = prevTab + "targetting" + ": { \n";
        s += currTab + "number: " + numberOfTargets.ToString() + "\n";
        s += currTab + "type: " + targetType.ToString() + "\n";
        s += prevTab + "}\n";

        return s;
    }
}

public class AreaOfEffectStats
{
    public AreaShape shape { get; }
    public int radius { get; }
    public int range { get; }

    public AreaOfEffectStats(int range, string shape, int radius)
    {
        this.range = range;
        this.radius = radius;

        switch (shape)
        {
            case "cube":
                this.shape = AreaShape.CUBE;
                break;
            case "cone":
                this.shape = AreaShape.CONE;
                break;
            case "sphere":
                this.shape = AreaShape.SPHERE;
                break;
            case "line":
                this.shape = AreaShape.LINE;
                break;
            case "select":
                this.shape = AreaShape.SELECT;
                break;
            case "circle":
                this.shape = AreaShape.CIRCLE;
                break;
            default:
                Assert.IsTrue(false, "The shape of the AOE is not correct");
                break;
        }
    }

    public string ToString(string prevTab)
    {
        string tab = "  ";
        string currTab = tab + prevTab;
        string s = prevTab + "areaOfEffect" + ": { \n";
        s += currTab + "range: " + range.ToString() + "\n";
        s += currTab + "shape: " + shape.ToString() + "\n";
        s += currTab + "radius: " + radius.ToString() + "\n";
        s += prevTab + "}\n";

        return s;
    }
}

public class ComparisonStat
{
    public string statName;
    public float multiplier;
    public string bonus;

    public ComparisonStat(string name, float multiplier, string bonus)
    {
        statName = name;
        this.multiplier = multiplier;
        this.bonus = bonus;
    }

    public string ToString(string prevTab, string comparedName)
    {
        string tab = "  ";
        string currTab = tab + prevTab;
        string s = currTab + comparedName + ": { \n";
        s += tab + currTab + "name: " + statName + "\n";
        s += tab + currTab + "name: " + multiplier.ToString() + "\n";
        s += tab + currTab + "name: " + bonus + "\n";
        s += currTab + "}\n";

        return s;
    }
}
public class EffectSucceedsStats
{
    public EffectSuccessCondition onCondition;
    public ComparisonStat attackerStat;
    public ComparisonStat defenderStat;
    public string staticNumberToPass;

    public EffectSucceedsStats(string type, string againstStatic, ComparisonStat attackerStat, ComparisonStat defenderStat)
    {
        this.attackerStat = attackerStat;
        this.defenderStat = defenderStat;
        this.staticNumberToPass = againstStatic;
        switch (type)
        {
            case "automatic":
                this.onCondition = EffectSuccessCondition.AUTOMATIC;
                break;
            case "attacker_rolls":
                this.onCondition = EffectSuccessCondition.ATTACKER_ROLLS;
                break;
            case "defender_rolls":
                this.onCondition = EffectSuccessCondition.DEFENDER_ROLLS;
                break;
            case "comparison":
                this.onCondition = EffectSuccessCondition.COMPARISON;
                break;
            default:
                Assert.IsTrue(false);
                break;
        }
    }

    public string ToString(string prevTab)
    {
        string tab = "  ";
        string currTab = tab + prevTab;
        string s = prevTab + "succeedsOn: { \n";
        
        s += currTab + "type: " + onCondition.ToString() + "\n";
        if(staticNumberToPass != null)
            s += currTab + "againstStatic: " + staticNumberToPass.ToString() + "\n";
        
        s += currTab + "comparisonStats: {\n";

        if(defenderStat != null)
            s += defenderStat.ToString(currTab, "defenderStat");

        if(attackerStat != null)
            s += attackerStat.ToString(currTab, "attackerStat");

        s += currTab + "}\n";
        s += prevTab + "}\n";
        return s;
    }
}

public class EffectDamageStats
{
    public string damagedStatName;
    public string baseValue;
    public List<string> statsAffecting;
    public float onSavedMultiplier;
    public bool isNegative = false;

    public EffectDamageStats(string baseAmount, string[] statsAffecting, string damagedStat, float onSavedMultiplier)
    {
        //If the first character of the baseAmount of the stat is negative then all the damage it deals is negative
        if (baseAmount[0] == '-')
        {
            isNegative = true;
            baseAmount = baseAmount.Substring(1);
        }            

        baseValue = baseAmount;
        damagedStatName = damagedStat;

        this.statsAffecting = new List<string>();
        this.onSavedMultiplier = onSavedMultiplier;

        foreach(string stat in statsAffecting)
        {
            this.statsAffecting.Add(stat);
        }
    }

    public int RollDamage(CharacterStats attackerStats = null)
    {
        int sum = 0;
        
        //Calculate dice roll
        int diceRoll = GameplayCalculatorFunctions.CalculateDiceRoll(baseValue);
        sum += diceRoll;

        //Add stat bonuses
        sum += GetStatBonusSum(attackerStats);

        //Return result
        return isNegative? -sum : sum;
    }

    public int GetStatBonusSum(CharacterStats attackerStats)
    {
        int sum = 0;

        if (attackerStats == null)
            return sum;

        List<int> statBonuses = GetStatBonuses(attackerStats);

        foreach(int bonus in statBonuses)
        {
            sum += bonus;
        }

        return sum;
    }

    public List<int> GetStatBonuses(CharacterStats attackerStats)
    {
        List<int> statBonuses = new List<int>();
        
        for(int i = 0; i < statsAffecting.Count; i++)
        {
            if (attackerStats.GetStat(statsAffecting[i]) != null)
            {
                statBonuses.Add(attackerStats.GetStat(statsAffecting[i]).GetCurrentValue());
            }
        }

        return statBonuses;
    }

    public int GetBaseDamage(CharacterStats attackerStats)
    {
        return RollDamage(attackerStats);
    }
    public string ToString(string prevTab)
    {
        string tab = "  ";
        string currTab = tab + prevTab;
        string s = prevTab + "damage" + ": { \n";
        
        s += currTab + "damagedStat: " + damagedStatName + "\n";
        s += currTab + "amount: {" + "\n";

        s += currTab + tab + "baseAmount: " + baseValue + "\n";
        s += currTab + tab + "statsAffecting: [";
        foreach(string stat in statsAffecting)
        {
            s += " " + stat;
        }
        s += " ]\n";

        s += currTab + "}\n";


        s += prevTab + "}\n";

        return s;
    }
}

public class EffectStats
{
    public EffectSucceedsStats effectSucceedsStats;
    public EffectDamageStats damageStats;
    public EffectType type;
    public int duration = 0;

    public bool EffectSucceeds(Character defender = null, Character attacker = null)
    {
        return EffectSucceedsChecker.GetSuccess(this, attacker, defender);
    }

    public void CreateEffectSucceedsStats(string type, string againstStatic, ComparisonStat attackerStat, ComparisonStat defenderStat)
    {
        effectSucceedsStats = new EffectSucceedsStats(type, againstStatic, attackerStat, defenderStat);
    }

    public void CreateDamageStatistics(string baseAmount, string[] statsAffecting, string damagedStat, float onSavedMultiplier)
    {
        damageStats = new EffectDamageStats(baseAmount, statsAffecting, damagedStat, onSavedMultiplier);
    }

    public EffectApplicationData CalculateApplicationData(bool effectSucceeds, Character defender, Character attacker, string abilityName = "")
    {
        CharacterStat affectedStat = defender.GetStats().GetStat(damageStats.damagedStatName);
        int damage = CalculateEffectDamage(effectSucceeds, type, defender, attacker, duration);

        return new EffectApplicationData(type, duration, abilityName, 
                                                                  damage, affectedStat.GetName(), effectSucceeds, 
                                                                  attacker.name, defender.name);
    }

    private int CalculateEffectDamage(bool effectSucceeds, EffectType type, Character defender = null, Character attacker = null, int duration = 0)
    {
        float damage = damageStats.GetBaseDamage(attacker.GetStats());

        //if an effect is saved get the recalculated damage
        if (!effectSucceeds)
            damage *= damageStats.onSavedMultiplier;

        //Include other damage multipliers such as critical, resistance, vulnerabilities etc.
        
        return (int)damage;
    }

    public string ToString(string prevTab)
    {
        string tab = "  ";
        string currTab = tab + prevTab;
        string s = prevTab + "EFFECT { \n";
        
        s += effectSucceedsStats.ToString(currTab);
        s += damageStats.ToString(currTab);
        s += prevTab + "}\n";

        return s;
    }
}

//Effects that will require selecting targets
public class PrimaryEffectStats : EffectStats
{
    public TargettingStats targetting;
    public AreaOfEffectStats areaOfEffect;
    public List<FollowupEffectStats> followUpEffects = new List<FollowupEffectStats>();

    public void CreateTargetting(string type, int number)
    {
        targetting = new TargettingStats(type, number);
    }

    public void CreateAreaOfEffect(int range, string shape, int radius)
    {
        areaOfEffect = new AreaOfEffectStats(range, shape, radius);
    }

    public new string ToString(string prevTab)
    {
        string tab = "  ";
        string currTab = tab + prevTab;
        string s = prevTab + "PRIMARY EFFECT { \n";

        s += targetting.ToString(currTab);
        s += areaOfEffect.ToString(currTab);
        s += effectSucceedsStats.ToString(currTab);
        s += damageStats.ToString(currTab);

        foreach(FollowupEffectStats stat in followUpEffects)
        {
            s += stat.ToString(currTab);
        }

        s += prevTab + "}\n";
        return s;
    }
}

//Effects that happen after a primary effect to the same targets
public class FollowupEffectStats : EffectStats
{
    public bool appliesIfPrimaryFailed;
}

//Singleton that is responsible for calculating whether an effect suceeds or not.
public class EffectSucceedsChecker
{
    static Dictionary<EffectSuccessCondition, Func<EffectStats, Character, Character, bool>> rollSuccess;
    private static EffectSucceedsChecker instance = null;

    private EffectSucceedsChecker()
    {
        CreateSuccessConditions();
    }

    public static EffectSucceedsChecker GetInstance()
    {
        if (instance == null)
        {
            instance = new EffectSucceedsChecker();
        }
        return instance;
    }

    private void CreateSuccessConditions()
    {
        rollSuccess = new Dictionary<EffectSuccessCondition, Func<EffectStats, Character, Character, bool>>();
        rollSuccess.Add(EffectSuccessCondition.AUTOMATIC, AutomaticSuccess);
        rollSuccess.Add(EffectSuccessCondition.ATTACKER_ROLLS, AttackStatic);
        rollSuccess.Add(EffectSuccessCondition.DEFENDER_ROLLS, DefenseStatic);
        rollSuccess.Add(EffectSuccessCondition.COMPARISON, Comparison);
    }

    private bool AutomaticSuccess(EffectStats effectSucceedsStats, Character defenderStats, Character attackerStats)
    {
        return true;
    }

    private bool AttackStatic(EffectStats effect, Character attacker, Character defender)
    {
        int attackerDiceRoll = GameplayCalculatorFunctions.CalculateDiceRoll(effect.effectSucceedsStats.attackerStat.bonus);
        int statBonusAttacker = attacker.GetStats().GetStat(effect.effectSucceedsStats.attackerStat.statName).GetCurrentValue();

        int attackRoll = (int)((statBonusAttacker + attackerDiceRoll) * effect.effectSucceedsStats.attackerStat.multiplier);
        int passNumber = GameplayCalculatorFunctions.CalculateDiceRoll(effect.effectSucceedsStats.staticNumberToPass);

        Logger.Instance.Log_Effect(effect, attacker, attackerDiceRoll, true);

        return (attackRoll > passNumber);
    }

    private bool DefenseStatic(EffectStats effect, Character attacker, Character defender)
    {
        int defenderDiceRoll = GameplayCalculatorFunctions.CalculateDiceRoll(effect.effectSucceedsStats.defenderStat.bonus);
        int statBonusDefender = defender.GetStats().GetStat(effect.effectSucceedsStats.defenderStat.statName).GetCurrentValue();

        int defenceRoll = (int)((statBonusDefender + defenderDiceRoll) * effect.effectSucceedsStats.defenderStat.multiplier);
        
        int passNumber = GameplayCalculatorFunctions.CalculateDiceRoll(effect.effectSucceedsStats.staticNumberToPass);

        Logger.Instance.Log_Effect(effect, defender, defenderDiceRoll, false);

        return (passNumber > defenceRoll);
    }

    private bool Comparison(EffectStats effect, Character attacker, Character defender)
    {
        int attackerDiceRollResult = GameplayCalculatorFunctions.CalculateDiceRoll(effect.effectSucceedsStats.attackerStat.bonus);
        int defenderDiceRollResult = GameplayCalculatorFunctions.CalculateDiceRoll(effect.effectSucceedsStats.defenderStat.bonus);

        int statBonusAttacker = attacker.GetStats().GetStat(effect.effectSucceedsStats.attackerStat.statName).GetCurrentValue();
        int statBonusDefender = defender.GetStats().GetStat(effect.effectSucceedsStats.defenderStat.statName).GetCurrentValue();

        int attackRoll = (int)((statBonusAttacker + attackerDiceRollResult) * effect.effectSucceedsStats.attackerStat.multiplier);
        int defenceRoll = (int)((statBonusDefender + defenderDiceRollResult) * effect.effectSucceedsStats.defenderStat.multiplier);

        Logger.Instance.Log_EffectComparison(effect, attacker, defender, attackerDiceRollResult, defenderDiceRollResult);

        return (attackRoll > defenceRoll);
    }

    public static bool GetSuccess(EffectStats effectSucceedsStats, Character defender = null, Character attacker = null)
    {
        return rollSuccess[effectSucceedsStats.effectSucceedsStats.onCondition](effectSucceedsStats, attacker, defender);
    }
}