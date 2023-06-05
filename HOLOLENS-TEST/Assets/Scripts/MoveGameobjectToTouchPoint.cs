using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveGameobjectToTouchPoint : MonoBehaviour, IMixedRealityTouchHandler
{
    public GameObject movee;
    public void OnTouchStarted(HandTrackingInputEventData eventData)
    {
    }
    public void OnTouchCompleted(HandTrackingInputEventData eventData) {
        Debug.Log(eventData.InputData);
        movee.transform.position = new Vector3(eventData.InputData.x, movee.transform.position.y, eventData.InputData.z);
    }
    public void OnTouchUpdated(HandTrackingInputEventData eventData) { }

}
