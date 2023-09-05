using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;

//Singleton that when activated allows the selection of units
//for the activation of an ability
public class SelectUnitManager : MonoBehaviour, IMixedRealityPointerHandler, IMixedRealityTouchHandler
{
    public GameObject selectToken;
    private bool isSelected;
    private GameObject token;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {

    }

    public void OnPointerClicked(MixedRealityPointerEventData eventData) { }

    public void OnTouchStarted(HandTrackingInputEventData eventData) { }
    public void OnTouchUpdated(HandTrackingInputEventData eventData) { }
    public void OnPointerDragged(MixedRealityPointerEventData eventData) { }

    //Selection with touch
    public void OnTouchCompleted(HandTrackingInputEventData eventData)
    {
        if(CastingAbilityManager.CurrentSelectionType == AbilitySelectType.SELECT)
        {

            float defenderDistance = GameplayCalculatorFunctions.CalculateDistanceInFeet(
                GameManager.playingCharacterGameObjects[CastingAbilityManager.attacker.name].transform.position,
                gameObject.transform.position
            );

            Debug.Log("defender distance is" + defenderDistance);

            int abilityRange = CastingAbilityManager.abilityToCast.effects[0].areaOfEffect.range;
            if (abilityRange > defenderDistance)
            {
                if (!isSelected)
                {
                    SpawnToken();
                    SelectCharacter();
                    isSelected = true;
                }
                else
                {
                    Destroy(token);
                    UnselectCharacter();
                    isSelected = false;
                }
            }
            else
            {
                Debug.Log("Defender " + gameObject.GetComponent<CharacterScript>().charName +
                    " is too far away from " + CastingAbilityManager.attacker.name
                );
            }
        }    
    }
   
    //Selection with point ray
    public void OnPointerUp(MixedRealityPointerEventData eventData) {
        if (CastingAbilityManager.CurrentSelectionType == AbilitySelectType.SELECT)
        {
            float defenderDistance = GameplayCalculatorFunctions.CalculateDistanceInFeet(
                GameManager.playingCharacterGameObjects[CastingAbilityManager.attacker.name].transform.position,
                gameObject.transform.position
            );

            Debug.Log("Defender distance is" + defenderDistance);

            int abilityRange = CastingAbilityManager.abilityToCast.effects[0].areaOfEffect.range;

            //(Un)Select a character only if they are within range
            if (abilityRange > defenderDistance)
            {
                if (!isSelected)
                {
                    //Abide by the number of selectable characters
                    if (CastingAbilityManager.defenderCharacters.Count < CastingAbilityManager.abilityToCast.effects[0].targetting.numberOfTargets)
                    {
                        SpawnToken();
                        SelectCharacter();
                        isSelected = true;
                    }
                    else
                    {
                        Debug.Log("Max number of selectable characters for these effect is " +
                            CastingAbilityManager.abilityToCast.effects[0].targetting.numberOfTargets.ToString());
                    }
                }
                else
                {
                    Destroy(token);
                    UnselectCharacter();
                    isSelected = false;
                }
            }
            else
                Debug.Log("Subject is too far away");
                       
        }
    }

    private void SpawnToken()
    {
        //Spawn selection token.
        token = Object.Instantiate(selectToken);
        token.transform.SetParent(gameObject.transform, false);

        //Models may be rescaled but the scale will be uniform. Remove this scale from the token.

        //Place selection token on top of model.
        float playerHeight = gameObject.GetComponent<CharacterScript>().model.transform.localScale.y + 0.4f;
        token.transform.localPosition += new Vector3(0, playerHeight + 0.1f, 0);
    }

    public void OnAbilityActivate()
    {
        Destroy(token);
    }

    public void OnAbilityDeActivate()
    {
        Destroy(token);
    }

    private void SelectCharacter()
    {
        CastingAbilityManager.defenderCharacters.Add(gameObject.GetComponent<CharacterScript>().GetCharacter());
        CastingAbilityManager.defendersGameObject.Add(gameObject);
    }

    private void UnselectCharacter()
    {
        CastingAbilityManager.defenderCharacters.Remove(gameObject.GetComponent<CharacterScript>().GetCharacter());
        CastingAbilityManager.defendersGameObject.Remove(gameObject);
    }
}
