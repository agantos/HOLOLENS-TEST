using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public enum AbiltyDisplayColors { CHARACTER_STAT, TURN_ECONOMY, DAMAGE, FAILURE, ABILITY, NORMAL }

public class AbilityDisplayManager : MonoBehaviour
{
    //Set in Editor
    public CostInfoManager costInfoManager;
    public EffectInfoManager effectInfoManager;
    public OtherStatsManager otherStatsManager;
    public SuccessInfoManager successInfoManager;
    public TMP_Text title;
    
    
    public static Ability displayingAbility;
    public static Dictionary<AbiltyDisplayColors, string> colorTypesDictionary = new Dictionary<AbiltyDisplayColors, string>();

    Vector3 localScale;

    // Start is called before the first frame update
    void Start()
    {
        localScale = new Vector3(1, 1, 1);
        colorTypesDictionary.Add(AbiltyDisplayColors.CHARACTER_STAT, "#65FF99");
        colorTypesDictionary.Add(AbiltyDisplayColors.TURN_ECONOMY, "#5BC4FF");
        colorTypesDictionary.Add(AbiltyDisplayColors.DAMAGE, "#E5D32D");
    }

    public void CreateUI(string abilityName) {        
        
        //Spawn UI by scaling it
        transform.localScale = localScale;

        //Set ability
        displayingAbility = AbilitiesManager.abilityPool[abilityName];
        title.text = displayingAbility.name;

        //Destroy Existing UI
        DestroyUI();

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

    void DestroyUI()
    {
        costInfoManager.DestroyUI();
        effectInfoManager.DestroyUI();
        otherStatsManager.DestroyUI();
        successInfoManager.DestroyUI();
    }

    public static string ColorString(string s, AbiltyDisplayColors type)
    {
        string coloredString = "<color=" + colorTypesDictionary[type] + ">";
        coloredString += s;
        coloredString += "</color>";
        return coloredString;
    }
    public static string BoldString(string s)
    {
        return "<b>" + s + "</b>";
    }

    public static string GetItalicString(string s)
    {
        return "<i>" + s + "</i>";
    }
}
