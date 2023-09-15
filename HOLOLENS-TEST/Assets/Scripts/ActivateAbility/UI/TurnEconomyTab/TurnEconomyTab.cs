using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnEconomyTab : MonoBehaviour
{
    public GameObject pageManagerInstance;
    public GameObject ButtonPrefab;
    public TMP_Text title;

    public void CreateUI()
    {
        Character character = GameManager.GetInstance().GetCurrentPlayer_Character();
        title.SetText("Turn Economy");


        foreach (string turnEconomyName in character.turnEconomy.Keys)
        {
            int maxValue = character.turnEconomy[turnEconomyName];
            int currentValue = character.currentTurnEconomy[turnEconomyName];

            GameObject instance = pageManagerInstance.GetComponent<PageManager>().AddElement(ButtonPrefab);

            TurnEconomyUIElement turnEconomyUIElement = instance.GetComponent<TurnEconomyUIElement>();

            turnEconomyUIElement.SetNameGameobject(turnEconomyName);
            turnEconomyUIElement.SetUses(currentValue, maxValue);
            turnEconomyUIElement.SetButtonOnClick(() => { SelectAbilityUIManager.Instance.OnClick_TurnEconomyButton(turnEconomyUIElement); });
        }
    }

    public void ClearUI()
    {
        pageManagerInstance.GetComponent<PageManager>().ClearState();
    }
}