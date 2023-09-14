using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnEconomyTab : MonoBehaviour
{
    public GameObject PageManagerPrefab;
    public GameObject ButtonPrefab;
    public TMP_Text title;

    GameObject pageManager;

    public void CreateUI()
    {
        Character character = GameManager.GetInstance().GetCurrentPlayer_Character();

        title.SetText("Turn Economy");
        pageManager = Instantiate(PageManagerPrefab, transform);

        foreach (string turnEconomyName in character.turnEconomy.Keys)
        {
            string name = turnEconomyName;
            int maxValue = character.turnEconomy[turnEconomyName];
            int currentValue = character.currentTurnEconomy[turnEconomyName];

            GameObject instance = pageManager.GetComponent<PageManager>().AddElement(ButtonPrefab);

            TurnEconomyUIElement turnEconomyUIElement = instance.GetComponent<TurnEconomyUIElement>();

            turnEconomyUIElement.SetNameGameobject(name);
            turnEconomyUIElement.SetUses(currentValue, maxValue);
            turnEconomyUIElement.turnEconomyName = turnEconomyName;
            turnEconomyUIElement.SetButtonOnClick(() => { SelectAbilityUIManager.OnClick_TurnEconomyButton(turnEconomyUIElement); });
        }
    }
}