using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI_Info
{
    public string currentPlayer, currentTurnEconomy, currentTag, currentAbility;
}

public class SelectAbilityUIManager : MonoBehaviour
{

    public ActivateAbilityUIManager activateAbilityUIManager;
    public TurnEconomyTab turnEconomyTab;
    public AbilityTagTab abilityTagTab;
    public AbilitiesTab abilitiesTab;


    public CharacterUI_Info UI_Info = new CharacterUI_Info();
    public static SelectAbilityUIManager Instance { get; private set; }

    private void Awake()
    {
        // Ensure only one instance of GameManager exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate GameManager objects
        }
    }

    public void GiveTurnToPlayingCharacter()
    {
        if(GameManager.GetInstance().player == GameManager.GetInstance().GetCurrentPlayer_Character().player)
        {
            UI_Info.currentPlayer = GameManager.GetInstance().GetCurrentPlayer_Name();

            turnEconomyTab.ClearUI();
            abilitiesTab.ClearUI();
            abilityTagTab.ClearUI();

            turnEconomyTab.CreateUI();

            turnEconomyTab.gameObject.SetActive(false);
            turnEconomyTab.gameObject.SetActive(true);

            abilityTagTab.gameObject.SetActive(false);

            abilitiesTab.gameObject.SetActive(false);

            activateAbilityUIManager.Deactivate();            
        }
        else
        {
            turnEconomyTab.gameObject.SetActive(false);
            abilityTagTab.gameObject.SetActive(false);
            abilitiesTab.gameObject.SetActive(false);
            activateAbilityUIManager.Deactivate();
        }
    }

    public void OnClick_TurnEconomyButton(TurnEconomyUIElement element)
    {
        UI_Info.currentTurnEconomy = element.nameText.text;

        turnEconomyTab.gameObject.SetActive(false);
        turnEconomyTab.ClearUI();

        abilityTagTab.gameObject.SetActive(true);
        abilityTagTab.CreateUI();
    }

    public void OnClick_AbilityTagButton(AbilityTagElement element)
    {
        UI_Info.currentTag = element.nameText.text;

        abilityTagTab.gameObject.SetActive(false);
        abilityTagTab.ClearUI();

        abilitiesTab.gameObject.SetActive(true);
        abilitiesTab.CreateUI();
    }

    public void OnClick_AbilityButton(string abilityName)
    {
        UI_Info.currentAbility = abilityName;

        abilitiesTab.gameObject.SetActive(false);
        activateAbilityUIManager.Activate();
    }

    public void OnClick_BackButton_AbilityTagTab()
    {
        abilityTagTab.ClearUI();
        abilityTagTab.gameObject.SetActive(false);

        turnEconomyTab.gameObject.SetActive(true);
        turnEconomyTab.CreateUI();
    }

    public void OnClick_BackButton_AbilityTab()
    {
        abilitiesTab.ClearUI();
        abilitiesTab.gameObject.SetActive(false);

        abilityTagTab.gameObject.SetActive(true);
        abilityTagTab.CreateUI();        
    }    
}
