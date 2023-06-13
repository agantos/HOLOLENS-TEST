using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;

public class SelectUnit : MonoBehaviour, IMixedRealityPointerHandler, IMixedRealityTouchHandler
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
    }
   
    //Selection with point ray
    public void OnPointerUp(MixedRealityPointerEventData eventData) {
        if (CastingAbilityManager.CurrentSelectionType == AbilitySelectType.SELECT)
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
    }

    private void SpawnToken()
    {
        //Spawn selection token.
        token = Object.Instantiate(selectToken);
        token.transform.SetParent(gameObject.transform, false);

        //Models may be rescaled but the scale will be uniform. Remove this scale from the token.
        token.transform.localScale *= 1 / gameObject.transform.localScale.x;

        //Place selection token on top of model.
        float playerHeight = gameObject.transform.localPosition.y + 0.5f;
        token.transform.localPosition += new Vector3(0, playerHeight + 0.3f, 0);
    }

    private void SelectCharacter()
    {
        CastingAbilityManager.defenders.Add(gameObject.GetComponent<CharacterScript>().GetCharacter());
    }

    private void UnselectCharacter()
    {
        CastingAbilityManager.defenders.Remove(gameObject.GetComponent<CharacterScript>().GetCharacter());
    }
}
