using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveGameobjectToTouchPoint : MonoBehaviour, IMixedRealityTouchHandler, IMixedRealityPointerHandler
{
    public GameObject movee;
    public void OnTouchStarted(HandTrackingInputEventData eventData)
    {
    }
    public void OnTouchCompleted(HandTrackingInputEventData eventData) {
        Debug.Log(eventData.InputData);
        if(movee != null)
            movee.transform.position = new Vector3(eventData.InputData.x, movee.transform.position.y, eventData.InputData.z);
    }

    public void OnTouchUpdated(HandTrackingInputEventData eventData) { }

    public void OnPointerDragged(MixedRealityPointerEventData eventData) { }

    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {
        Debug.Log(eventData.Pointer.Position);
        
        if (movee != null)
            movee.transform.position = new Vector3(eventData.Pointer.BaseCursor.Position.x, movee.transform.position.y, eventData.Pointer.BaseCursor.Position.z);
    }

    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {

    }

    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {

    }
}
