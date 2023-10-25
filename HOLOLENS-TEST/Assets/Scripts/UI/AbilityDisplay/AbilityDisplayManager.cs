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
    
    
    public Ability displayingAbility;
    public static Dictionary<AbilityDisplayColors, string> colorTypesDictionary = new Dictionary<AbilityDisplayColors, string>();

    Vector3 localScale = new Vector3(1, 1, 1);

    public void CreateUI(string abilityName, string characterName = "") {        
        
        //Spawn UI by scaling it
        transform.localScale = localScale;

        //Set ability
        displayingAbility = AbilitiesManager.GetInstance().abilities[abilityName];
        title.text = displayingAbility.name;

        //Set Character
        Character referencedCharacter = null;        
        if(characterName != "")
            referencedCharacter = GameManager.GetInstance().characterPool[characterName];

        //Clear Existing UI
        ClearUI();

        //Populate UI Information
        costInfoManager.CreateUI(displayingAbility, referencedCharacter);
        effectInfoManager.CreateUI(displayingAbility, referencedCharacter);
        otherStatsManager.CreateUI(displayingAbility, referencedCharacter);
        successInfoManager.CreateUI(displayingAbility, referencedCharacter);
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
