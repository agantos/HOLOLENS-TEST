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
        CharacterMoveManager.Instance.PerformMove();
        gameObject.SetActive(false);
    }

    public void CancelMovement()
    {
        CharacterMoveManager.Instance.CancelMove();
        gameObject.SetActive(false);
    }
}
