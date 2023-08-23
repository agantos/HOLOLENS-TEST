using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginAbilityActivationButton : Button
{
    public GameObject spawnRadius;

    public string abilityName;
    public string attackerName;

    public void Initialize(string abilityName, string attackerName, GameObject spawnRadius)
    {
        this.abilityName = abilityName;
        this.attackerName = attackerName;

        gameObject.GetComponentInChildren<Text>().text = abilityName;

        onClick.AddListener(delegate {
            if (spawnRadius.GetComponent<CastingAbilityManager>().
                    BeginCasting(AbilitiesManager.abilityPool[abilityName], GameManager.characterPool[attackerName])
            )
            {
                //Despawn window that displays the abilities
                FindAnyObjectByType<AbilityTabManager>(FindObjectsInactive.Include).Deactivate();

                //Spawn the buttons for activation and canceling of the ability
                FindAnyObjectByType<ActivateAbilityButton>(FindObjectsInactive.Include).Activate();
                FindAnyObjectByType<CancelAbilityButton>(FindObjectsInactive.Include).Activate();
            }
        });
    }
}


