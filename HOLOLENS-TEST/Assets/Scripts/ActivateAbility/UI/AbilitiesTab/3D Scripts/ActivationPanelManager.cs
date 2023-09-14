using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActivationPanelManager : MonoBehaviour
{
    public GameObject PageManagerPrefab;
    public GameObject ButtonPrefab;
    public TMP_Text title;
    public GameObject SpawnRadius;

    GameObject pageManager;

    public void CreateUI()
    {
        Character character = GameManager.GetInstance().GetCurrentPlayer_Character();

        title.SetText("Abilities");
        pageManager = Instantiate(PageManagerPrefab, transform);

        foreach (string abilityName in character.abilities.Values)
        {
            Ability ability = character.GetCharacterAbility(abilityName);
            if (true)
            {
                if (true)
                {
                    GameObject instance = pageManager.GetComponent<PageManager>().AddElement(ButtonPrefab);

                    instance.name = abilityName + "_Button";
                    instance.GetComponentInChildren<BeginAbilityActivationButton>().Initialize(abilityName, SelectAbilityUIManager.UI_Info.currentPlayer, SpawnRadius);
                    instance.GetComponentInChildren<BeginAbilityActivationButton>().SetButtonOnClick(SelectAbilityUIManager.OnClick_AbilityButton);
                }
            }
        }
    }
}
