using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.Assertions;

public class GameplayCalculatorFunctions
{
    private static GameplayCalculatorFunctions instance = null;
    public static float localscale_1feet;
    public static float gamespaceScale;
    public static System.Random random;

    static public GameplayCalculatorFunctions GetInstance(GameObject gamespaceObject)
    {
        if (instance == null)
        {
            instance = new GameplayCalculatorFunctions(gamespaceObject);
        }
        return instance;
    }

    private GameplayCalculatorFunctions(GameObject gamespaceObject) {
        
        //Set the gamespace Scale
        Assert.AreEqual(gamespaceObject.transform.lossyScale, gamespaceObject.transform.localScale);
        gamespaceScale = gamespaceObject.transform.localScale.x;
        random = new System.Random();
        
        //Set 1_feet in local scale
        localscale_1feet = 0.3048f;
    }


    public static int CalculateDiceRoll(string value)
    {
        int sum = 0;
        int diceNumber, diceSides, staticValue;

        ParseDiceString(value, out diceNumber, out diceSides, out staticValue);

        for (int i = 0; i < diceNumber; i++)
        {
            sum += random.Next(1, diceSides);
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

    //Takes an integer and returns a float that is the scale for unity.
    public static float FeetToUnityMeasurement(float feet)
    {
        return feet * localscale_1feet;
    }

    public static float RealWorldToGameFeet(float realWorldMeasurementInMeters)
    {
        float realWorldMeasurementInFeet = realWorldMeasurementInMeters * 3.2808399f;
        return realWorldMeasurementInFeet / gamespaceScale;
    }

    public static float CalculateDistanceInFeet(Vector3 object1, Vector3 object2)
    {
        float lossyDistanceInMeters = Vector3.Distance(object1, object2);
        return RealWorldToGameFeet(lossyDistanceInMeters);
    }
}
