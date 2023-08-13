using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI_Info
{
    public string currentPlayer, currentTurnEconomy, currentTag;
}

public class CharacterUIManager : MonoBehaviour
{
    public static TurnEconomyTabManager turnEconomyTabManager;
    public static AbilityTagTabManager abilityTagTabManager;
    public static AbilityTabManager abilityTabManager;

    public static CharacterUI_Info UI_Info = new CharacterUI_Info();
    public static CharacterUIManager instance;

    public static CharacterUIManager GetInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CharacterUIManager>();

                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("SingletonMonoBehaviour");
                    instance = singletonObject.AddComponent<CharacterUIManager>();
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        turnEconomyTabManager = FindAnyObjectByType<TurnEconomyTabManager>();
        abilityTagTabManager = FindAnyObjectByType<AbilityTagTabManager>();
        abilityTabManager = FindAnyObjectByType<AbilityTabManager>();      
    }

    static public void GiveTurnToPlayingCharacter()
    {
        UI_Info.currentPlayer = GameManager.GetCurrentPlayer_Name();
        Debug.Log(GameManager.GetCurrentPlayer_Name());

        turnEconomyTabManager.Deactivate();
        turnEconomyTabManager.Activate();

        abilityTagTabManager.Deactivate();
        abilityTabManager.Deactivate();
    }

    public static void OnClick_TurnEconomyButton(TurnEconomyUIElement element)
    {
        UI_Info.currentTurnEconomy = element.turnEconomyName;

        turnEconomyTabManager.Deactivate();
        abilityTagTabManager.Activate();
    }

    public static void OnClick_AbilityTagButton(AbilityTagElement element)
    {
        UI_Info.currentTag = element.abilityTag;

        abilityTagTabManager.Deactivate();
        abilityTabManager.Activate();
    }

    public static void OnClick_BackButton_AbilityTagTab()
    {
        abilityTagTabManager.Deactivate();
        turnEconomyTabManager.Activate();
    }

    public static void OnClick_BackButton_AbilityTab()
    {
        abilityTabManager.Deactivate();
        abilityTagTabManager.Activate();
    }


}
