using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;


public enum AbilitySelectType { SHAPE, SELECT, INACTIVE }

[System.Serializable]
public class EffectApplicationData {
    public EffectType type;
       
    public int duration;
    public int damage;

    public string affectedStat;
    public bool effectSucceeds;

    public string attacker;
    public string defender;
    public string abilityName;

    public EffectApplicationData(EffectType type, int duration, string abilityName,
                                 int damage, string statAffected, 
                                 bool effectSucceeds, string attacker, string defender)
    {
        this.type = type;
        this.duration = duration;
        this.damage = damage;
        this.affectedStat = statAffected;
        this.abilityName = abilityName;
        this.effectSucceeds = effectSucceeds;
        this.attacker = attacker;
        this.defender = defender;
    }

    public void ApplyEffect()
    {
        Character defenderCharacter = GameManager.GetInstance().playingCharacterPool[defender];
        GameObject defenderGameObject = GameManager.GetInstance().playingCharacterGameObjects[defender];
        CharacterStat stat = defenderCharacter.GetStat(affectedStat);

        switch (type)
        {
            case EffectType.DAMAGE:
                //Deal damage
                stat.DealDamage(damage);

                //Spawn Floating Text
                GameObject text = GameObject.Instantiate(GameManager.GetInstance().FloatingTextPrefab, defenderGameObject.transform);
                text.GetComponent<FloatingText>().Inititalize(damage.ToString(), true);

                break;
            case EffectType.HEALING:
                stat.HealDamage(damage);
                break;
            case EffectType.TEMPORAL:
                if(effectSucceeds)
                    defenderCharacter.GetStats().AddTemporalEffect(affectedStat, "", duration, damage);
                break;
        }

        stat.CalculateCurrentValue();
    }

    public void LogApplication()
    {
        Character defenderCharacter = GameManager.GetInstance().playingCharacterPool[defender];
        Character attackerCharacter = GameManager.GetInstance().playingCharacterPool[attacker];

        switch (type)
        {
            case EffectType.DAMAGE:
                Logger.Instance.Log_Damage((int)damage, affectedStat, defenderCharacter, attackerCharacter);
                break;
            case EffectType.HEALING:
                Logger.Instance.Log_Heal((int)damage, affectedStat, defenderCharacter, attackerCharacter);
                break;
            case EffectType.TEMPORAL:
                Logger.Instance.Log_Apply_Temporal((int)damage, affectedStat, duration, defenderCharacter, attackerCharacter);
                break;
        }
    }

    // Serialize the list to a JSON string
    public string SerializeListToJson()
    {
        return JsonUtility.ToJson(this);        
    }

    // Deserialize the JSON string back to a list
    public static EffectApplicationData DeserializeJsonToList(string jsonString)
    {
        return JsonUtility.FromJson<EffectApplicationData>(jsonString);
    }

    public override string ToString()
    {
        return $"Effect Type: {type}\n" +
               $"Duration: {duration}\n" +
               $"Damage: {damage}\n" +
               $"Affected Stat: {affectedStat}\n" +
               $"Effect Succeeds: {effectSucceeds}\n" +
               $"Attacker: {attacker}\n" +
               $"Defender: {defender}\n" +
               $"Ability Name: {abilityName}";
    }
}

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
    public List<EffectApplicationData> applicationData;

    public List<Character> defenderCharacters = new List<Character>();
    public List<GameObject> defendersGameObject = new List<GameObject>();
    
    public List<bool> abilitySuccessList = new List<bool>();
    public Ability abilityToCastInformation;
    public AbilityPresentation abilityToCastPresentation;

    public AbilitySelectType CurrentSelectionType = AbilitySelectType.INACTIVE;

    GameObject radiusSelect;
    Vector3 radiusSelectPosition;

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
                    break;                
                case AreaShape.CIRCLE:
                    radiusSelect = Instantiate(CircleSelectPrefab);
                    CurrentSelectionType = AbilitySelectType.SHAPE;
                    break;
                case AreaShape.SELECT:
                    radiusSelect = null;
                    CurrentSelectionType = AbilitySelectType.SELECT;
                    radiusMoveOnTouch.GetComponent<AbilityRangeDisplay>().Activate();
                    return true;
            }       

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

            return true;          
        }
        else
        {
            ToastMessageManager.Instance.CreateToast(attacker.name + " cannot cast ability " + abilityInformation.name);
            Debug.Log(attacker.name + " cannot cast ability " + abilityInformation.name);
            return false;
        }            
    }

    // START OF METHODS FOR RPC CALLS
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
        
        foreach(string data in applicationData)
        {
            GetInstance().applicationData.Add(EffectApplicationData.DeserializeJsonToList(data));
        }

        foreach(bool b in abilitySuccessList)
        {
            GetInstance().abilitySuccessList.Add(b);
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
        foreach(EffectApplicationData data in applicationData)
        {
            list.Add(data.SerializeListToJson());
        }

        return list.ToArray();
    }

    /*      MULTIPLAYER     */

    public void ActivateAbility_Remotely()
    {
        //Start Animation
        if (defenderCharacters.Count > 0)
            FaceDirection();

        attackerAnimationManager.IdleTo_Animation(abilityToCastPresentation.animations.attacker);

        //Activate Ability
        Instance.StartCoroutine(ActivationSyncAbility_Remotely());        
    }

    //The differece is that it does not calculate the ability effects because it receives it remotely
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
        PlayImpactVFX();

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

    // END

    //DEPRECATE
    public void ActivateAttackerAbility()
    {
        //attacker.ActivateAbility(abilityToCast.name, out abilitySuccessList, defenderCharacters, attacker);
    }

    public void ApplyAbilityEffects()
    {
        foreach(EffectApplicationData appData in applicationData)
        {
            appData.ApplyEffect();
            appData.LogApplication();
        }
    }

    public void ActivateAbility()
    {
        //Get Data to apply the ability
        attacker.GetAbilityApplicationData(abilityToCastInformation.name, out abilitySuccessList, out applicationData, defenderCharacters, attacker);
        
        if(radiusSelect)
            radiusSelectPosition = radiusSelect.transform.localPosition;

        //Sync AbilityManager with all the others
        MultiplayerCallsAbilityCast.Instance.Propagate_AbilityManagerSync(abilityToCastInformation.name, attacker.name, GetDefenderNameList(), GetApplicationDataStrings(), abilitySuccessList.ToArray());

        //Deactivate any spawned objects related to the activation of the ability
        DeactivateAbilityActivationObjects();        

        //Activate Ability
        Instance.StartCoroutine(ActivationSyncAbility());

        //Send for other players to Activate
        MultiplayerCallsAbilityCast.Instance.Propagate_RemoteActivateAbility();
    }

    public void CancelActivation()
    {
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
        attackerAnimationManager.Animation_ToIdle(abilityToCastPresentation.animations.attacker);
    }

    public void ActivateDefenderAnimations()
    {
        Assert.AreEqual(abilitySuccessList.Count, defendersGameObject.Count);
        for (int i = 0; i < abilitySuccessList.Count; i++)
        {
            if (abilitySuccessList[i])
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
        Assert.AreEqual(abilitySuccessList.Count, defendersGameObject.Count);
        for (int i = 0; i < abilitySuccessList.Count; i++)
        {
            if (abilitySuccessList[i])
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
        abilityToCastInformation = null;

        attacker = null;

        defenderCharacters.Clear();
        defendersGameObject.Clear();

        abilitySuccessList.Clear();
        applicationData.Clear();

        CurrentSelectionType = AbilitySelectType.INACTIVE;
    }

    IEnumerator ActivationSyncAbility()
    {
        //Start Animation
        if (defenderCharacters.Count > 0)
            FaceDirection();

        attackerAnimationManager.IdleTo_Animation(abilityToCastPresentation.animations.attacker);

        //Play DuringActivationVFX
        ParticleSystem duringActivation = null;

        if(abilityToCastPresentation.visualEffects.duringActivation != "")
            duringActivation = PlayVFXBottomOfAttacker(abilityToCastPresentation.visualEffects.duringActivation);

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
        PlayImpactVFX();

        if(duringActivation != null)
            VFXManager.Instance.DeactivateVFX(duringActivation);

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

    /*VISUAL VIDEO EFFECTS*/
    public void PlayImpactVFX()
    {        
        VFXManager.Instance.ActivateVFX(
            abilityToCastPresentation.visualEffects.onImpact, 
            null, 
            radiusSelectPosition
        );
        //Destroy VFX when it stops Playing.
    }

    //Models are created with their bottom center being (0,0,0)
    public ParticleSystem PlayVFXBottomOfAttacker(string effectName)
    {
        Vector3 LocalPositionBottomOfAttacker = Vector3.zero;

        Transform attackerTransform = GameManager.GetInstance().playingCharacterGameObjects[attacker.name].transform;

        return VFXManager.Instance.ActivateVFX(
            effectName,
            attackerTransform,
            LocalPositionBottomOfAttacker
        );    
    }
}
