using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TakeTableCoordinates : MonoBehaviour, IMixedRealityTouchHandler
{
    public GameObject movee;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTouchStarted(HandTrackingInputEventData eventData)
    {
    }
    public void OnTouchCompleted(HandTrackingInputEventData eventData) {
        Debug.Log(eventData.InputData);
        movee.transform.position = new Vector3(eventData.InputData.x, movee.transform.position.y, eventData.InputData.z);
    }
    public void OnTouchUpdated(HandTrackingInputEventData eventData) { }

}
