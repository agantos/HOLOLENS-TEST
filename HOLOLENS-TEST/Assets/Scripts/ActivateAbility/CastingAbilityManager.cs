using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class CastingAbilityManager : MonoBehaviour
{
    //Prefabs to set in editor
    public GameObject radiusMoveOnTouch;
    public GameObject CubeSelectPrefab;
    public GameObject CircleSelectPrefab;
    public GameObject LineSelectPrefab;
    
    //Variables for casting the ability
    public bool SelectAbility;
    public Character attacker;
    public AnimationManager attackerAnimationManager;
    public List<EffectApplicationData> applicationData;

    public List<Character> defenderCharacters = new List<Character>();
    public List<GameObject> defendersGameObject = new List<GameObject>();
    
    public List<bool> abilitySucceedsOnDefendersList = new List<bool>();
    public Ability abilityToCastInformation;
    public AbilityPresentation abilityToCastPresentation;

    public AbilitySelectType CurrentSelectionType = AbilitySelectType.INACTIVE;

    GameObject radiusSelect;
    Vector3 radiusSelectPosition;
    Vector3 radiusSelectRotation;

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
        Ability abilityInformation = AbilitiesManager.GetInstance().abilities[abilityName];
        AbilityPresentation abilityPresentation = AbilitiesManager.GetInstance().abilitiesPresentation[abilityName];
        Character attacker = GameManager.GetInstance().characterPool[attackerName];


        //Check if the ability can be cast
        if (true/**AbilityManager.Activate_CheckCost(ability.name, attacker)**/)
        {
            //Cancel a selection of move if it had happened
            ConfirmMoveCanvas cm = FindFirstObjectByType<ConfirmMoveCanvas>();
            if (cm != null)
                cm.CancelMovement();

            //Disable movements
            CharacterMoveManager.Instance.isMovementAllowed = false;


            //Set the parameters for the casting of the ability
            GetInstance().attacker = attacker;
            GetInstance().abilityToCastInformation = abilityInformation;
            GetInstance().abilityToCastPresentation = abilityPresentation;
            GetInstance().attackerAnimationManager = GameManager.GetInstance().playingCharacterGameObjects[attacker.name].GetComponent<AnimationManager>();

            //Perform the appropriate operations for selection
            switch (abilityInformation.effects[0].areaOfEffect.shape)
            {
                case AreaShape.CUBE:
                    radiusSelect = Instantiate(CubeSelectPrefab);
                    CurrentSelectionType = AbilitySelectType.SHAPE;

                    break;
                case AreaShape.CONE:
                    break;
                case AreaShape.SPHERE:
                    break;
                case AreaShape.LINE:
                    
                    //Spawn LineSelectPrefab at the players location
                    radiusSelect = Instantiate(
                        LineSelectPrefab, 
                        GameManager.GetInstance().playingCharacterGameObjects[attackerName].transform
                    );

                    //Resize it according to the ability's statistics.
                    radiusSelect.GetComponent<LineSelectScript>().SetScale(
                        abilityInformation.effects[0].areaOfEffect.range,
                        abilityInformation.effects[0].areaOfEffect.radius
                    );

                    CurrentSelectionType = AbilitySelectType.SELF_SHAPE;
                    break;                

                case AreaShape.CIRCLE:
                    radiusSelect = Instantiate(CircleSelectPrefab);
                    CurrentSelectionType = AbilitySelectType.SHAPE;

                    //Size the spawned selector
                    radiusSelect.GetComponent<RadiusSelectScript>().radius = abilityInformation.effects[0].areaOfEffect.radius;
                    radiusSelect.GetComponent<RadiusSelectScript>().SetScale();

                    //Set the spawned selector's position
                    radiusSelect.transform.SetParent(this.transform.parent.transform, false);
                    radiusSelect.transform.localPosition = this.transform.localPosition;

                    //Set the AbilityRangeDisplay
                    radiusMoveOnTouch.SetActive(true);
                    radiusMoveOnTouch.GetComponent<MoveGameobjectToTouchPoint>().movee = radiusSelect;
                    radiusMoveOnTouch.GetComponent<AbilityRangeDisplay>().Activate();

                    break;

                case AreaShape.SELECT:
                    radiusSelect = null;
                    CurrentSelectionType = AbilitySelectType.SELECT;
                    radiusMoveOnTouch.GetComponent<AbilityRangeDisplay>().Activate();
                    break;
            }                               

            return true;          
        }
        else
        {
            ToastMessageManager.Instance.CreateToast(attacker.name + " cannot cast ability " + abilityInformation.name);
            Debug.Log(attacker.name + " cannot cast ability " + abilityInformation.name);
            return false;
        }            
    }

    /*-------------------------- ABILITY ACTIVATION --------------------------*/

    public void ApplyAbilityEffects()
    {
        foreach (EffectApplicationData appData in applicationData)
        {
            appData.ApplyEffect();
            appData.LogApplication();
        }
    }

    IEnumerator ActivationSyncAbility()
    {
        //Start Animation
        if (defenderCharacters.Count > 0)
            Attacker_Face_Defenders();

        attackerAnimationManager.IdleTo_Animation(abilityToCastPresentation.animations.attacker);

        //Play DuringActivationVFX
        ParticleSystem duringActivationVFX = null;

        PlayDuringActivationFX(out duringActivationVFX);

        /*
         * Wait for the animation to register
        */
        float secsForAnimationToRegister = 0.4f;
        yield return new WaitForSeconds(secsForAnimationToRegister);
        float animationEnds = attackerAnimationManager.GetCurrentAnimationDuration()
                                - secsForAnimationToRegister;

        /*
        * Wait for the Impact point of the animation to activate the ability
        *      1. Create the data
        *      2. Send them to other players
        *      3. Apply the Effects
        */
        yield return new WaitForSeconds(animationEnds - animationEnds * 3 / 8);

        ApplyAbilityEffects();
        ActivateDefenderAnimations();

        //For VFX
        PlayOnImpactFX();
        PlayDefendersVFX();

        DeactivateDuringActivationFX(duringActivationVFX);

        /*
         * Wait for the animation to end
        */
        yield return new WaitForSeconds(animationEnds - animationEnds * 5 / 8);

        /*
         * Switch Back to Idle Animation
        */
        ReturnAttackerToIdleAnimation();
        ReturnDefendersToIdleAnimation();

        /*
         * Clean manager state
        */
        CleanState();

        /*
         * Spawn the window that displays the abilities
        */
        SelectAbilityUIManager.Instance.GiveTurnToPlayingCharacter();
    }

    public void ActivateAbility()
    {
        //Get Data to apply the ability
        attacker.GetAbilityApplicationData(
            abilityToCastInformation.name, 
            out abilitySucceedsOnDefendersList, 
            out applicationData, 
            defenderCharacters, 
            attacker
        );

        if (radiusSelect)
        {
            radiusSelectPosition = radiusSelect.transform.localPosition;
            radiusSelectRotation = radiusSelect.transform.rotation.eulerAngles;

            //Because of Line model Shanenigans
            radiusSelectRotation.y -= 90;
        }


        //Sync AbilityManager with all the others
        MultiplayerCallsAbilityCast.Instance.Propagate_AbilityManagerSync(abilityToCastInformation.name, attacker.name, GetDefenderNameList(), GetApplicationDataStrings(), abilitySucceedsOnDefendersList.ToArray());

        //Enable movement again
        CharacterMoveManager.Instance.isMovementAllowed = true;

        //Deactivate any spawned objects related to the activation of the ability
        DeactivateAbilityActivationObjects();

        //Activate Ability
        Instance.StartCoroutine(ActivationSyncAbility());

        //Send for other players to Activate
        MultiplayerCallsAbilityCast.Instance.Propagate_RemoteActivateAbility();
    }

    public void CancelActivation()
    {
        //Enable movement again
        CharacterMoveManager.Instance.isMovementAllowed = true;

        DeactivateAbilityActivationObjects();
        CleanState();

        SelectAbilityUIManager.Instance.abilitiesTab.gameObject.SetActive(true);
    }

    //Deactivate Spawned Objects
    public void DeactivateAbilityActivationObjects()
    {
        //Objects searched with ANYobjecttype are singletons
        AbilityRangeDisplay.Instance.Deactivate();
        //FindAnyObjectByType<AbilityRangeDisplay>().Deactivate();

        if (CurrentSelectionType == AbilitySelectType.SHAPE)
        {
            radiusSelect.GetComponent<RadiusSelectScript>().OnAbilityActivate();
        }
        else if (CurrentSelectionType == AbilitySelectType.SELF_SHAPE)
        {
            radiusSelect.GetComponent<LineSelectScript>().OnAbilityActivate();
        }
        else if (CurrentSelectionType == AbilitySelectType.SELECT)
        {
            foreach (GameObject defender in defendersGameObject)
            {
                defender.GetComponent<SelectUnitManager>().OnAbilityResolved();
            }
        }
    }


    /*-------------------------- MULPTIPLAYER --------------------------*/

    public void SyncManagerData(
        string ablityToCast, string attackerName,
        string[] defenders, string[] applicationData,
        bool[] abilitySuccessList
    )
    {
        //Set the parameters for the casting of the ability
        GetInstance().attacker = GameManager.GetInstance().characterPool[attackerName];
        GetInstance().abilityToCastInformation = AbilitiesManager.GetInstance().abilities[ablityToCast];
        GetInstance().attackerAnimationManager = GameManager.GetInstance().playingCharacterGameObjects[attackerName].GetComponent<AnimationManager>();

        foreach (string data in applicationData)
        {
            GetInstance().applicationData.Add(EffectApplicationData.DeserializeJsonToList(data));
        }

        foreach (bool b in abilitySuccessList)
        {
            GetInstance().abilitySucceedsOnDefendersList.Add(b);
        }

        foreach (string defender in defenders)
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

    public string[] GetApplicationDataStrings()
    {
        List<string> list = new List<string>();
        foreach (EffectApplicationData data in applicationData)
        {
            list.Add(data.SerializeListToJson());
        }

        return list.ToArray();
    }

    public void ActivateAbility_Remotely()
    {
        //Start Animation
        if (defenderCharacters.Count > 0)
            Attacker_Face_Defenders();

        attackerAnimationManager.IdleTo_Animation(abilityToCastPresentation.animations.attacker);

        //Activate Ability
        Instance.StartCoroutine(ActivationSyncAbility_Remotely());        
    }

    //The difference is that it does not calculate the ability effects because it receives it remotely
    IEnumerator ActivationSyncAbility_Remotely()
    {
        /*
         * Wait for the animation to register
        */
        float secsForAnimationToRegister = 0.4f;
        yield return new WaitForSeconds(secsForAnimationToRegister);
        float animationEnds = attackerAnimationManager.GetCurrentAnimationDuration()
                                - secsForAnimationToRegister;

        /*
        * Wait for the Impact point of the animation to activate the ability
        *      1. Create the data
        *      2. Send them to other players
        *      3. Apply the Effects
        */
        yield return new WaitForSeconds(animationEnds - animationEnds * 3 / 8);

        ApplyAbilityEffects();
        ActivateDefenderAnimations();
        PlayOnImpactFX();


        /*
         * Wait for the animation to end
        */
        yield return new WaitForSeconds(animationEnds - animationEnds * 5 / 8);

        /*
         * Switch Back to Idle Animation
        */
        ReturnAttackerToIdleAnimation();
        ReturnDefendersToIdleAnimation();

        /*
         * Clean manager state
        */
        CleanState();
    }


    /*-------------------------- ANIMATION --------------------------*/
    public void ReturnAttackerToIdleAnimation()
    {
        attackerAnimationManager.Animation_ToIdle(abilityToCastPresentation.animations.attacker);
    }

    public void ActivateDefenderAnimations()
    {
        Assert.AreEqual(abilitySucceedsOnDefendersList.Count, defendersGameObject.Count);
        for (int i = 0; i < abilitySucceedsOnDefendersList.Count; i++)
        {
            if (abilitySucceedsOnDefendersList[i])
            {
                defendersGameObject[i].GetComponent<AnimationManager>()
                    .IdleTo_Animation(abilityToCastPresentation.animations.defender_AbilitySucceeds);
            }
            else
            {
                defendersGameObject[i].GetComponent<AnimationManager>()
                    .IdleTo_Animation(abilityToCastPresentation.animations.defender_AbilityFails);
            }
        }
    }

    public void ReturnDefendersToIdleAnimation()
    {
        Assert.AreEqual(abilitySucceedsOnDefendersList.Count, defendersGameObject.Count);
        for (int i = 0; i < abilitySucceedsOnDefendersList.Count; i++)
        {
            if (abilitySucceedsOnDefendersList[i])
            {
                defendersGameObject[i].GetComponent<AnimationManager>()
                    .Animation_ToIdle(abilityToCastPresentation.animations.defender_AbilitySucceeds);
            }
            else
            {
                defendersGameObject[i].GetComponent<AnimationManager>()
                    .Animation_ToIdle(abilityToCastPresentation.animations.defender_AbilityFails);
            }
        }
    }

    void FaceDirection(Transform direction_object, Transform turn_object)
    {
        float rotSpeed = 360f;

        //When on target -> dont rotate!
        if ((direction_object.position - turn_object.position).magnitude < 0.1f) return;

        Vector3 direction = (direction_object.position - turn_object.transform.position).normalized;
        Quaternion qDir = Quaternion.LookRotation(direction);
        turn_object.transform.rotation = Quaternion.Slerp(turn_object.transform.rotation, qDir, Time.deltaTime * rotSpeed);
    }

    void Attacker_Face_Defenders()
    {
        Transform attackerTransform = GameManager.GetInstance().playingCharacterGameObjects[attacker.name].transform;
        Transform defenderTransform = defendersGameObject[0].transform;
        
        FaceDirection(defenderTransform, attackerTransform);      
    }

    //Cleans Manager State
    public void CleanState()
    {
        abilityToCastInformation = null;

        attacker = null;

        defenderCharacters.Clear();
        defendersGameObject.Clear();

        abilitySucceedsOnDefendersList.Clear();
        applicationData.Clear();

        CurrentSelectionType = AbilitySelectType.INACTIVE;
    }

    /*-------------------------- FX --------------------------*/
    public void PlayOnImpactFX()
    {
        Vector3 effectPosition = Vector3.zero;
         
        if(abilityToCastPresentation.visualEffects.onImpact != "")
        {
            string effectName = abilityToCastPresentation.visualEffects.onImpact;

            //Activate the effect at the selected position of the shape
            if (CurrentSelectionType == AbilitySelectType.SHAPE)
            {
                VFXManager.Instance.ActivateVFX(
                    abilityToCastPresentation.visualEffects.onImpact,
                    null,
                    out effectPosition,
                    radiusSelectPosition
                );
            }

            //Activate the effect originating from the attacker
            else if (CurrentSelectionType == AbilitySelectType.SELF_SHAPE)
            {
                Vector3 LocalPositionBottomOfAttacker = Vector3.zero;
                Transform attackerTransform = GameManager.GetInstance().playingCharacterGameObjects[attacker.name].transform;

                VFXManager.Instance.ActivateVFX(
                    effectName,
                    attackerTransform,
                    out effectPosition,
                    LocalPositionBottomOfAttacker,
                    radiusSelectRotation
                );
            }

            //Activate the effect for all defenders originating from the middle of their bodies.
            else if (CurrentSelectionType == AbilitySelectType.SELECT)
            {

            }
        }
        
        if (abilityToCastPresentation.soundEffects.onImpact != "")
        {
            VFXManager.Instance.PlaySoundFX_onImpact(
                abilityToCastPresentation.soundEffects.onImpact,
                effectPosition
            );
        }
    }

    public void PlayDuringActivationFX(out ParticleSystem fx)
    {
        //Models are created with their bottom center being (0,0,0)
        Vector3 LocalPositionBottomOfAttacker = Vector3.zero;
        Vector3 effectPosition = Vector3.zero;

        Transform attackerTransform = GameManager.GetInstance().playingCharacterGameObjects[attacker.name].transform;
        fx = null;

        string effectName = abilityToCastPresentation.visualEffects.duringActivation;

        if (abilityToCastPresentation.visualEffects.duringActivation != "") {
            fx = VFXManager.Instance.ActivateVFX(
                effectName,
                attackerTransform,
                out effectPosition,
                LocalPositionBottomOfAttacker
            );
        }
            
        if (abilityToCastPresentation.soundEffects.duringActivation != "")
        {
            VFXManager.Instance.PlaySoundFX_duringActivation(
                abilityToCastPresentation.soundEffects.duringActivation,
                effectPosition
            );
        }
    }

    public void PlayDefendersVFX()
    {
        Assert.AreEqual(abilitySucceedsOnDefendersList.Count, defendersGameObject.Count);
        string effectName = abilityToCastPresentation.visualEffects.onDefender;

        if (effectName != "")
        {            
            for (int i = 0; i < abilitySucceedsOnDefendersList.Count; i++)
            {
                //Models are created with their bottom center being (0,0,0)
                Vector3 LocalPositionBottomOfAttacker = Vector3.zero;
                Vector3 effectPosition = Vector3.zero;

                //foreach defender that the ability succeeded
                if (abilitySucceedsOnDefendersList[i])
                {
                    Transform defenderTransform = defendersGameObject[i].transform;
                    VFXManager.Instance.ActivateVFX(
                        effectName,
                        defenderTransform,
                        out effectPosition,
                        LocalPositionBottomOfAttacker
                    );

                }
            }
        }        
    }

    void DeactivateDuringActivationFX(ParticleSystem duringActivationVFX)
    {
        if (duringActivationVFX != null)
            VFXManager.Instance.DeactivateVFX(duringActivationVFX);

        if (abilityToCastPresentation.soundEffects.duringActivation != "")
            VFXManager.Instance.DeactivateDuringActivationSoundFX();
    }
}
