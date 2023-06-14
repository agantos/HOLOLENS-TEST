using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class GameplayCalculatorFunctions
{
    public static int CalculateDiceRoll(string value)
    {
        System.Random roll = new System.Random();
        int sum = 0;
        int diceNumber, diceSides, staticValue;
        ParseDiceString(value, out diceNumber, out diceSides, out staticValue);
        for (int i = 0; i < diceNumber; i++)
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
        else
        {
            // If the input is not in the dice format, assume it's a plain number
            staticValue = int.Parse(input);
        }
    }

    public static int CalculateDistance(Vector3 object1, Vector3 object2)
    {
        float feetMultiplier = 0.3048f;
        float realWorldDistanceInFeet = Vector3.Distance(object1, object2) / feetMultiplier;
        //NOTE: size multiplier is calculated by the lossy scale of a character.
        float sizeMultiplier = 0.1f;
        float distance = realWorldDistanceInFeet / sizeMultiplier;
        return (int)distance;
    }
}
