using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterInfoDisplayManager : MonoBehaviour
{
    //Other Managers
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
        displayingCharacter = GameManager.characterPool[characterName];
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
}
