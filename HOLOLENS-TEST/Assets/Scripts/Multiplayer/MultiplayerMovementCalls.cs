using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class MultiplayerMovementCalls : MonoBehaviourPunCallbacks
{
    public static MultiplayerMovementCalls Instance { get; private set; }

    public static MultiplayerMovementCalls GetInstance()
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

    [PunRPC]
    void PerformRemoteMovement(float x, float y, float z, string name, float distance)
    {
        CharacterMover.Instance.SyncMovementData(x, y, z, name, distance);
        CharacterMover.Instance.PerformMoveRemotely();
    }

    public void Propagate_Movement(float x, float y, float z, string name, float distance)
    {
        photonView.RPC(
            "PerformRemoteMovement", 
            RpcTarget.Others, 
            x, y, z,
            name, 
            distance
        );
    }
}
