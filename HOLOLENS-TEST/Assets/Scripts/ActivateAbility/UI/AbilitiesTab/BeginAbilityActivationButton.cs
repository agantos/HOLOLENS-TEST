using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Microsoft.MixedReality.Toolkit.UI;
using TMPro;

public class BeginAbilityActivationButton: MonoBehaviour
{
    public TextMeshPro text;
    public Interactable button;

    public void Initialize(string abilityName, string attackerName, GameObject spawnRadius)
    {
        
        text.text = abilityName;

        SetButtonOnClick(delegate {
            CastingAbilityManager.GetInstance().BeginAbilityActivation(abilityName, attackerName);
        });
    }

    public void SetButtonOnClick(UnityEngine.Events.UnityAction method)
    {
        button.OnClick.AddListener(method);
    }
}


