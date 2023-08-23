using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SuccessInfoManager : MonoBehaviour
{
    public TMP_Text OnSaveText;
    public TMP_Text RollExplainedText;
    Ability displayingAbility;
    EffectSucceedsStats succeedStats;
    EffectDamageStats damageStats;

    // Start is called before the first frame update
    void Start()
    {
        displayingAbility = AbilityDisplayManager.displayingAbility;
        succeedStats = displayingAbility.effects[0].effectSucceedsStats;
        damageStats = displayingAbility.effects[0].damageStats;

        CreateUI();
    }

    void Update()
    {
    }

    void CreateUI()
    {
        CreateInfoText();
        CreateOnSaveText();
    }

    void CreateInfoText()
    {
        string text = "";

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

    void CreateOnSaveText()
    {
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
    
}
