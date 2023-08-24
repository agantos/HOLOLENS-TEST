using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnEconomyTabManager : MonoBehaviour
{
    Character character;
    public GameObject Container;
    public GameObject TurnEconomyElementPrefab;
    public List<GameObject> UIelementList = new List<GameObject>();

    Vector3 scale;

    void Start()
    {
        scale = gameObject.transform.localScale;
    }

    public void Activate()
    {
        character = GameManager.characterPool[SelectAbilityUIManager.UI_Info.currentPlayer];
        CreateUI();

        //"Spawn" object
        gameObject.transform.localScale = scale;
    }

    void CreateUI()
    {        
        foreach (string turnEconomyName in character.turnEconomy.Keys)
        {
            string name = turnEconomyName;
            int maxValue = character.turnEconomy[turnEconomyName];
            int currentValue = character.currentTurnEconomy[turnEconomyName];

            GameObject element = Instantiate(TurnEconomyElementPrefab, Container.transform);
            UIelementList.Add(element);

            TurnEconomyUIElement turnEconomyUIElement = element.GetComponent<TurnEconomyUIElement>();

            turnEconomyUIElement.SetNameGameobject(name);
            turnEconomyUIElement.SetUses(currentValue, maxValue);
            turnEconomyUIElement.turnEconomyName = turnEconomyName;
            turnEconomyUIElement.SetButtonOnClick(() => { SelectAbilityUIManager.OnClick_TurnEconomyButton(turnEconomyUIElement); });
        }
    }

    public void Deactivate()
    {
        foreach(GameObject instance in UIelementList)
        {
            Destroy(instance);
        }
        character = null;

        UIelementList.Clear();

        //"Despawn" object
        gameObject.transform.localScale = Vector3.zero;
    }
}
