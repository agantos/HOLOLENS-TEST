using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using UnityEngine.Assertions;

public class GameplayCalculatorFunctions
{
    public static int CalculateDiceRoll(string value)
    {
        Random roll = new Random();
        int sum = 0;
        int diceNumber, diceSides, staticValue;
        ParseDiceString(value, out diceNumber, out diceSides, out staticValue);        

        for(int i = 0; i < diceNumber; i++)
        {
            sum += roll.Next(1, diceSides);
        }
        sum += staticValue;
        return sum;
    }
    public static void ParseDiceString(string input, out int diceNumber, out int diceSides, out int staticValue)
    {
        // Initialize the output values to 0
        diceNumber = 0;
        diceSides = 0;
        staticValue = 0;

        // Define the regular expression pattern
        string pattern = @"(\d+)d(\d+)(?:\s*\+\s*(\d+))?";

        // Match the pattern against the input string
        Match match = Regex.Match(input, pattern);

        // Extract the values of x, y, and z if they are present
        if (match.Success)
        {
            // Extract x and y
            diceNumber = int.Parse(match.Groups[1].Value);
            diceSides = int.Parse(match.Groups[2].Value);

            // Extract z if it is present
            if (match.Groups.Count > 3 && !string.IsNullOrEmpty(match.Groups[3].Value))
            {
                staticValue = int.Parse(match.Groups[3].Value);
            }
        }
    }
}

enum EffectType { INDEPENDENT, DEPENDENT}
enum TargetType { SELF, ALLY, ENEMY, ALL, ALL_NOT_SELF, AREA, TYPED}
enum TargetNumber {NUMBERED, IN_RADIUS}
enum AreaShape { CUBE, CONE, SPHERE, LINE, SELECT }
public enum EffectSuccessCondition {AUTOMATIC, ATTACKER_ROLLS, DEFENDER_ROLLS, COMPARISON}

public class TargettingStats
{
    TargetNumber numberType;
    TargetType targetType;
    int numberOfTargets;

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
    AreaShape shape;
    int radius;
    int range;

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
    public float onSavedMultiplier;

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
    string baseValue;
    List<string> statsAffecting;

    public EffectDamageStats(string baseAmount, string[] statsAffecting, string damagedStat)
    {
        baseValue = baseAmount;
        damagedStatName = damagedStat;
        this.statsAffecting = new List<string>();

        foreach(string stat in statsAffecting)
        {
            this.statsAffecting.Add(stat);
        }
    }

    public int RollDamage(List<int> statBonuses)
    {
        int sum = 0;
        sum += GameplayCalculatorFunctions.CalculateDiceRoll(baseValue);

        foreach(int bonus in statBonuses)
        {
            sum += bonus;
        }

        return sum;
    }

    List<int> GetStatBonuses(CharacterStatistics attackerStats)
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

    public int GetDamage(CharacterStatistics attackerStats)
    {
        return RollDamage(GetStatBonuses(attackerStats));
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
    public EffectDamageStats damage;

    public bool EffectSucceeds(CharacterStatistics defenderStats = null, CharacterStatistics attackerStats = null)
    {
        return EffectSucceedsChecker.GetSuccess(effectSucceedsStats, attackerStats, defenderStats);
    }

    public void CreateEffectSucceedsStats(string type, string againstStatic, ComparisonStat attackerStat, ComparisonStat defenderStat)
    {
        effectSucceedsStats = new EffectSucceedsStats(type, againstStatic, attackerStat, defenderStat);
    }

    public void CreateDamageStatistics(string baseAmount, string[] statsAffecting, string damagedStat)
    {
        damage = new EffectDamageStats(baseAmount, statsAffecting, damagedStat);
    }

    public void DealDamage(CharacterStatistics defenderStats, CharacterStatistics attackerStats)
    {
        CharacterStat damagedStat = defenderStats.GetStat(damage.damagedStatName);
        damagedStat.DealDamage(damage.GetDamage(attackerStats));
    }

    public string ToString(string prevTab)
    {
        string tab = "  ";
        string currTab = tab + prevTab;
        string s = prevTab + "EFFECT { \n";
        
        s += effectSucceedsStats.ToString(currTab);
        s += damage.ToString(currTab);
        s += prevTab + "}\n";

        return s;
    }
}

//Effects that will require selecting targets
public class PrimaryEffectStats : EffectStats
{
    TargettingStats targetting;
    AreaOfEffectStats areaOfEffect;
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
        s += damage.ToString(currTab);

        foreach(FollowupEffectStats stat in followUpEffects)
        {
            s += stat.ToString(currTab);
        }

        s += prevTab + "}\n";
        return s;
    }
}

//Effects that happen after another effect to the same targets as the first
public class FollowupEffectStats : EffectStats
{
    bool appliesIfPrimaryFailed;
}


//Singleton that is responsible for calculating whether an effect suceeds or not.
public class EffectSucceedsChecker
{
    static Dictionary<EffectSuccessCondition, Func<EffectSucceedsStats, CharacterStatistics, CharacterStatistics, bool>> rollSuccess;
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
        rollSuccess = new Dictionary<EffectSuccessCondition, Func<EffectSucceedsStats, CharacterStatistics, CharacterStatistics, bool>>();
        rollSuccess.Add(EffectSuccessCondition.AUTOMATIC, AutomaticSuccess);
        rollSuccess.Add(EffectSuccessCondition.ATTACKER_ROLLS, AttackStatic);
        rollSuccess.Add(EffectSuccessCondition.DEFENDER_ROLLS, DefenseStatic);
        rollSuccess.Add(EffectSuccessCondition.COMPARISON, Comparison);
    }

    private bool AutomaticSuccess(EffectSucceedsStats effectSucceedsStats, CharacterStatistics defenderStats, CharacterStatistics attackerStats)
    {
        return true;
    }

    private bool AttackStatic(EffectSucceedsStats effectSucceedsStats, CharacterStatistics attackerStats, CharacterStatistics defenderStats)
    {
        int attackRoll = (int)((attackerStats.GetStat(effectSucceedsStats.attackerStat.statName).GetCurrentValue() + GameplayCalculatorFunctions.CalculateDiceRoll(effectSucceedsStats.attackerStat.statName)) * effectSucceedsStats.attackerStat.multiplier);
        int passNumber = GameplayCalculatorFunctions.CalculateDiceRoll(effectSucceedsStats.staticNumberToPass);

        return (attackRoll > passNumber);

    }

    private bool DefenseStatic(EffectSucceedsStats effectSucceedsStats, CharacterStatistics attackerStats, CharacterStatistics defenderStats)
    {
        int defenceRoll = (int)((defenderStats.GetStat(effectSucceedsStats.defenderStat.statName).GetCurrentValue() +
            GameplayCalculatorFunctions.CalculateDiceRoll(effectSucceedsStats.defenderStat.statName)) * effectSucceedsStats.defenderStat.multiplier);
        int passNumber = GameplayCalculatorFunctions.CalculateDiceRoll(effectSucceedsStats.staticNumberToPass);

        return (passNumber > defenceRoll);
    }

    private bool Comparison(EffectSucceedsStats effectSucceedsStats, CharacterStatistics attackerStats, CharacterStatistics defenderStats)
    {
        int attackRoll = (int)((attackerStats.GetStat(effectSucceedsStats.attackerStat.statName).GetCurrentValue() + GameplayCalculatorFunctions.CalculateDiceRoll(effectSucceedsStats.attackerStat.statName)) * effectSucceedsStats.attackerStat.multiplier);
        int defenceRoll = (int)((defenderStats.GetStat(effectSucceedsStats.defenderStat.statName).GetCurrentValue() + GameplayCalculatorFunctions.CalculateDiceRoll(effectSucceedsStats.defenderStat.statName)) * effectSucceedsStats.defenderStat.multiplier);

        return (attackRoll > defenceRoll);
    }

    public static bool GetSuccess(EffectSucceedsStats effectSucceedsStats, CharacterStatistics defenderStats = null, CharacterStatistics attackerStats = null)
    {
        return rollSuccess[effectSucceedsStats.onCondition](effectSucceedsStats, defenderStats, attackerStats);
    }

}