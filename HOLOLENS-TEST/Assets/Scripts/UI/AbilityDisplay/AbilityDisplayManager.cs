using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AbilityDisplayManager : MonoBehaviour
{
    //Set in Editor
    public CostInfoManager costInfoManager;
    public EffectInfoManager effectInfoManager;
    public OtherStatsManager otherStatsManager;
    public SuccessInfoManager successInfoManager;
    public TMP_Text title;
    
    
    public static Ability displayingAbility;
    public static Dictionary<AbilityDisplayColors, string> colorTypesDictionary = new Dictionary<AbilityDisplayColors, string>();

    Vector3 localScale = new Vector3(1, 1, 1);

    public void CreateUI(string abilityName) {        
        
        //Spawn UI by scaling it
        transform.localScale = localScale;

        //Set ability
        displayingAbility = AbilitiesManager.GetInstance().abilities[abilityName];
        title.text = displayingAbility.name;

        //Clear Existing UI
        ClearUI();

        //Populate UI Information
        costInfoManager.CreateUI(displayingAbility);
        effectInfoManager.CreateUI(displayingAbility);
        otherStatsManager.CreateUI(displayingAbility);
        successInfoManager.CreateUI(displayingAbility);
    }

    void DespawnUI()
    {
        localScale = transform.localScale;
        transform.localScale = Vector3.zero;      
    }

    public void ClearUI()
    {
        costInfoManager.ClearUI();
        effectInfoManager.ClearUI();
        otherStatsManager.ClearUI();
        successInfoManager.ClearUI();
    }

    public void DestroyPage()
    {
        foreach (Transform child in gameObject.transform)
        {
            AbilityDisplayObjectsManager.instance.DestroyAbilityDisplayInfo(displayingAbility.name);
            Destroy(child.gameObject);
            Destroy(this.gameObject);
        }
    }
}
