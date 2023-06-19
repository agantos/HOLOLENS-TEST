using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum ColorType { CHARACTER_1, CHARACTER_2, SUCCESS, FAILURE, ABILITY, NORMAL}
public class Logger : MonoBehaviour
{
    private static Logger instance;
    private static TextMeshProUGUI text;
    private static string line;
    private static ScrollRect scrollRect;

    private static Dictionary<ColorType, string> colorTypesDictionary = new Dictionary<ColorType, string>();


    public static Logger Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject); // Ensures only one instance exists
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject); // Keeps the singleton across scenes
    }

    // Start is called before the first frame update
    void Start()
    {
        //Initialize color table
        colorTypesDictionary.Add(ColorType.CHARACTER_1, "#DDAA33");
        colorTypesDictionary.Add(ColorType.CHARACTER_2, "#80ADA0");
        colorTypesDictionary.Add(ColorType.ABILITY, "#454F83");
        colorTypesDictionary.Add(ColorType.FAILURE, "#AC1820");
        colorTypesDictionary.Add(ColorType.SUCCESS, "#3EA055");
        colorTypesDictionary.Add(ColorType.NORMAL, "#1F1F1F");

        //Get the scroll and text components of Gameobject.
        text = gameObject.GetComponent<TextMeshProUGUI>();
        scrollRect = gameObject.GetComponentInParent<ScrollRect>();
    }

    public static void AddLine()
    {
        string s = "\n____\n";
        text.text += line + s + "\n";
        ScrollToBottom();
        line = "";
    }

    public static void AddToLine(string s)
    {
        line += s;
    }

    public static string GetColoredString(string s, ColorType type)
    {
        string coloredString = "<color=" + colorTypesDictionary[type] + ">";
        coloredString += s;
        coloredString += "</color>";
        return coloredString;
    }

    public static string GetBoldedString(string s)
    {
        return "<b>" + s + "</b>";
    }

    public static string GetItalicString(string s)
    {
        return "<i>" + s + "</i>";
    }

    static void ScrollToBottom()
    {
        scrollRect.verticalNormalizedPosition = 0f;
    }

    public static void Log_Effect(EffectStats effect, Character roller, int rollerResult, bool attack)
    {
        int statBonus;
        int finalRoll;
        if (attack)
        {
            statBonus = roller.GetStats().GetStat(effect.effectSucceedsStats.attackerStat.statName).GetCurrentValue();
            finalRoll = (int)((statBonus + rollerResult) * effect.effectSucceedsStats.attackerStat.multiplier);
        }
        else
        {
            statBonus = roller.GetStats().GetStat(effect.effectSucceedsStats.defenderStat.statName).GetCurrentValue();
            finalRoll = (int)((statBonus + rollerResult) * effect.effectSucceedsStats.defenderStat.multiplier);
        }
                    
        int passNumber = GameplayCalculatorFunctions.CalculateDiceRoll(effect.effectSucceedsStats.staticNumberToPass);

        AddToLine(GetColoredString(roller.name, ColorType.CHARACTER_1));
        AddToLine(" rolls ");
        AddToLine(effect.effectSucceedsStats.attackerStat.bonus + " + " + statBonus.ToString());
        AddToLine(" against ");
        AddToLine(passNumber.ToString());
        AddToLine(": ");
        AddToLine(rollerResult.ToString() + " + " + statBonus.ToString());
        AddToLine("\n Result is " + GetBoldedString(finalRoll.ToString()));
        AddToLine(" and ");

        if (finalRoll > passNumber)
        {
            if (attack)
                AddToLine(GetColoredString("Effect Succeeds", ColorType.SUCCESS));
            else
                AddToLine(GetColoredString("Effect Fails", ColorType.FAILURE));
        }
        else
        {
            if (attack)
                AddToLine(GetColoredString("Effect Fails", ColorType.FAILURE));
            else
                AddToLine(GetColoredString("Effect Succeeds", ColorType.SUCCESS));
        }            

        AddLine();
    }

    public static void Log_EffectComparison(EffectStats effect, Character attacker, Character defender, int attackerResult, int defenderResult)
    {
        int statBonusAttacker = attacker.GetStats().GetStat(effect.effectSucceedsStats.attackerStat.statName).GetCurrentValue();
        int finalAttackRoll = (int)((statBonusAttacker + attackerResult) * effect.effectSucceedsStats.attackerStat.multiplier);

        int statBonusDefender = defender.GetStats().GetStat(effect.effectSucceedsStats.defenderStat.statName).GetCurrentValue();
        int finalDefenceRoll = (int)((statBonusDefender + attackerResult) * effect.effectSucceedsStats.defenderStat.multiplier);

        AddToLine(GetColoredString(attacker.name, ColorType.CHARACTER_1));
        AddToLine(" rolls ");
        AddToLine(effect.effectSucceedsStats.attackerStat.bonus + " + " + statBonusAttacker.ToString());
        AddToLine(" against ");
        AddToLine(GetColoredString(defender.name, ColorType.CHARACTER_2) + ".");
        AddToLine("Attack Roll: ");
        AddToLine(GetColoredString(attackerResult.ToString() + " + " + statBonusAttacker.ToString(), ColorType.CHARACTER_1));
        AddToLine(" vs ");
        AddToLine(GetColoredString(defenderResult.ToString() + " + " + statBonusDefender.ToString(), ColorType.CHARACTER_2));
        AddToLine("\n Results are ");
        AddToLine(GetBoldedString(GetColoredString(finalAttackRoll.ToString(), ColorType.CHARACTER_1)));
        AddToLine(" vs ");
        AddToLine(GetBoldedString(GetColoredString(finalDefenceRoll.ToString(), ColorType.CHARACTER_2)));
        AddToLine(" and ");

        if (finalAttackRoll > finalDefenceRoll)
            AddToLine(GetColoredString("Effect Succeeds", ColorType.SUCCESS));
        else
            AddToLine(GetColoredString("Effect Fails", ColorType.FAILURE));

        AddLine();
    }

}