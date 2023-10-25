using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SuccessInfoManager : MonoBehaviour
{
    public TMP_Text OnSaveText;
    public TMP_Text RollExplainedText;    

    public void CreateUI(Ability displayingAbility, Character character = null)
    {
        CreateInfoText(displayingAbility, character);
        CreateOnSaveText(displayingAbility);
    }

    void CreateInfoText(Ability displayingAbility, Character c)
    {
        string text = "";
        EffectSucceedsStats succeedStats = displayingAbility.effects[0].effectSucceedsStats;

        switch (succeedStats.onCondition)
        {
            case EffectSuccessCondition.AUTOMATIC:
                text = "The ability succeeds automatically";
                break;
            case EffectSuccessCondition.ATTACKER_ROLLS:
                text += "";
                break;
            case EffectSuccessCondition.DEFENDER_ROLLS:

                break;
            case EffectSuccessCondition.COMPARISON:
                text += AbilityDisplayGeneralMethods.Instance.BoldString("Defenders") + " compare ";
                text += AbilityDisplayGeneralMethods.Instance.ColorString(succeedStats.defenderStat.statName, AbilityDisplayColors.CHARACTER_STAT) + " ";
                text += "against " + AbilityDisplayGeneralMethods.Instance.BoldString("Attacker's") + " ";
                text += AbilityDisplayGeneralMethods.Instance.ColorString(succeedStats.attackerStat.statName, AbilityDisplayColors.CHARACTER_STAT);

                

                if (c != null)
                    text += " (= " +
                        AbilityDisplayGeneralMethods.Instance.ColorString(
                            c.GetStat(
                                succeedStats.attackerStat.statName).GetCurrentValue().ToString(),
                                AbilityDisplayColors.DAMAGE
                            ) +
                        ")"
                    ;

                text+= ". ";
                text += "Defenders save on bigger roll result.";
                break;
        }

        RollExplainedText.text = text;           
    }

    void CreateOnSaveText(Ability displayingAbility)
    {
        EffectDamageStats damageStats = displayingAbility.effects[0].damageStats;

        string text = "";
        if (damageStats.onSavedMultiplier == 0)
            text += "No Damage";
        else
        {
            text += "Defenders take ";
            text += AbilityDisplayGeneralMethods.Instance.ColorString(damageStats.onSavedMultiplier.ToString(), AbilityDisplayColors.DAMAGE);
            text += " times the damage.";
        }

        OnSaveText.text = text;            
    }

    public void ClearUI() { /*Empty, exists for symmetry*/ }
    
}
