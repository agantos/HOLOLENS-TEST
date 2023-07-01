using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Spawns the buttons for the abilities of a specific character 
// Is placed in the content of a scroll window
public class CharacterAbilityButtons : MonoBehaviour
{

    public GameObject SpawnRadius;
    public GameObject BeginActivationButtonPrefab;
    public List<GameObject> buttonInstances = new List<GameObject>();
    public string characterName;

    void Start()
    {
        SpawnCharacterAbilityButtons();
    }

    void SpawnCharacterAbilityButtons()
    {
        foreach (string abilityName in GameManager.characterPool[characterName].abilities.Values)
        {
            GameObject instance = Instantiate(BeginActivationButtonPrefab, gameObject.transform);
            instance.name = abilityName + "_Button";
            instance.GetComponentInChildren<BeginAbilityActivationButton>().Initialize(abilityName, characterName, SpawnRadius);
            buttonInstances.Add(instance);
        }
    }

    public void OnChangeTurn(string newCharName)
    {
        foreach(GameObject instance in buttonInstances){
            Destroy(instance);
        }

        buttonInstances.Clear();
        
        characterName = newCharName;
        SpawnCharacterAbilityButtons();
    }

    public void Activate()
    {
        gameObject.transform.parent.transform.parent.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.transform.parent.transform.parent.gameObject.SetActive(false);
    }
}
