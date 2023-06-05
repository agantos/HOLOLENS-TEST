using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnRadiusButton : Button
{
    public GameObject SpawnRadius;
    public GameObject ActivateButtonPrefab;

    public string abilityName;
    public string attackerName;
    // Start is called before the first frame update
    void Start()
    {
        AreaOfEffectStats aoe = new AreaOfEffectStats(30, "cube", 10);
        onClick.AddListener(delegate { 
            SpawnRadius.GetComponent<CreateAOE>().InstantiateObject(AbilityManager.abilityPool[abilityName], ScriptTesting.characterPool[attackerName]);
            ActivateButtonPrefab.SetActive(true);
        });
    }

    // Update is called once per frame
    void Update()
    {

    }    
}


