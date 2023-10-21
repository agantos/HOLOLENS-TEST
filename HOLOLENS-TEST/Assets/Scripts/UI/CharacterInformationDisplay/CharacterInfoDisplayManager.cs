using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    private ScrollRect scrollRect;


    // Start is called before the first frame update
    void Start()
    {
        scrollRect = gameObject.GetComponentInParent<ScrollRect>();
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
        CharacterInfoObjectsManager.Instance.OnDestroyCharacterInformationObject(displayingCharacter.name);
    }

    public void DestroyPage()
    {
        foreach (Transform child in root.transform)
        {
            CharacterInfoObjectsManager.Instance.OnDestroyCharacterInformationObject(displayingCharacter.name);
            Destroy(child.gameObject);
            Destroy(this.gameObject);
        }
    }

    public void ScrollUp()
    {
        if (scrollRect.verticalNormalizedPosition >= 0.8f)
            scrollRect.verticalNormalizedPosition = 1;
        else
            scrollRect.verticalNormalizedPosition += 0.2f;
    }

    public void ScrollDown()
    {
        if (scrollRect.verticalNormalizedPosition >= 0.2f)
            scrollRect.verticalNormalizedPosition -= 0.2f;
        else
            scrollRect.verticalNormalizedPosition = 0f;
    }
}
