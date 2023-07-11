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
        CastingAbilityManager.attackerAnimationManager.IdleTo_Animation(CastingAbilityManager.abilityToCast.animationType);

        //Activate Ability
        StartCoroutine(ActivateAbilityAfterAnimation());
    }

    IEnumerator ActivateAbilityAfterAnimation()
    {
        //Wait for the animation to end
        float secsForAnimationToRegister = 0.4f;
        yield return new WaitForSeconds(secsForAnimationToRegister);
        yield return new WaitForSeconds(CastingAbilityManager.attackerAnimationManager.GetCurrentAnimationDuration() - secsForAnimationToRegister);

        //Switch Back to Idle Animation
        CastingAbilityManager.attackerAnimationManager.Animation_ToIdle(AnimationType.Attack_Melee_Spell);
        
        //Cast Ability
        CastingAbilityManager.ActivateAttackerAbility();

        //Clean manager state
        CastingAbilityManager.CleanState();

        //Spawn the window that displays the abilities
        FindAnyObjectByType<CharacterAbilityButtons>(FindObjectsInactive.Include).Activate();
    }
}
