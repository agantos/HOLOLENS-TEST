using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public enum AbiltyDisplayColors { CHARACTER_STAT, TURN_ECONOMY, DAMAGE, FAILURE, ABILITY, NORMAL }

public class AbilityDisplayManager : MonoBehaviour
{
    public TMP_Text title;
    public static Ability displayingAbility;

    public static Dictionary<AbiltyDisplayColors, string> colorTypesDictionary = new Dictionary<AbiltyDisplayColors, string>();

    // Start is called before the first frame update
    void Start()
    {
        colorTypesDictionary.Add(AbiltyDisplayColors.CHARACTER_STAT, "#65FF99");
        colorTypesDictionary.Add(AbiltyDisplayColors.TURN_ECONOMY, "#5BC4FF");
        colorTypesDictionary.Add(AbiltyDisplayColors.DAMAGE, "#E5D32D");
        CreateUI();
    }

    void CreateUI() {
        displayingAbility = AbilitiesManager.abilityPool["Finesse Ranged Attack"];
        title.text = displayingAbility.name;
    }

    public static string ColorString(string s, AbiltyDisplayColors type)
    {
        string coloredString = "<color=" + colorTypesDictionary[type] + ">";
        coloredString += s;
        coloredString += "</color>";
        return coloredString;
    }
    public static string BoldString(string s)
    {
        return "<b>" + s + "</b>";
    }

    public static string GetItalicString(string s)
    {
        return "<i>" + s + "</i>";
    }
}
