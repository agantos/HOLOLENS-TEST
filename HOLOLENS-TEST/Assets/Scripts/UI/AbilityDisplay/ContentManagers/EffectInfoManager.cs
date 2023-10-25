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

    public void CreateUI(Ability displayingAbility, Character character = null)
    {
        CreateDamageText(displayingAbility, character);
        CreateRollExplained(displayingAbility, character);
        CreateDescription(displayingAbility);
        CreateTypeImage(displayingAbility);
    }

    void CreateDescription(Ability displayingAbility)
    {
        DescriptionText.text = displayingAbility.description;
    }

    void CreateDamageText(Ability displayingAbility, Character character)
    {
        string text = "";

        if(displayingAbility.effects[0].type == EffectType.DAMAGE)
        {
            int minDamage = 0, maxDamage = 0;

            int diceNumber, diceSides, staticValue, bonuses = 0;

            EffectDamageStats damageStats = displayingAbility.effects[0].damageStats;
            GameplayCalculatorFunctions.ParseDiceString(damageStats.baseValue, out diceNumber, out diceSides, out staticValue);

            if (character != null)
                bonuses = displayingAbility.effects[0].damageStats.GetStatBonusSum(character.GetStats());

            minDamage += 1 * diceNumber + staticValue + bonuses;
            maxDamage += diceSides * diceNumber + staticValue + bonuses;

            text = AbilityDisplayGeneralMethods.Instance.BoldString(AbilityDisplayGeneralMethods.Instance.ColorString(minDamage.ToString(), AbilityDisplayColors.DAMAGE))
                    + " - "
                    + AbilityDisplayGeneralMethods.Instance.BoldString(AbilityDisplayGeneralMethods.Instance.ColorString(maxDamage.ToString(), AbilityDisplayColors.DAMAGE))
                    + " Damage";

            DamageText.text = text;
        }
        else if(displayingAbility.effects[0].type == EffectType.HEALING)
        {
            int minHealing = 0, maxHealing = 0;

            int diceNumber, diceSides, staticValue, bonuses = 0;

            EffectDamageStats damageStats = displayingAbility.effects[0].damageStats;
            GameplayCalculatorFunctions.ParseDiceString(damageStats.baseValue, out diceNumber, out diceSides, out staticValue);

            if (character != null)
                bonuses = displayingAbility.effects[0].damageStats.GetStatBonusSum(character.GetStats());

            minHealing += 1 * diceNumber + staticValue;
            maxHealing += diceSides * diceNumber + staticValue;

            text = AbilityDisplayGeneralMethods.Instance.BoldString(AbilityDisplayGeneralMethods.Instance.ColorString(minHealing.ToString(), AbilityDisplayColors.DAMAGE))
                    + " - "
                    + AbilityDisplayGeneralMethods.Instance.BoldString(AbilityDisplayGeneralMethods.Instance.ColorString(maxHealing.ToString(), AbilityDisplayColors.DAMAGE))
                    + " Healing";

            DamageText.text = text;
        }
        else
        {
            DamageText.text = "Read Description";
        }
        
    }

    void CreateTypeImage(Ability displayingAbility)
    {
        string path = "UI/AbilityDisplayImages/Effect/";

        switch (displayingAbility.effects[0].type)
        {
            case EffectType.DAMAGE:
                path += "Effect_Damage";
                break;
            case EffectType.HEALING:
                path += "Effect_Healing";
                break;
            case EffectType.TEMPORAL:
                path += "Effect_Magical";
                break;
        }


        typeImage.texture = (Texture)Resources.Load(path);
    }

    void CreateRollExplained(Ability displayingAbility, Character c)
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

            if (c != null)
                text += " = " + c.GetStat(stat).GetCurrentValue(); 

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
