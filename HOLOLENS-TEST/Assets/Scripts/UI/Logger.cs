using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum ColorType { CHARACTER_1, CHARACTER_2, SUCCESS, FAILURE, ABILITY, NORMAL}
public class Logger : MonoBehaviour
{

    private TextMeshProUGUI text;
    private string line;
    private ScrollRect scrollRect;

    private Dictionary<ColorType, string> colorTypesDictionary = new Dictionary<ColorType, string>();


    public static Logger Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
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

    public void AddLine()
    {
        string s = "\n____\n";
        text.text += line + s + "\n";
        ScrollToBottom();
        line = "";
    }

    public void AddToLine(string s)
    {
        line += s;
    }

    public string GetColoredString(string s, ColorType type)
    {
        string coloredString = "<color=" + colorTypesDictionary[type] + ">";
        coloredString += s;
        coloredString += "</color>";
        return coloredString;
    }

    public string GetBoldedString(string s)
    {
        return "<b>" + s + "</b>";
    }

    public string GetItalicString(string s)
    {
        return "<i>" + s + "</i>";
    }

    void ScrollToBottom()
    {        
        scrollRect.verticalNormalizedPosition = 0f;
    }

    public void ScrollUp()
    {
        if (scrollRect.verticalNormalizedPosition >= 0.8f)
            scrollRect.verticalNormalizedPosition = 1;
        else
            scrollRect.verticalNormalizedPosition += 0.2f;
    }

    public void ScrollDown()
    {
        if(scrollRect.verticalNormalizedPosition >= 0.2f)
            scrollRect.verticalNormalizedPosition -= 0.2f;
        else
            scrollRect.verticalNormalizedPosition = 0f;
    }

    public void Log_Effect(EffectStats effect, Character roller, int rollerResult, bool attack)
    {
        int statBonus;
        int finalRoll;
        string dice;
        if (attack)
        {
            statBonus = roller.GetStats().GetStat(effect.effectSucceedsStats.attackerStat.statName).GetCurrentValue();
            finalRoll = (int)((statBonus + rollerResult) * effect.effectSucceedsStats.attackerStat.multiplier);
            dice = effect.effectSucceedsStats.attackerStat.bonus;
        }
        else
        {
            statBonus = roller.GetStats().GetStat(effect.effectSucceedsStats.defenderStat.statName).GetCurrentValue();
            finalRoll = (int)((statBonus + rollerResult) * effect.effectSucceedsStats.defenderStat.multiplier);
            dice = effect.effectSucceedsStats.defenderStat.bonus;
        }
                    
        int passNumber = GameplayCalculatorFunctions.CalculateDiceRoll(effect.effectSucceedsStats.staticNumberToPass);

        AddToLine(GetColoredString(roller.name, ColorType.CHARACTER_1));
        AddToLine(" rolls ");
        AddToLine(dice + " + " + statBonus.ToString());
        AddToLine(" against ");
        AddToLine(GetBoldedString(passNumber.ToString()));
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

    public void Log_EffectComparison(EffectStats effect, Character attacker, Character defender, int attackerResult, int defenderResult)
    {
        int statBonusAttacker = attacker.GetStats().GetStat(effect.effectSucceedsStats.attackerStat.statName).GetCurrentValue();
        int finalAttackRoll = (int)((statBonusAttacker + attackerResult) * effect.effectSucceedsStats.attackerStat.multiplier);

        int statBonusDefender = defender.GetStats().GetStat(effect.effectSucceedsStats.defenderStat.statName).GetCurrentValue();
        int finalDefenceRoll = (int)((statBonusDefender + defenderResult) * effect.effectSucceedsStats.defenderStat.multiplier);

        AddToLine(GetColoredString(attacker.name, ColorType.CHARACTER_1));
        AddToLine(" rolls ");
        AddToLine(effect.effectSucceedsStats.attackerStat.bonus + " + " + statBonusAttacker.ToString());
        AddToLine(" against ");
        AddToLine(GetColoredString(defender.name, ColorType.CHARACTER_2) + ".");
        AddToLine(" Comparison: ");
        AddToLine(GetColoredString(attackerResult.ToString() + " + " + statBonusAttacker.ToString(), ColorType.CHARACTER_1));
        AddToLine(" vs ");
        AddToLine(GetColoredString(defenderResult.ToString() + " + " + statBonusDefender.ToString(), ColorType.CHARACTER_2));
        AddToLine("\nResults are ");
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

    public void Log_Damage(int damage, string damagedStatName, Character defender, Character attacker)
    {
        AddToLine(GetColoredString(attacker.name, ColorType.CHARACTER_1));
        AddToLine(" deals ");
        AddToLine(GetBoldedString(damage.ToString()));
        AddToLine(" damage to ");
        AddToLine(GetColoredString(defender.name, ColorType.CHARACTER_2) + "'s ");
        AddToLine(damagedStatName);

        AddLine();
    }

    public void Log_Heal(int damage, string damagedStatName, Character defender, Character attacker)
    {
        AddToLine(GetColoredString(attacker.name, ColorType.CHARACTER_1));
        AddToLine(" Heals ");
        AddToLine(GetBoldedString(damage.ToString()));
        AddToLine(" damage to ");
        AddToLine(GetColoredString(defender.name, ColorType.CHARACTER_2) + "'s ");
        AddToLine(damagedStatName);

        AddLine();
    }

    public void Log_Apply_Temporal(int damage, string damagedStatName, int duration, Character defender, Character attacker)
    {
        if (damage < 0)
        {
            AddToLine(GetColoredString(attacker.name, ColorType.CHARACTER_1));
            AddToLine(" decreases ");
            AddToLine(GetColoredString(defender.name, ColorType.CHARACTER_2) + "'s ");
            AddToLine(damagedStatName);
            AddToLine(" by ");
            AddToLine(GetBoldedString((-damage).ToString()));
            AddToLine(" for " + duration + " rounds ");
        }
        else
        {
            AddToLine(GetColoredString(attacker.name, ColorType.CHARACTER_1));
            AddToLine(" enhances ");
            AddToLine(GetColoredString(defender.name, ColorType.CHARACTER_2) + "'s ");
            AddToLine(damagedStatName);
            AddToLine(" by ");
            AddToLine(GetBoldedString(damage.ToString()));
            AddToLine(" for " + duration + " rounds ");
        }
        
        AddLine();
    }

}