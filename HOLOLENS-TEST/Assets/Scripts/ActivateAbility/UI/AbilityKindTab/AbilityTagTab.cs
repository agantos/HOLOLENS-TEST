using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbilityTagTab : MonoBehaviour
{
    public GameObject pageManagerInstance;
    public GameObject ButtonPrefab;
    public TMP_Text title;

    public void CreateUI()
    {
        Character character = GameManager.GetInstance().GetCurrentPlayer_Character();

        title.text = SelectAbilityUIManager.Instance.UI_Info.currentTurnEconomy;

        HashSet<string> tags = new HashSet<string>();

        //Foreach ability that is the currently selected turn economy add its tags to the hash set
        foreach (string abilityName in character.abilities.Values)
        {
            Ability ability = character.GetCharacterAbility(abilityName);
            if (ability.turnEconomyCost.ContainsKey(SelectAbilityUIManager.Instance.UI_Info.currentTurnEconomy))
            {
                foreach (string tag in ability.tags)
                {
                    tags.Add(tag);
                }
            }

        }

        //Create and Initialize the UI elements
        foreach (string tag in tags)
        {
            GameObject instance = pageManagerInstance.GetComponent<PageManager>().AddElement(ButtonPrefab);

            AbilityTagElement abilityTagElementUI = instance.GetComponent<AbilityTagElement>();

            abilityTagElementUI.SetNameGameobject(tag);
            abilityTagElementUI.abilityTag = tag;
            abilityTagElementUI.SetButtonOnClick(() => { SelectAbilityUIManager.Instance.OnClick_AbilityTagButton(abilityTagElementUI); });
        }
    }

    public void ClearUI()
    {
        pageManagerInstance.GetComponent<PageManager>().ClearState();
    }
}

