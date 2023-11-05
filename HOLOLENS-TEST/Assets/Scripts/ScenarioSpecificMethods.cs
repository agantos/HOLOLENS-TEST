using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioSpecificMethods
{

    static ScenarioSpecificMethods Instance = null;
    private ScenarioSpecificMethods() { }

    public static ScenarioSpecificMethods GetInstance()
    {
        if (Instance == null)
        {
            Instance = new ScenarioSpecificMethods();
        }

        return Instance;
    }

    // At the start of each turn the characters:
    //  - Restore 1 power point
    //  - Gain their speed back
    public void Character_onStartTurn(Character c)
    {
        //Heal speed damage that is applied on movement
        CharacterStat speed = c.GetStat("Speed");
        speed.HealDamage(speed.GetDamage());
        speed.CalculateCurrentValue();

        //Every turn a character gets 1 power point
        CharacterStat powerPoints = c.GetStat("Power Points");
        powerPoints.HealDamage(1);
        powerPoints.CalculateCurrentValue();
    }

    //At the start of the game the characters have no powerpoints
    public void Character_onInitialize(Character c)
    {
        CharacterStat powerPoints = c.GetStat("Power Points");
        powerPoints.DealDamage(powerPoints.GetCurrentValue());
        powerPoints.CalculateCurrentValue();
    }

    //Initializing Dialog for characters
    public void InitializeCharacterDialog(CharacterDialogue d)
    {
        d.CreateCategoryFromFolder("onMove", "onMove", 100);
        d.CreateCategoryFromFolder("onStartTurn", "onStartTurn", 100);
        d.CreateCategoryFromFolder("onGettingHit", "onGettingHit", 100);
        d.CreateCategoryFromFolder("onGettingDebuffed", "onGettingDebuffed", 100);
        d.CreateCategoryFromFolder("onGettingBuffed", "onGettingBuffed", 100);
        d.CreateCategoryFromFolder("onGettingHealed", "onGettingHealed", 100);
        d.CreateCategoryFromFolder("onGettingPeekedOn", "onGettingPeekedOn", 100);
        d.CreateCategoryFromFolder("onDodge", "onDodge", 100);
        d.CreateCategoryFromFolder("onDeath", "onDeath", 100);
        d.CreateCategoryFromFolder("onMaxSpecialPoints", "onMaxSpecialPoints", 100);
        d.CreateCategoryFromFolder("onKill", "onKill", 100);
    }

    //Checking Death condition
    public bool ShouldDie(Character c)
    {
        return c.GetStat("HP").GetCurrentValue() <= 0;
    }

    //Play OnStartTurn Audio
    public void PlayDialogueOnStartTurn(string charName)
    {
        DialogueManager.Instance.PlayDialogue(charName, "onStartTurn", 100);
    }

    //Play OnMove Audio
    public void PlayDialogueOnMove(string charName, Character c)
    {
        if(c.GetStat("Speed").GetDamage() == 0)
            DialogueManager.Instance.PlayDialogue(charName, "onMove", 100);
        else
            DialogueManager.Instance.PlayDialogue(charName, "onMove", 20);
    }
}
