using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EffectInfoManager : MonoBehaviour
{
    //Initialized in Unity Editor
    public GameObject DamageExplainedStatPrefab;
    public RawImage typeImage;
    public TMP_Text DamageText;
    public TMP_Text DescriptionText;
    public GameObject RollExplainedContainer;

    List<GameObject> rollExplainedTextList = new List<GameObject>();

    public void CreateUI(Ability displayingAbility)
    {
        CreateDamageText(displayingAbility);
        CreateRollExplained(displayingAbility);
        CreateDescription(displayingAbility);
    }

    void CreateDescription(Ability displayingAbility)
    {
        DescriptionText.text = displayingAbility.description;
    }

    void CreateDamageText(Ability displayingAbility)
    {
        string text = "";
        int minDamage = 0, maxDamage = 0;

        int diceNumber, diceSides, staticValue;

        EffectDamageStats damageStats = displayingAbility.effects[0].damageStats;
        GameplayCalculatorFunctions.ParseDiceString(damageStats.baseValue, out diceNumber, out diceSides, out staticValue);

        minDamage += 1 * diceNumber + staticValue;
        maxDamage += diceSides * diceNumber + staticValue; 

        text = AbilityDisplayGeneralMethods.Instance.BoldString(AbilityDisplayGeneralMethods.Instance.ColorString(minDamage.ToString(), AbilityDisplayColors.DAMAGE))
                + " - " 
                + AbilityDisplayGeneralMethods.Instance.BoldString(AbilityDisplayGeneralMethods.Instance.ColorString(maxDamage.ToString(), AbilityDisplayColors.DAMAGE))
                + " Damage**";

        DamageText.text = text;
    }

    void CreateTypeImage()
    {

    }

    void CreateRollExplained(Ability displayingAbility)
    {
        EffectDamageStats damageStats = displayingAbility.effects[0].damageStats;
        string baseDamage = "";
        baseDamage += AbilityDisplayGeneralMethods.Instance.ColorString(damageStats.baseValue, AbilityDisplayColors.DAMAGE);
        baseDamage += " (Base)";
        AddToRollExplainedContainer(baseDamage);

        foreach (string stat in damageStats.statsAffecting)
        {
            string text = "+ ";
            text += AbilityDisplayGeneralMethods.Instance.ColorString(stat, AbilityDisplayColors.CHARACTER_STAT);

            AddToRollExplainedContainer(text);
        }
    }

    void AddToRollExplainedContainer(string t)
    {
        GameObject instance = Instantiate(DamageExplainedStatPrefab, RollExplainedContainer.transform);
        rollExplainedTextList.Add(instance);

        instance.GetComponent<TMP_Text>().text = t;
    }

    public void ClearUI()
    {
        foreach(GameObject obj in rollExplainedTextList)
        {
            Destroy(obj);
        }

        rollExplainedTextList.Clear();
    }


}
