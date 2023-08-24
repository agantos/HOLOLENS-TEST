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

    Ability displayingAbility;
    EffectDamageStats damageStats;

    // Start is called before the first frame update
    void Start()
    {
        displayingAbility = AbilityDisplayManager.displayingAbility;
        damageStats = displayingAbility.effects[0].damageStats;

        CreateUI();
    }

    void CreateUI()
    {
        CreateDamageText();
        CreateRollExplained();
        CreateDescription();
    }

    void CreateDescription()
    {
        DescriptionText.text = displayingAbility.description;
    }

    void CreateDamageText()
    {
        string text = "";
        int minDamage = 0, maxDamage = 0;

        int diceNumber, diceSides, staticValue;
        GameplayCalculatorFunctions.ParseDiceString(damageStats.baseValue, out diceNumber, out diceSides, out staticValue);

        minDamage += 1 * diceNumber + staticValue;
        maxDamage += diceSides * diceNumber + staticValue; 

        text = AbilityDisplayManager.BoldString(AbilityDisplayManager.ColorString(minDamage.ToString(), AbiltyDisplayColors.DAMAGE))
                + " - " 
                + AbilityDisplayManager.BoldString(AbilityDisplayManager.ColorString(maxDamage.ToString(), AbiltyDisplayColors.DAMAGE))
                + " Damage**";

        DamageText.text = text;
    }

    void CreateTypeImage()
    {

    }

    void CreateRollExplained()
    {
        string baseDamage = "";
        baseDamage += AbilityDisplayManager.ColorString(damageStats.baseValue, AbiltyDisplayColors.DAMAGE);
        baseDamage += " (Base)";
        AddToRollExplainedContainer(baseDamage);

        foreach (string stat in damageStats.statsAffecting)
        {
            string text = "+ ";
            text += AbilityDisplayManager.ColorString(stat, AbiltyDisplayColors.CHARACTER_STAT);

            AddToRollExplainedContainer(text);
        }
    }

    void AddToRollExplainedContainer(string t)
    {
        GameObject instance = Instantiate(DamageExplainedStatPrefab, RollExplainedContainer.transform);
        rollExplainedTextList.Add(instance);

        instance.GetComponent<TMP_Text>().text = t;
    }


}
