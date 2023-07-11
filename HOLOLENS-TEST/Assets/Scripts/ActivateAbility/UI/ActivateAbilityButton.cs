using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//Singleton Button that activates current selected ability
public class ActivateAbilityButton : Button
{
    // Start is called before the first frame update
    void Start()
    {
        #if UNITY_EDITOR
                if (!UnityEditor.EditorApplication.isPlaying)
                    return;
        #endif

        onClick.AddListener(delegate {
            ActivateAbility();
        });
        
        Deactivate();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ActivateAbility();
    }

    public void Deactivate()
    {
        gameObject.transform.localScale = new Vector3(0, 0, 0);
        //gameObject.SetActive(false);
    }

    public void Activate()
    {
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        //gameObject.SetActive(true);
    }

    public void ActivateAbility()
    {
        //Deactivate any spawned objects related to the activation of the ability
        CastingAbilityManager.DeactivateAbilityActivationObjects();

        //Start Animation
        CastingAbilityManager.attackerAnimationManager.IdleTo_Animation(CastingAbilityManager.abilityToCast.animationTypes.attacker);

        //Activate Ability
        StartCoroutine(ActivateAbilityAfterAnimation());
    }

    IEnumerator ActivateAbilityAfterAnimation()
    {
        //Wait for the animation to register
        float secsForAnimationToRegister = 0.4f;
        yield return new WaitForSeconds(secsForAnimationToRegister);
        float animationEnds = CastingAbilityManager.attackerAnimationManager.GetCurrentAnimationDuration() 
                                - secsForAnimationToRegister;
        
        //Wait for the Impact point of the animation to activate the ability
        yield return new WaitForSeconds(animationEnds-animationEnds*3/8);
        CastingAbilityManager.ActivateAttackerAbility();
        CastingAbilityManager.ActivateDefenderAnimations();

        //Wait for the animation to end
        yield return new WaitForSeconds(animationEnds-animationEnds*5/8);

        //Switch Back to Idle Animation
        CastingAbilityManager.ReturnAttackerToIdleAnimation();
        CastingAbilityManager.ReturnDefendersToIdleAnimation();

        //Clean manager state
        CastingAbilityManager.CleanState();

        //Spawn the window that displays the abilities
        FindAnyObjectByType<CharacterAbilityButtons>(FindObjectsInactive.Include).Activate();
    }
}
