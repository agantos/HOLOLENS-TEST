using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//Singleton Button that activates current selected ability
public class ActivateAbilityButton : Button
{
    public void Deactivate()
    {
        gameObject.transform.localScale = new Vector3(0, 0, 0);
    }

    public void Activate()
    {
        gameObject.transform.localScale = new Vector3(1, 1, 1);
    }

    public void ActivateAbility()
    {
        //Deactivate any spawned objects related to the activation of the ability
        CastingAbilityManager.GetInstance().DeactivateAbilityActivationObjects();

        //Start Animation
        CastingAbilityManager.GetInstance().attackerAnimationManager.IdleTo_Animation(CastingAbilityManager.GetInstance().abilityToCast.animationTypes.attacker);

        //Activate Ability
        StartCoroutine(ActivateAbilityAfterAnimation());
    }

    IEnumerator ActivateAbilityAfterAnimation()
    {
        //Wait for the animation to register
        float secsForAnimationToRegister = 0.4f;
        yield return new WaitForSeconds(secsForAnimationToRegister);
        float animationEnds = CastingAbilityManager.GetInstance().attackerAnimationManager.GetCurrentAnimationDuration() 
                                - secsForAnimationToRegister;
        
        //Wait for the Impact point of the animation to activate the ability
        yield return new WaitForSeconds(animationEnds-animationEnds*3/8);
        CastingAbilityManager.GetInstance().ActivateAttackerAbility();
        CastingAbilityManager.GetInstance().ActivateDefenderAnimations();

        //Wait for the animation to end
        yield return new WaitForSeconds(animationEnds-animationEnds*5/8);

        //Switch Back to Idle Animation
        CastingAbilityManager.GetInstance().ReturnAttackerToIdleAnimation();
        CastingAbilityManager.GetInstance().ReturnDefendersToIdleAnimation();

        //Clean manager state
        CastingAbilityManager.GetInstance().CleanState();

        //Spawn the window that displays the abilities
        SelectAbilityUIManager.GiveTurnToPlayingCharacter();
    }
}
