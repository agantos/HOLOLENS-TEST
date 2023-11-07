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
        if(CastingAbilityManager.GetInstance().CurrentSelectionType == AbilitySelectType.SELECT)
            PerformAbilitySelection();
        else
            CharacterInfoObjectsManager.Instance.CreateCharacterInfo(GetComponent<CharacterScript>().charName);
    }
   
    //Selection with point ray
    public void OnPointerUp(MixedRealityPointerEventData eventData) {
        if (CastingAbilityManager.GetInstance().CurrentSelectionType == AbilitySelectType.SELECT)
        {
            //Select unit for ability
            PerformAbilitySelection();
        }
        else
        {
            //View unit's information
            CharacterInfoObjectsManager.Instance.CreateCharacterInfo(GetComponent<CharacterScript>().charName);
        }
            
    }

    /*SELECTION FOR ABILITY METHODS */
    private void PerformAbilitySelection()
    {
        float defenderDistance = GameplayCalculatorFunctions.CalculateDistanceInFeet(
                GameManager.GetInstance().playingCharacterGameObjects[CastingAbilityManager.GetInstance().attacker.name].transform.position,
                gameObject.transform.position
            );

        Debug.Log("Defender distance is" + defenderDistance);

        int abilityRange = CastingAbilityManager.GetInstance().abilityToCastInformation.effects[0].areaOfEffect.range;

        //(Un)Select a character only if they are within range
        if (abilityRange > defenderDistance)
        {
            if (!isSelected)
            {
                //Abide by the number of selectable characters
                if (CastingAbilityManager.GetInstance().defenderCharacters.Count < CastingAbilityManager.GetInstance().abilityToCastInformation.effects[0].targetting.numberOfTargets)
                {
                    SpawnSelectionToken();
                    SelectCharacter_AbilityTarget();
                }
                else
                {
                    ToastMessageManager.Instance.CreateToast(
                        "Max number of selectable characters for these effect is " +
                        CastingAbilityManager.GetInstance().abilityToCastInformation.effects[0].targetting.numberOfTargets.ToString()
                    );
                }
            }
            else
            {
                Destroy(token);
                UnselectCharacter_AbilityTarget();
            }
        }
        else
            ToastMessageManager.Instance.CreateToast("Subject is too far away");
    }
    
    private void SpawnSelectionToken()
    {
        //Spawn selection token.
        token = Instantiate(selectToken);
        token.transform.SetParent(gameObject.transform, false);

        //Models may be rescaled but the scale will be uniform. Remove this scale from the token.

        //Place selection token on top of model.
        float playerHeight = gameObject.transform.localPosition.y + gameObject.transform.localScale.y;
        token.transform.localPosition = Vector3.zero;
        token.transform.localPosition += new Vector3(0, playerHeight + 0.3f, 0);
    }

    public void OnAbilityResolved()
    {
        Destroy(token);
        isSelected = false;
    }

    private void SelectCharacter_AbilityTarget()
    {
        isSelected = true;
        CastingAbilityManager.GetInstance().defenderCharacters.Add(gameObject.GetComponent<CharacterScript>().GetCharacter());
        CastingAbilityManager.GetInstance().defendersGameObject.Add(gameObject);
    }

    private void UnselectCharacter_AbilityTarget()
    {
        isSelected = false;
        CastingAbilityManager.GetInstance().defenderCharacters.Remove(gameObject.GetComponent<CharacterScript>().GetCharacter());
        CastingAbilityManager.GetInstance().defendersGameObject.Remove(gameObject);
    }

    /* CHARACTER MENU SELECTION METHODS */
    private void SelectCharacter_SpawnMenu()
    {

    }

    /* INTERFACE METHODS THAT ARE NOT USED */
    public void OnPointerDown(MixedRealityPointerEventData eventData)  {
        
    }
    public void OnPointerClicked(MixedRealityPointerEventData eventData) { }

    public void OnTouchStarted(HandTrackingInputEventData eventData) { }
    public void OnTouchUpdated(HandTrackingInputEventData eventData) { }
    public void OnPointerDragged(MixedRealityPointerEventData eventData) { }
}
