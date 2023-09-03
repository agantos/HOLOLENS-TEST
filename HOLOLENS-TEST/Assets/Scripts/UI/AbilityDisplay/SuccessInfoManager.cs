using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SuccessInfoManager : MonoBehaviour
{
    public TMP_Text OnSaveText;
    public TMP_Text RollExplainedText;    

    public void CreateUI(Ability displayingAbility)
    {
        CreateInfoText(displayingAbility);
        CreateOnSaveText(displayingAbility);
    }

    void CreateInfoText(Ability displayingAbility)
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
                text += AbilityDisplayManager.BoldString("Defenders") + " compare ";
                text += AbilityDisplayManager.ColorString(succeedStats.defenderStat.statName, AbiltyDisplayColors.CHARACTER_STAT) + " ";
                text += "against " + AbilityDisplayManager.BoldString("Attacker's") + " ";
                text += AbilityDisplayManager.ColorString(succeedStats.attackerStat.statName, AbiltyDisplayColors.CHARACTER_STAT) + ". ";
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
            text += AbilityDisplayManager.ColorString(damageStats.onSavedMultiplier.ToString(), AbiltyDisplayColors.DAMAGE);
            text += " times the damage.";
        }

        OnSaveText.text = text;            
    }

    public void DestroyUI() { /*Empty, exists for symmetry*/ }
    
}
