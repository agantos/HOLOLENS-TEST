using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginAbilityActivationButton : Button
{
    public void Initialize(string abilityName, string attackerName, GameObject spawnRadius)
    {
        gameObject.GetComponentInChildren<Text>().text = abilityName;

        SetButtonOnClick(delegate {
            CastingAbilityManager.instance.
                BeginAbilityActivation( AbilitiesManager.abilityPool[abilityName], 
                                        GameManager.GetInstance().characterPool[attackerName]
                );
            });
    }

    public void SetButtonOnClick(UnityEngine.Events.UnityAction method)
    {
        onClick.AddListener(method);
    }
}


