using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;

public class SelectUnit : MonoBehaviour, IMixedRealityPointerHandler, IMixedRealityTouchHandler
{
    public GameObject selectToken;
    public List<GameObject> tokensCreated;

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

    public void OnPointerClicked(MixedRealityPointerEventData eventData) {
        //if (eventData.selectedObject.tag == "Player")
        //{
        //    //Spawn selection token on top of player model.
        //    GameObject token = Object.Instantiate(selectToken);
        //    token.transform.SetParent(eventData.selectedObject.transform.parent.transform, false);
        //    float playerHeight = eventData.selectedObject.transform.localPosition.y + eventData.selectedObject.transform.localScale.y / 2;
        //    token.transform.localPosition += new Vector3(0, playerHeight + 0.1f, 0);
        //    tokensCreated.Add(token);
        //}
    }

    public void OnTouchStarted(HandTrackingInputEventData eventData)
    {
    }
    public void OnTouchCompleted(HandTrackingInputEventData eventData)
    {
        Debug.Log("Select");
    }
    public void OnTouchUpdated(HandTrackingInputEventData eventData) { }

    public void OnPointerDragged(MixedRealityPointerEventData eventData) { }
    public void OnPointerUp(MixedRealityPointerEventData eventData) {
        Debug.Log("Select");
    }
}
