using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterInfoDisplayManager : MonoBehaviour
{
    //Parent Root GameObject
    public GameObject root;

    //Content Managers set in UnityEditor
    public BarsInfoDisplayManager barsInfoDisplayManager;
    public BaseStatsManager baseStatsManager;
    public KeyAbilitiesManager keyAbilitiesManager;
    public StatCategoriesManager statCategoriesManager;

    //UI Elements
    public TMP_Text title;

    //Other Variables
    Character displayingCharacter;

    
    // Start is called before the first frame update
    void Start()
    {
    
    }

    public void CreateUI(string characterName)
    {
        displayingCharacter = GameManager.GetInstance().characterPool[characterName];
        title.text = characterName;

        barsInfoDisplayManager.CreateUI(displayingCharacter);
        baseStatsManager.CreateUI(displayingCharacter);
        keyAbilitiesManager.CreateUI(displayingCharacter);
        statCategoriesManager.CreateUI(displayingCharacter);
    }

    public void ClearUI()
    {
        barsInfoDisplayManager.ClearUI();
        baseStatsManager.ClearUI();
        keyAbilitiesManager.ClearUI();
        statCategoriesManager.ClearUI();
    }

    public void OnDestroy()
    {
        ClearUI();
    }

    public void DestroyPage()
    {
        foreach (Transform child in root.transform)
        {
            CharacterInfoObjectsManager.instance.DestroyCharacterInfo(displayingCharacter.name);
            Destroy(child.gameObject);
            Destroy(this.gameObject);
        }
    }
}
