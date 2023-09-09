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
    public bool SelectAbility;
    public Character attacker;
    public AnimationManager attackerAnimationManager;

    public List<Character> defenderCharacters = new List<Character>();
    public List<GameObject> defendersGameObject = new List<GameObject>();
    
    public List<bool> abilitySuccessList = new List<bool>();
    public Ability abilityToCast;

    public AbilitySelectType CurrentSelectionType = AbilitySelectType.INACTIVE;

    GameObject spawned;

    public static CastingAbilityManager Instance { get; private set; }

    public static CastingAbilityManager GetInstance()
    {
        return Instance;
    }

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

    //Spawns the necessary objects for selecting targets for an ability
    public bool BeginAbilityActivation(string abilityName, string attackerName)
    {
        Ability ability = AbilitiesManager.abilityPool[abilityName];
        Character attacker = GameManager.GetInstance().characterPool[attackerName];


        //Check if the ability can be cast
        if (true/**AbilityManager.Activate_CheckCost(ability.name, attacker)**/)
        {
            //Set the parameters for the casting of the ability
            GetInstance().attacker = attacker;
            GetInstance().abilityToCast = ability;
            GetInstance().attackerAnimationManager = GameManager.GetInstance().playingCharacterGameObjects[attacker.name].GetComponent<AnimationManager>();

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

    // START OF METHODS FOR RPC CALLS
    public void SyncManagerData(string ablityToCast, string attackerName, string[] defenders)
    {
        //Set the parameters for the casting of the ability
        GetInstance().attacker = GameManager.GetInstance().characterPool[attackerName];
        GetInstance().abilityToCast = AbilitiesManager.abilityPool[ablityToCast];
        GetInstance().attackerAnimationManager = GameManager.GetInstance().playingCharacterGameObjects[attackerName].GetComponent<AnimationManager>();
        
        foreach(string defender in defenders)
        {
            GetInstance().defenderCharacters.Add(GameManager.GetInstance().characterPool[defender]);
            GetInstance().defendersGameObject.Add(GameManager.GetInstance().playingCharacterGameObjects[defender]);
        }
    }

    public string[] GetDefenderNameList()
    {
        List<string> list = new List<string>();
        foreach (Character defender in defenderCharacters)
        {
            list.Add(defender.name);
        }

        return list.ToArray();
    }

    public void ActivateAbilityRemotely()
    {
        //Start Animation
        if (defenderCharacters.Count > 0)
            FaceDirection();

        attackerAnimationManager.IdleTo_Animation(abilityToCast.animationTypes.attacker);

        //Activate Ability
        Instance.StartCoroutine(ActivationSyncAbility());
    }

    // END

    //DEPRECATE
    public void ActivateAttackerAbility()
    {
        attacker.ActivateAbility(abilityToCast.name, out abilitySuccessList, defenderCharacters, attacker);
    }

    public void ActivateAbility()
    {
        //Sync AbilityManager with all the others
        MultiplayerCallsAbilityCast.Instance.Propagate_AbilityManagerSync(abilityToCast.name, attacker.name, GetDefenderNameList());

        //Deactivate any spawned objects related to the activation of the ability
        DeactivateAbilityActivationObjects();

        //Start Animation
        if(defenderCharacters.Count > 0)
            FaceDirection();

        attackerAnimationManager.IdleTo_Animation(abilityToCast.animationTypes.attacker);

        //Activate Ability
        Instance.StartCoroutine(ActivationSyncAbility());

        //Send for other players to Activate
        MultiplayerCallsAbilityCast.Instance.Propagate_RemoteActivateAbility();
    }

    public void CancelActivation()
    {
        DeactivateAbilityActivationObjects();
        CleanState();

        FindAnyObjectByType<AbilityTabManager>(FindObjectsInactive.Include).Activate();
    }

    //Deactivate Spawned Objects
    public void DeactivateAbilityActivationObjects()
    {
        //Objects searched with ANYobjecttype are singletons
        FindAnyObjectByType<AbilityRangeDisplay>().Deactivate();

        if (CurrentSelectionType == AbilitySelectType.SHAPE)
        {
            FindAnyObjectByType<RadiusSelectScript>().OnAbilityActivate();
        }
        else if (CurrentSelectionType == AbilitySelectType.SELECT)
        {
            foreach (GameObject defender in defendersGameObject)
            {
                defender.GetComponent<SelectUnitManager>().OnAbilityResolved();
            }            
        }
    }

    //Methods for Animation
    public void ReturnAttackerToIdleAnimation()
    {
        attackerAnimationManager.Animation_ToIdle(abilityToCast.animationTypes.attacker);
    }

    public void ActivateDefenderAnimations()
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

    public void ReturnDefendersToIdleAnimation()
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

    void FaceDirection()
    {
        float rotSpeed = 360f;
        Transform attackerTransform = GameManager.GetInstance().playingCharacterGameObjects[attacker.name].transform;
        Transform defenderTransform = defendersGameObject[0].transform;

        //When on target -> dont rotate!
        if ((defenderTransform.position - attackerTransform.position).magnitude < 0.1f) return;

        Vector3 direction = (defenderTransform.position - attackerTransform.transform.position).normalized;
        Quaternion qDir = Quaternion.LookRotation(direction);
        attackerTransform.transform.rotation = Quaternion.Slerp(attackerTransform.transform.rotation, qDir, Time.deltaTime * rotSpeed);
    }

    //Cleans Manager State
    public void CleanState()
    {
        abilityToCast = null;

        attacker = null;

        defenderCharacters.Clear();
        defendersGameObject.Clear();

        abilitySuccessList.Clear();
        CurrentSelectionType = AbilitySelectType.INACTIVE;
    }

    IEnumerator ActivationSyncAbility()
    {
        //Wait for the animation to register
        float secsForAnimationToRegister = 0.4f;
        yield return new WaitForSeconds(secsForAnimationToRegister);
        float animationEnds = attackerAnimationManager.GetCurrentAnimationDuration()
                                - secsForAnimationToRegister;

        //Wait for the Impact point of the animation to activate the ability
        yield return new WaitForSeconds(animationEnds - animationEnds * 3 / 8);
        attacker.ActivateAbility(abilityToCast.name, out abilitySuccessList, defenderCharacters, attacker);
        ActivateDefenderAnimations();

        //Wait for the animation to end
        yield return new WaitForSeconds(animationEnds - animationEnds * 5 / 8);

        //Switch Back to Idle Animation
        ReturnAttackerToIdleAnimation();
        ReturnDefendersToIdleAnimation();

        //Clean manager state
        CleanState();

        //Spawn the window that displays the abilities
        SelectAbilityUIManager.GiveTurnToPlayingCharacter();
    }
}
