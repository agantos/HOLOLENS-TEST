using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Microsoft.MixedReality.Toolkit.Input;

public class MoveCharacter : MonoBehaviour, IMixedRealityPointerHandler, IMixedRealityTouchHandler
{
    public NavMeshAgent movee;
    public Material lineMaterial;
    public GameObject DestinationToken;

    AnimationManager animationManager;

    Vector3 newPosition;
    float distance;
    void Update()
    {
        DrawPath();
        MoveeNotMoving();
    }

    //Write this function according to the game system
    bool MoveRequirements()
    {
        float characterSpeed = movee.gameObject.GetComponent<CharacterScript>().GetCharacter().GetStat("Speed").GetCurrentValue();
        distance = CalculateDistance(newPosition);

        Debug.Log("Move Speed is " + movee.gameObject.GetComponent<CharacterScript>().GetCharacter().GetStat("Speed").GetCurrentValue());

        return true;
        //return characterSpeed >= distance;
    }

    float CalculateDistance(Vector3 newPosition)
    {
        NavMeshPath path = new NavMeshPath();
        movee.CalculatePath(newPosition, path);
        float distance = 0;
        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            distance += Vector3.Distance(path.corners[i], path.corners[i + 1]);
        }

        return GameplayCalculatorFunctions.RealWorldToGameFeet(distance);
    }    
   
    public void OnChangeTurn(string newCharName)
    {
       movee = GameManager.characterGameObjects[newCharName].GetComponent<NavMeshAgent>();        
    }

    public void Enable()
    {
        this.enabled = true;
    }
    public void Disable()
    {
        enabled = false;
        movee = null;
    }
    public void PerformMove()
    {
        //Do the movement
        InstantlyTurn();
        movee.isStopped = false;
        
        Character c = GameManager.characterPool[movee.gameObject.GetComponent<CharacterScript>().name];

        //Subtract the speed.
        c.GetStat("Speed").DealDamage((int)distance);
        c.GetStat("Speed").CalculateCurrentValue();

        //Start Moving Animation
        movee.GetComponent<AnimationManager>().IdleTo_Moving();
    }
    public void CancelMove()
    {
        movee.ResetPath();
        //Disable();
    }
    void SpawnDestinationToken()
    {     
        DestinationToken.SetActive(true);
        DestinationToken.transform.position = movee.destination;
        //DestinationToken.transform.localPosition = new Vector3(DestinationToken.transform.localPosition.x, 0.5f, DestinationToken.transform.localPosition.z);
    }
    void MoveeNotMoving()
    {
        if(movee == null || movee.path.corners.Length == 1)
        {
            //Despawn Destination token
            if(DestinationToken.activeSelf)
                DestinationToken.SetActive(false);

            //Move back to Idle animation
            if (movee.GetComponent<AnimationManager>() && movee.GetComponent<AnimationManager>().GetHasMovingAnimation())
                movee.GetComponent<AnimationManager>().MovingTo_Idle();
        }
    }
    void DrawPath()
    {
        var line = GetComponent<LineRenderer>();
        if (movee == null || movee.path.corners.Length == 1)
        {
            if(line != null)
                GetComponent<LineRenderer>().startWidth = 0f;
            return;
        }
            

        if (line == null)
        {
            line = this.gameObject.AddComponent<LineRenderer>();
            line.material = lineMaterial;            
        }

        line.startWidth = 0.005f;
        var path = movee.path;

        line.positionCount = path.corners.Length;

        for (int i = 0; i < path.corners.Length; i++)
        {
            line.SetPosition(i, path.corners[i]);
        }

    }
    void SpawnMoveMenu()
    {
        FindAnyObjectByType<ConfirmMoveCanvas>(FindObjectsInactive.Include).gameObject.SetActive(true);
        Vector3 menuPosition = new Vector3(movee.destination.x, movee.destination.y + 0.02f, movee.destination.z);
        FindAnyObjectByType<ConfirmMoveCanvas>().MoveToPoint(menuPosition);
    }
    private void InstantlyTurn()
    {
        float rotSpeed = 360f;

        //When on target -> dont rotate!
        if ((movee.destination - movee.transform.position).magnitude < 0.1f) return;

        Vector3 direction = (movee.destination - movee.transform.position).normalized;
        Quaternion qDir = Quaternion.LookRotation(direction);
        movee.transform.rotation = Quaternion.Slerp(movee.transform.rotation, qDir, Time.deltaTime * rotSpeed);
    }

    /* INTERFACE FUNCTIONS */
    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {
        if (movee != null)
        {
            newPosition = new Vector3(eventData.Pointer.BaseCursor.Position.x, movee.transform.position.y, eventData.Pointer.BaseCursor.Position.z);
            if (MoveRequirements())
            {
                movee.SetDestination(newPosition);
                SpawnMoveMenu();
                SpawnDestinationToken();
                movee.isStopped = true;
            }
            else
            {
                Debug.Log("Character does not have enough speed to move there");
            }
        }
    }
    public void OnPointerDown(MixedRealityPointerEventData eventData) { }
    public void OnPointerClicked(MixedRealityPointerEventData eventData) { }
    public void OnTouchStarted(HandTrackingInputEventData eventData) { }
    public void OnTouchUpdated(HandTrackingInputEventData eventData) { }
    public void OnPointerDragged(MixedRealityPointerEventData eventData) { }

    public void OnTouchCompleted(HandTrackingInputEventData eventData) { }
}
