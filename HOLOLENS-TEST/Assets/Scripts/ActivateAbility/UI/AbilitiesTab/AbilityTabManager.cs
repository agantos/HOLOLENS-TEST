using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

// Spawns the buttons for the abilities of a specific character 
// Is placed in the content of a scroll window
public class AbilityTabManager : MonoBehaviour
{
    public GameObject Content;
    public GameObject SpawnRadius;
    public GameObject BeginActivationButtonPrefab;
    public GameObject BackButton;
    public List<GameObject> UIelementList = new List<GameObject>();

    public TMP_Text title;

    Character character;
    Vector3 scale;

    void Start()
    {
        BackButton.GetComponent<Button>().onClick.AddListener(SelectAbilityUIManager.OnClick_BackButton_AbilityTab);
        scale = gameObject.transform.localScale;
    }

    void CreateUI()
    {
        title.SetText(SelectAbilityUIManager.UI_Info.currentTag);

        foreach(string abilityName in character.abilities.Values)
        {
            Ability ability = character.GetCharacterAbility(abilityName);
            if (ability.turnEconomyCost.ContainsKey(SelectAbilityUIManager.UI_Info.currentTurnEconomy))
            {
                if (ability.tags.Contains(SelectAbilityUIManager.UI_Info.currentTag)) {
                    GameObject instance = Instantiate(BeginActivationButtonPrefab, Content.transform);
                    instance.name = abilityName + "_Button";
                    instance.GetComponentInChildren<BeginAbilityActivationButton>().Initialize(abilityName, SelectAbilityUIManager.UI_Info.currentPlayer, SpawnRadius);
                    instance.GetComponentInChildren<BeginAbilityActivationButton>().SetButtonOnClick(SelectAbilityUIManager.OnClick_AbilityButton);
                    
                    UIelementList.Add(instance);
                }
            }
        }
    }

    public void ClearState()
    {
        foreach (GameObject instance in UIelementList)
        {
            Destroy(instance);
        }

        UIelementList.Clear();
    }

    public void Activate()
    {
        character = GameManager.characterPool[SelectAbilityUIManager.UI_Info.currentPlayer];
        CreateUI();

        //"Spawn" object
        gameObject.transform.localScale = scale;
    }

    public void Deactivate()
    {
        foreach (GameObject instance in UIelementList)
        {
            Destroy(instance);
        }
        character = null;

        UIelementList.Clear();

        //"Despawn" object
        gameObject.transform.localScale = Vector3.zero;
    }
}
