using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class MultiplayerTurnManagementCalls : MonoBehaviourPunCallbacks
{
    public static MultiplayerTurnManagementCalls Instance { get; private set; }

    public static MultiplayerTurnManagementCalls GetInstance()
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
    void FirstTurnRemotely(float[] initiatives, string[] characters)
    {
        GameManager.GetInstance().FirstTurn_Remotely(initiatives, characters);
    }

    [PunRPC]
    void NextTurnRemotely()
    {
        GameManager.GetInstance().NextTurn_Remotely();
    }

    public void Propagate_NextTurn()
    {
        photonView.RPC(
            "NextTurnRemotely", 
            RpcTarget.Others
        );
    }


    public void Propagate_TurnManagerInformation()
    {
        photonView.RPC(
            "FirstTurnRemotely",
            RpcTarget.OthersBuffered,
            GameManager.GetInstance().turnManager.GetInitiativeNumbers(),
            GameManager.GetInstance().turnManager.GetInitiativeNames()
       );
    }
}
