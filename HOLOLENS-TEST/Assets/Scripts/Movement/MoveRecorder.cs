using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;

public class MoveRecorder : MonoBehaviour, IMixedRealityPointerHandler, IMixedRealityTouchHandler
{
    /* SELECT DESTINATION */
    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {
        Vector3 newPosition;

        //Touch Pointed
        if (eventData.Pointer.BaseCursor.Position.y == 0)
            newPosition = new Vector3(eventData.Pointer.Position.x, CharacterMoveManager.Instance.movee.transform.position.y, eventData.Pointer.Position.z);
        //Ray Pointed
        else
            newPosition = new Vector3(eventData.Pointer.BaseCursor.Position.x, CharacterMoveManager.Instance.movee.transform.position.y, eventData.Pointer.BaseCursor.Position.z);

        CharacterMoveManager.Instance.RecordMovement(newPosition);
    }

    /* INTERFACE METHODS */
    public void OnPointerDown(MixedRealityPointerEventData eventData) { }
    public void OnPointerClicked(MixedRealityPointerEventData eventData) { }
    public void OnTouchStarted(HandTrackingInputEventData eventData) { }
    public void OnTouchUpdated(HandTrackingInputEventData eventData) { }
    public void OnPointerDragged(MixedRealityPointerEventData eventData) { }

    public void OnTouchCompleted(HandTrackingInputEventData eventData) { }
}
