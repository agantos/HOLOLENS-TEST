using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI_Info
{
    public string currentPlayer, currentTurnEconomy, currentTag;
}

public class SelectAbilityUIManager : MonoBehaviour
{
    public static TurnEconomyTabManager turnEconomyTabManager;
    public static AbilityTagTabManager abilityTagTabManager;
    public static AbilityTabManager abilityTabManager;
    public static ActivateAbilityUIManager activateAbilityUIManager;

    public static CharacterUI_Info UI_Info = new CharacterUI_Info();
    public static SelectAbilityUIManager instance;

    public static SelectAbilityUIManager GetInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SelectAbilityUIManager>();

                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("SingletonMonoBehaviour");
                    instance = singletonObject.AddComponent<SelectAbilityUIManager>();
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
        activateAbilityUIManager = FindAnyObjectByType<ActivateAbilityUIManager>();
    }

    static public void GiveTurnToPlayingCharacter()
    {
        UI_Info.currentPlayer = GameManager.GetCurrentPlayer_Name();

        turnEconomyTabManager.Deactivate();
        turnEconomyTabManager.Activate();

        abilityTagTabManager.Deactivate();

        abilityTabManager.Deactivate();

        activateAbilityUIManager.Deactivate();
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

    //New
    public static void OnClick_AbilityButton()
    {
        abilityTabManager.Deactivate();
        activateAbilityUIManager.Activate();
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
