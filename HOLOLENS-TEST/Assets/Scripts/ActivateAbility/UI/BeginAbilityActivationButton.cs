using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginAbilityActivationButton : Button
{
    public GameObject spawnRadius;
    public GameObject activateButtonPrefab;

    public string abilityName;
    public string attackerName;

    public void Initialize(string abilityName, string attackerName, GameObject spawnRadius, GameObject activateButtonPrefab)
    {
        this.abilityName = abilityName;
        this.attackerName = attackerName;
        this.spawnRadius = spawnRadius;
        this.activateButtonPrefab = activateButtonPrefab;

        gameObject.GetComponentInChildren<Text>().text = abilityName;

        onClick.AddListener(delegate {
            if (spawnRadius.GetComponent<CastingAbilityManager>().
                    ActivateSelection(AbilityManager.abilityPool[abilityName], GameManager.characterPool[attackerName])
            )
            {
                activateButtonPrefab.SetActive(true);
            }
        });
    }
}


