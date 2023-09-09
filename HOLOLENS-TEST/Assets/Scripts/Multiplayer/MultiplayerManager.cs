using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class MultiplayerManager : MonoBehaviourPunCallbacks
{
    void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            return;
        }
        Debug.LogFormat("PhotonNetwork : Loading Level");
        PhotonNetwork.LoadLevel("TESTSCENE");
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom()", other.NickName); // not seen if you're the player connecting

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom                                                                                                     //Start Counting Turns

            GameManager.GetInstance().FirstTurn();
        }
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
        }
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    [PunRPC]
    public void PrintSomething()
    {
        if (CastingAbilityManager.GetInstance().attacker != null)
            Debug.Log(CastingAbilityManager.GetInstance().attacker.name);
        else
            Debug.Log("Attacker = null");
    }

    public void CallRPCPrint()
    {
        string[] array;
        photonView.RPC("PrintSomething", RpcTarget.All);
    }    
}


public class MultiplayerCallsAbilityCast : MonoBehaviourPunCallbacks
{
    public static MultiplayerCallsAbilityCast Instance { get; private set; }

    public static MultiplayerCallsAbilityCast GetInstance()
    {
        return Instance;
    }

    private void Awake()
    {
        // Ensure only one instance of GameManager exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate GameManager objects
        }
    }

    [PunRPC]
    void SyncCastingAbilityManagers(string ablityToCast, string attackerName, string[] defenders)
    {
        CastingAbilityManager.GetInstance().SyncManagerData(ablityToCast, attackerName, defenders);
    }

    [PunRPC]
    void RemoteActivateAbility()
    {
        CastingAbilityManager.Instance.ActivateAbilityRemotely();
    }

    public void Propagate_RemoteActivateAbility()
    {
        photonView.RPC(
            "RemoteActivateAbility",
            RpcTarget.Others
       );
    }

    public void Propagate_AbilityManagerSync(string ablityToCast, string attackerName, string[] defenders)
    {
        photonView.RPC(
            "SyncCastingAbilityManagers",
            RpcTarget.Others,
            ablityToCast,
            attackerName,
            defenders
       );
    }
}