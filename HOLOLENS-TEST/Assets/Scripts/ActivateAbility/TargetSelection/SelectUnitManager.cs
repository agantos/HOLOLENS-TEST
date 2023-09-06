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

    //Selection with touch
    public void OnTouchCompleted(HandTrackingInputEventData eventData)
    {
        if(CastingAbilityManager.CurrentSelectionType == AbilitySelectType.SELECT)
            PerformAbilitySelection();
   
    }
   
    //Selection with point ray
    public void OnPointerUp(MixedRealityPointerEventData eventData) {
        if (CastingAbilityManager.CurrentSelectionType == AbilitySelectType.SELECT)
            PerformAbilitySelection();
        else
            CharacterInfoObjectsManager.instance.CreateCharacterInfo(GetComponent<CharacterScript>().charName);
    }

    /*SELECTION FOR ABILITY METHODS */
    private void PerformAbilitySelection()
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
            }
        }
        else
            Debug.Log("Subject is too far away");
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

    public void OnAbilityResolved()
    {
        Destroy(token);
        isSelected = false;
    }

    private void SelectCharacter()
    {
        isSelected = true;
        CastingAbilityManager.defenderCharacters.Add(gameObject.GetComponent<CharacterScript>().GetCharacter());
        CastingAbilityManager.defendersGameObject.Add(gameObject);
    }

    private void UnselectCharacter()
    {
        isSelected = false;
        CastingAbilityManager.defenderCharacters.Remove(gameObject.GetComponent<CharacterScript>().GetCharacter());
        CastingAbilityManager.defendersGameObject.Remove(gameObject);
    }

    /* CHARACTER MENU SELECTION METHODS */

    /* INTERFACE METHODS THAT ARE NOT USED */
    public void OnPointerDown(MixedRealityPointerEventData eventData)  {
        
    }
    public void OnPointerClicked(MixedRealityPointerEventData eventData) { }

    public void OnTouchStarted(HandTrackingInputEventData eventData) { }
    public void OnTouchUpdated(HandTrackingInputEventData eventData) { }
    public void OnPointerDragged(MixedRealityPointerEventData eventData) { }
}
