using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Microsoft.MixedReality.Toolkit.Input;

public class CharacterMover : MonoBehaviour, IMixedRealityPointerHandler, IMixedRealityTouchHandler
{
    public NavMeshAgent movee;

    /* SET ON UNITY EDITOR */
    public Material lineMaterial;
    public GameObject DestinationToken;
    public ConfirmMoveCanvas confirmMoveUI;
    public bool allowDestinationSelection = true;

    Vector3 newPosition;
    float distance;

    public static CharacterMover Instance { get; private set; }

    public static CharacterMover GetInstance()
    {
        return Instance;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        DrawPath();
        WhenMoveeStops();
    }

    /* SELECT DESTINATION */
    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {
        if (movee != null)
        {
            if (
                allowDestinationSelection && 
                GameManager.GetInstance().player == movee.gameObject.
                                                    GetComponent<CharacterScript>().
                                                    GetCharacter().player
            )
            {
                //Find Selected Table Position
                FindPointPosition(eventData);

                // If the new position is legal display interface to perform the move
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
    }

    void FindPointPosition(MixedRealityPointerEventData eventData)
    {
        //Touch Pointed
        if (eventData.Pointer.BaseCursor.Position.y == 0)
            newPosition = new Vector3(eventData.Pointer.Position.x, movee.transform.position.y, eventData.Pointer.Position.z);
        //Ray Pointed
        else
            newPosition = new Vector3(eventData.Pointer.BaseCursor.Position.x, movee.transform.position.y, eventData.Pointer.BaseCursor.Position.z);
    }

    /* ACCEPT OR DENY MOVE METHODS */

    public void PerformMove()
    {
        //Disable Movement Selection until movement stops
        allowDestinationSelection = false;

        //Do the movement
        FaceDirection();
        movee.isStopped = false;

        Character c = GameManager.GetInstance().characterPool[movee.gameObject.GetComponent<CharacterScript>().name];

        //Subtract the speed.
        c.GetStat("Speed").DealDamage((int)distance);
        c.GetStat("Speed").CalculateCurrentValue();

        //Start Moving Animation
        movee.GetComponent<AnimationManager>().IdleTo_Moving();

        //Send to other players to perform the move
        MultiplayerMovementCalls.Instance.Propagate_Movement(
            newPosition.x, 
            newPosition.y, 
            newPosition.z, 
            c.name, 
            distance
        );
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

    public void CancelMove()
    {
        movee.ResetPath();
    }

    /* FOR MULTIPLAYER */
    public void SyncMovementData(float x, float y, float z, string name, float distance)
    {
        newPosition = new Vector3(x, y, z);
        this.distance = distance;
        movee = GameManager.GetInstance().playingCharacterGameObjects[name].GetComponent<NavMeshAgent>();
    }

    //the only different is that it doesn't send a call to other clients
    public void PerformMoveRemotely()
    {
        //Disable Movement Selection until movement stops
        allowDestinationSelection = false;

        //Do the movement
        movee.SetDestination(newPosition);
        FaceDirection();
        

        Character c = GameManager.GetInstance().characterPool[movee.gameObject.GetComponent<CharacterScript>().name];

        //Subtract the speed.
        c.GetStat("Speed").DealDamage((int)distance);
        c.GetStat("Speed").CalculateCurrentValue();

        //Start Moving Animation
        movee.GetComponent<AnimationManager>().IdleTo_Moving();
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

    void WhenMoveeStops()
    {
        if(movee == null || movee.path.corners.Length == 1)
        {
            //Despawn Destination token
            if(DestinationToken.activeSelf)
                DestinationToken.SetActive(false);

            //Move back to Idle animation
            if (movee.GetComponent<AnimationManager>() && movee.GetComponent<AnimationManager>().GetHasMovingAnimation())
                movee.GetComponent<AnimationManager>().MovingTo_Idle();

            //Allow Selection of new Destination
            allowDestinationSelection = true;
        }
    }
        
    void FaceDirection()
    {
        float rotSpeed = 360f;

        //When on target -> dont rotate!
        if ((movee.destination - movee.transform.position).magnitude < 0.1f) return;

        Vector3 direction = (movee.destination - movee.transform.position).normalized;
        Quaternion qDir = Quaternion.LookRotation(direction);
        movee.transform.rotation = Quaternion.Slerp(movee.transform.rotation, qDir, Time.deltaTime * rotSpeed);
    }

    /* UI METHODS */
    void DrawPath()
    {
        var line = GetComponent<LineRenderer>();
        if (movee == null || movee.path.corners.Length == 1)
        {
            if (line != null)
                GetComponent<LineRenderer>().startWidth = 0f;
            return;
        }


        if (line == null)
        {
            line = this.gameObject.AddComponent<LineRenderer>();
            line.material = lineMaterial;
        }

        line.startWidth = 0.002f;
        var path = movee.path;

        line.positionCount = path.corners.Length;

        for (int i = 0; i < path.corners.Length; i++)
        {
            line.SetPosition(i, path.corners[i]);
        }
    }

    void SpawnDestinationToken()
    {
        DestinationToken.SetActive(true);
        DestinationToken.transform.position = movee.destination;
        //DestinationToken.transform.localPosition = new Vector3(DestinationToken.transform.localPosition.x, 0.5f, DestinationToken.transform.localPosition.z);
    }

    void SpawnMoveMenu()
    {
        confirmMoveUI.gameObject.SetActive(true);
        Vector3 menuPosition = new Vector3(movee.destination.x, movee.destination.y + 0.02f, movee.destination.z);
        confirmMoveUI.MoveToPoint(menuPosition);
    }

    /* GENERAL MANAGEMENT METHODS */

    public void OnChangeTurn(string newCharName)
    {
        movee = GameManager.GetInstance().playingCharacterGameObjects[newCharName].GetComponent<NavMeshAgent>();
    }

    public void Enable()
    {
        enabled = true;
    }
    public void Disable()
    {
        enabled = false;
        movee = null;
    }

    

    /* INTERFACE METHODS */
    
    public void OnPointerDown(MixedRealityPointerEventData eventData) { }
    public void OnPointerClicked(MixedRealityPointerEventData eventData) { }
    public void OnTouchStarted(HandTrackingInputEventData eventData) { }
    public void OnTouchUpdated(HandTrackingInputEventData eventData) { }
    public void OnPointerDragged(MixedRealityPointerEventData eventData) { }

    public void OnTouchCompleted(HandTrackingInputEventData eventData) { }
}
