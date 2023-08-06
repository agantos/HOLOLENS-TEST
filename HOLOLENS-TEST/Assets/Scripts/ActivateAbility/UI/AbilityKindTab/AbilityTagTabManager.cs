using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class AbilityTagTabManager : MonoBehaviour
{
    
    public TMP_Text         title;
    public GameObject       Container;
    public GameObject       AbilityKindElementPrefab;
    public GameObject       BackButton;
    
    Character character;
    List<GameObject> UIelementList = new List<GameObject>();

    Vector3 scale;

    void Start()
    {
        BackButton.GetComponent<Button>().onClick.AddListener(CharacterUIManager.OnClick_BackButton_AbilityTagTab);
        scale = gameObject.transform.localScale;
    }

    public void Activate()
    {
        character = GameManager.characterPool[CharacterUIManager.UI_Info.currentPlayer];

        //"Spawn" object
        gameObject.transform.localScale = scale;

        SetTitle(CharacterUIManager.UI_Info.currentTurnEconomy);
        CreateUI();        
    }

    public void CreateUI()
    {
        HashSet<string> tags = new HashSet<string>();
        
        //Foreach ability that is the currently selected turn economy add its tags to the hash set
        foreach(string abilityName in character.abilities.Values)
        {
            Ability ability = character.GetCharacterAbility(abilityName);
            if (ability.turnEconomyCost.ContainsKey(CharacterUIManager.UI_Info.currentTurnEconomy))
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
            GameObject element = Instantiate(AbilityKindElementPrefab, Container.transform);
            UIelementList.Add(element);

            AbilityTagElement abilityTagElementUI = element.GetComponent<AbilityTagElement>();

            abilityTagElementUI.SetNameGameobject(tag);
            abilityTagElementUI.abilityTag = tag;
            abilityTagElementUI.SetButtonOnClick(() => { CharacterUIManager.OnClick_AbilityTagButton(abilityTagElementUI); });
        }
    }

    public void SetTitle(string title)
    {
        this.title.SetText(title);
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
