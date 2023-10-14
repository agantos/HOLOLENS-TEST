using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityDisplayColors { CHARACTER_STAT, TURN_ECONOMY, DAMAGE, FAILURE, ABILITY, NORMAL }
public class AbilityDisplayGeneralMethods : MonoBehaviour
{
    public Dictionary<AbilityDisplayColors, string> colorTypesDictionary = new Dictionary<AbilityDisplayColors, string>();

    public static AbilityDisplayGeneralMethods Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            colorTypesDictionary.Add(AbilityDisplayColors.CHARACTER_STAT, "#65FF99");
            colorTypesDictionary.Add(AbilityDisplayColors.TURN_ECONOMY, "#5BC4FF");
            colorTypesDictionary.Add(AbilityDisplayColors.DAMAGE, "#E5D32D");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public string ColorString(string s, AbilityDisplayColors type)
    {
        string coloredString = "<color=" + colorTypesDictionary[type] + ">";
        coloredString += s;
        coloredString += "</color>";
        return coloredString;
    }
    public string BoldString(string s)
    {
        return "<b>" + s + "</b>";
    }

    public string GetItalicString(string s)
    {
        return "<i>" + s + "</i>";
    }
}
