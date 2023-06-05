using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateAbilityButton : Button
{
    // Start is called before the first frame update
    void Start()
    {
        onClick.AddListener(delegate { 
            FindFirstObjectByType<RadiusSelectScript>().Activate();
            gameObject.SetActive(false);
        });
    }
}
