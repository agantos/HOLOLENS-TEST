using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

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
