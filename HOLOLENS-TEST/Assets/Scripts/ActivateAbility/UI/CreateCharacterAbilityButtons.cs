using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCharacterAbilityButtons : MonoBehaviour
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
