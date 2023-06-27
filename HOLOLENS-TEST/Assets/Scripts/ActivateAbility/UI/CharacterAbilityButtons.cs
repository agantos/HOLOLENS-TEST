using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Spawns the buttons for the abilities of a specific character 
// Is placed in the content of a scroll window
public class CharacterAbilityButtons : MonoBehaviour
{

    public GameObject SpawnRadius;
    public GameObject ActivateButtonPrefab;
    public GameObject BeginActivationButtonPrefab;

    public string characterName;
    // Start is called before the first frame update
    void Start()
    {
        foreach (string abilityName in GameManager.characterPool[characterName].abilities.Values)
        {
            GameObject instance = Instantiate(BeginActivationButtonPrefab, gameObject.transform);
            instance.name = abilityName + "_Button";           
            instance.GetComponentInChildren<BeginAbilityActivationButton>().Initialize(abilityName, characterName, SpawnRadius, ActivateButtonPrefab);
        }
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
