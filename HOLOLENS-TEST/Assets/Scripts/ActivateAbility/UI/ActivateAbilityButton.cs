using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Singleton Button that activates current selected ability
public class ActivateAbilityButton : Button
{
    // Start is called before the first frame update
    void Start()
    {
        #if UNITY_EDITOR
                if (!UnityEditor.EditorApplication.isPlaying)
                    return;
        #endif

        onClick.AddListener(delegate {
            CastingAbilityManager.ActivateAbility();        
        });

        Deactivate();
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }
}
