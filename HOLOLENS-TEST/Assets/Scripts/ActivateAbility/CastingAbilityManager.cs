using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;


public enum AbilitySelectType { SHAPE, SELECT, INACTIVE }

public class CastingAbilityManager : MonoBehaviour
{
    //Prefabs to set in editor
    public GameObject radiusMoveOnTouch;
    public GameObject CubeSelectPrefab;
    public GameObject CircleSelectPrefab;
    
    //Variables for casting the ability
    public static bool SelectAbility;
    public static Character attacker;
    public static AnimationManager attackerAnimationManager;

    public static List<Character> defenderCharacters = new List<Character>();
    public static List<GameObject> defendersGameObject = new List<GameObject>();
    public static List<bool> abilitySuccessList = new List<bool>();
    public static List<AnimationManager> defendersAnimationManager = new List<AnimationManager>();
    public static Ability abilityToCast;

    public static AbilitySelectType CurrentSelectionType = AbilitySelectType.INACTIVE;

    GameObject spawned;

    //Spawns the necessary objects for selecting targets for an ability
    public bool BeginCasting(Ability ability, Character attacker)
    {
        //Check if the ability can be cast
        if (true/**AbilityManager.Activate_CheckCost(ability.name, attacker)**/)
        {
            //Set the parameters for the casting of the ability
            CastingAbilityManager.attackerAnimationManager = GameManager.characterGameObjects[attacker.name].GetComponent<AnimationManager>();
            CastingAbilityManager.attacker = attacker;
            CastingAbilityManager.abilityToCast = ability;

            //Perform the appropriate operations for selection
            switch (ability.effects[0].areaOfEffect.shape)
            {
                case AreaShape.CUBE:
                    spawned = Instantiate(CubeSelectPrefab);
                    CurrentSelectionType = AbilitySelectType.SHAPE;

                    break;
                case AreaShape.CONE:
                    break;
                case AreaShape.SPHERE:
                    break;
                case AreaShape.LINE:
                    break;                
                case AreaShape.CIRCLE:
                    spawned = Instantiate(CircleSelectPrefab);
                    CurrentSelectionType = AbilitySelectType.SHAPE;
                    break;
                case AreaShape.SELECT:
                    CurrentSelectionType = AbilitySelectType.SELECT;
                    radiusMoveOnTouch.GetComponent<AbilityRangeDisplay>().Activate();
                    return true;
            }       

            //Size the spawned selector
            spawned.GetComponent<RadiusSelectScript>().radius = ability.effects[0].areaOfEffect.radius;
            spawned.GetComponent<RadiusSelectScript>().SetScale();

            //Set the spawned selector's position
            spawned.transform.SetParent(this.transform.parent.transform, false);
            spawned.transform.localPosition = this.transform.localPosition;

            //Set the AbilityRangeDisplay
            radiusMoveOnTouch.SetActive(true);
            radiusMoveOnTouch.GetComponent<MoveGameobjectToTouchPoint>().movee = spawned;
            radiusMoveOnTouch.GetComponent<AbilityRangeDisplay>().Activate();

            return true;          
        }
        else
        {
            Debug.Log(attacker.name + " cannot cast ability " + ability.name);
            return false;
        }            
    }

    public static void ActivateAttackerAbility()
    {
        attacker.ActivateOwnedAbility(abilityToCast.name, out abilitySuccessList, defenderCharacters, attacker);
    }

    public static void DeactivateAbility()
    {
        DeactivateAbilityActivationObjects();
        CleanState();

        FindAnyObjectByType<AbilityTabManager>(FindObjectsInactive.Include).Activate();
    }

    //Deactivate Spawned Objects
    public static void DeactivateAbilityActivationObjects()
    {
        //Objects searched with ANYobjecttype are singletons
        FindAnyObjectByType<AbilityRangeDisplay>().Deactivate();
        FindAnyObjectByType<CancelAbilityButton>().Deactivate();
        FindAnyObjectByType<ActivateAbilityButton>().Deactivate();

        if (CastingAbilityManager.CurrentSelectionType == AbilitySelectType.SHAPE)
        {
            FindAnyObjectByType<RadiusSelectScript>().OnAbilityActivate();
        }
        else if (CastingAbilityManager.CurrentSelectionType == AbilitySelectType.SELECT)
        {
            foreach (GameObject defender in defendersGameObject)
            {
                defender.GetComponent<SelectUnitManager>().OnAbilityActivate();
            }
        }
    }

    public static void ReturnAttackerToIdleAnimation()
    {
        attackerAnimationManager.Animation_ToIdle(abilityToCast.animationTypes.attacker);
    }

    public static void ActivateDefenderAnimations()
    {
        Assert.AreEqual(abilitySuccessList.Count, defendersGameObject.Count);
        for (int i = 0; i < abilitySuccessList.Count; i++)
        {
            if (abilitySuccessList[i])
            {
                defendersGameObject[i].GetComponent<AnimationManager>()
                    .IdleTo_Animation(abilityToCast.animationTypes.defender_AbilitySucceeds);
            }
            else
            {
                defendersGameObject[i].GetComponent<AnimationManager>()
                    .IdleTo_Animation(abilityToCast.animationTypes.defender_AbilityFails);
            }
        }
    }

    public static void ReturnDefendersToIdleAnimation()
    {
        Assert.AreEqual(abilitySuccessList.Count, defendersGameObject.Count);
        for (int i = 0; i < abilitySuccessList.Count; i++)
        {
            if (abilitySuccessList[i])
            {
                defendersGameObject[i].GetComponent<AnimationManager>()
                    .Animation_ToIdle(abilityToCast.animationTypes.defender_AbilitySucceeds);
            }
            else
            {
                defendersGameObject[i].GetComponent<AnimationManager>()
                    .Animation_ToIdle(abilityToCast.animationTypes.defender_AbilityFails);
            }
        }
    }

    //Cleans Manager State
    public static void CleanState()
    {
        abilityToCast = null;
        attacker = null;
        defenderCharacters.Clear();
        defendersGameObject.Clear();
        abilitySuccessList.Clear();
        CurrentSelectionType = AbilitySelectType.INACTIVE;
    }
}
