using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

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
enum TargetType { SELF, ALLY, ENEMY, CREATURE, CREATURE_NOT_SELF, AREA, TYPED}
enum TargetNumber {NUMBERED, IN_RADIUS}
enum AreaShape { CUBE, CONE, SPHERE, LINE, NONE}
public enum EffectSuccessCondition {AUTOMATIC, ATTACKER_ROLLS, DEFENDER_ROLLS, COMPARISON}

public class TargettingStatistics
{
    TargetNumber numberType;
    TargetType targetType;
    int numberOfTargets;
}

public class AreaOfEffect
{
    AreaShape shape;
    int radius;
    int range;
}

public class ComparisonStat
{
    public string statName;
    public float multiplier;
    public string bonus;
}

public class EffectSucceeds
{
    EffectSuccessCondition onCondition;
    ComparisonStat attackerStat;
    ComparisonStat defenderStat;
    string staticNumberToPass;
    float onSavedMultiplier;

    Dictionary<EffectSuccessCondition, Func<CharacterStatistics, CharacterStatistics, bool>> rollSuccess;

    private void CreateSuccessConditions()
    {
        rollSuccess = new Dictionary<EffectSuccessCondition, Func<CharacterStatistics, CharacterStatistics, bool>>();
        rollSuccess.Add(EffectSuccessCondition.AUTOMATIC, AutomaticSuccess);
        rollSuccess.Add(EffectSuccessCondition.ATTACKER_ROLLS, AttackStatic);
        rollSuccess.Add(EffectSuccessCondition.DEFENDER_ROLLS, DefenseStatic);
        rollSuccess.Add(EffectSuccessCondition.COMPARISON, Comparison);
    }

    public EffectSucceeds() {
        CreateSuccessConditions();
    }

    private bool AutomaticSuccess(CharacterStatistics defenderStats, CharacterStatistics attackerStats)
    {
        return true;
    }

    private bool AttackStatic(CharacterStatistics attackerStats, CharacterStatistics defenderStats)
    {
        int attackRoll = (int)((attackerStats.GetStat(attackerStat.statName).GetCurrentValue() + GameplayCalculatorFunctions.CalculateDiceRoll(attackerStat.statName)) * attackerStat.multiplier);
        int passNumber = GameplayCalculatorFunctions.CalculateDiceRoll(staticNumberToPass);

        return (attackRoll > passNumber); 
        
    }

    private bool DefenseStatic(CharacterStatistics attackerStats, CharacterStatistics defenderStats)
    {
        int defenceRoll = (int)((defenderStats.GetStat(defenderStat.statName).GetCurrentValue() + GameplayCalculatorFunctions.CalculateDiceRoll(defenderStat.statName)) * defenderStat.multiplier);
        int passNumber = GameplayCalculatorFunctions.CalculateDiceRoll(staticNumberToPass);

        return (passNumber > defenceRoll);
    }

    private bool Comparison(CharacterStatistics attackerStats, CharacterStatistics defenderStats)
    {
        int attackRoll = (int)((attackerStats.GetStat(attackerStat.statName).GetCurrentValue() + GameplayCalculatorFunctions.CalculateDiceRoll(attackerStat.statName)) * attackerStat.multiplier);
        int defenceRoll = (int)((defenderStats.GetStat(defenderStat.statName).GetCurrentValue() + GameplayCalculatorFunctions.CalculateDiceRoll(defenderStat.statName)) * defenderStat.multiplier);

        return (attackRoll > defenceRoll);
    }

    public bool GetSuccess(CharacterStatistics defenderStats = null, CharacterStatistics attackerStats = null)
    {
        return rollSuccess[onCondition](defenderStats, attackerStats);
    }

}

public class EffectDamage
{
    public string damagedStatName;
    string baseValue;
    List<string> statsAffecting;

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
}


public class Effect
{
    EffectSucceeds effectSucceeds;
    EffectDamage damage;
    public bool EffectSucceeds(CharacterStatistics defenderStats = null, CharacterStatistics attackerStats = null)
    {
        return effectSucceeds.GetSuccess(defenderStats, attackerStats);
    }

    public void DealDamage(CharacterStatistics defenderStats, CharacterStatistics attackerStats)
    {
        CharacterStat damagedStat = defenderStats.GetStat(damage.damagedStatName);
        damagedStat.DealDamage(damage.GetDamage(attackerStats));
    }
}
public class PrimaryEffect : Effect
{
    TargettingStatistics targetting;
    AreaOfEffect areaOfEffect;
    FollowUpEffect[] followUpEffects;
}
public class FollowUpEffect : Effect
{

}
public class Ability 
{
    public string name;
    public string description;
    public Effect[] effects;
}