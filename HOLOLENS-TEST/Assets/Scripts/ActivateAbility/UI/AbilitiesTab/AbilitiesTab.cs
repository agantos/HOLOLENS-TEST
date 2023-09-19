using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbilitiesTab : MonoBehaviour
{
    public GameObject pageManagerInstance;
    public GameObject ButtonPrefab;
    public TMP_Text title;
    public GameObject SpawnRadius;

    public void CreateUI()
    {
        Character character = GameManager.GetInstance().GetCurrentPlayer_Character();

        title.SetText(SelectAbilityUIManager.Instance.UI_Info.currentTag);

        foreach (string abilityName in character.abilities.Values)
        {
            Ability ability = character.GetCharacterAbility(abilityName);
            if (ability.turnEconomyCost.ContainsKey(SelectAbilityUIManager.Instance.UI_Info.currentTurnEconomy))
            {
                if (ability.tags.Contains(SelectAbilityUIManager.Instance.UI_Info.currentTag))
                {
                    GameObject instance = pageManagerInstance.GetComponent<PageManager>().AddElement(ButtonPrefab);

                    instance.name = abilityName + "_Button";
                    instance.GetComponentInChildren<BeginAbilityActivationButton>().Initialize(abilityName, SelectAbilityUIManager.Instance.UI_Info.currentPlayer, SpawnRadius);
                    instance.GetComponentInChildren<BeginAbilityActivationButton>().SetButtonOnClick(() => {  SelectAbilityUIManager.Instance.OnClick_AbilityButton(abilityName); });
                }
            }
        }
    }

    public void ClearUI()
    {
        pageManagerInstance.GetComponent<PageManager>().ClearState();
    }
}
