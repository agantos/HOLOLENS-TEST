using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmMoveCanvas : MonoBehaviour
{
    public void MoveToPoint(Vector3 position)
    {
        gameObject.transform.position = position;
    }

    public void ConfirmMovement()
    {
        FindAnyObjectByType<MoveCharacter>().Move();
    }

    public void CancelMovement()
    {
        FindAnyObjectByType<MoveCharacter>().CancelMove();
    }
}
