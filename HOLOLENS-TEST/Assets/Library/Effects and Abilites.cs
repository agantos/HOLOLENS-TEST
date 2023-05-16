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

enum TargetType { SELF, ALLY, ENEMY, CREATURE, CREATURE_NOT_SELF, AREA, TYPED}
enum TargetNumber {NUMBERED, IN_RADIUS}
enum AreaShape { CUBE, CONE, SPHERE, LINE, NONE}
enum EffectSuccessCondition {AUTOMATIC, COMPARISON}

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
    string statName;
    float multiplier;
    string bonus;
}

public class EffectSucceeds
{
    EffectSuccessCondition onCondition;
    ComparisonStat attackerStat;
    ComparisonStat defenderStat;
    string staticNumberToPass;
    float onSavedMultiplier;


}

public class EffectDamage
{
    string damagedStatName;
    string baseValue;
    List<string> statsAffecting;

    int CalculateDamage(List<int> statBonuses)
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


    //Transport to Effect class
    public void DealDamage(CharacterStatistics targetStats, CharacterStatistics attackerStats)
    {
        targetStats.GetStat(damagedStatName).DealDamage(CalculateDamage(GetStatBonuses(attackerStats)));
    }
}

class Effect
{
    int range;
    TargettingStatistics targetting;
    AreaOfEffect areaOfEffect;
    EffectSucceeds effectSucceeds;
    EffectDamage damage;

}

public class Ability { }