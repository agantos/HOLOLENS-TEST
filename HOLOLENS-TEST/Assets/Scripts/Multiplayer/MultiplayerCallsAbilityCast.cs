using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class MultiplayerCallsAbilityCast : MonoBehaviourPunCallbacks
{
    public static MultiplayerCallsAbilityCast Instance { get; private set; }

    public static MultiplayerCallsAbilityCast GetInstance()
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
    void SyncCastingAbilityManagers(string ablityToCast, string attackerName, string[] defenders,
                                                string[] applicationData, bool[] abilitySuccessList,
                                                AbilitySelectType abilitySelectType,
                                                Vector3 radiusSelectPosition = default(Vector3),
                                                Vector3 radiusSelectRotation = default(Vector3))
    {
        CastingAbilityManager.GetInstance().SyncManagerData(ablityToCast, attackerName, 
                                                            defenders, applicationData, 
                                                            abilitySuccessList,
                                                            abilitySelectType,
                                                            radiusSelectPosition, radiusSelectRotation
        );
    }

    [PunRPC]
    void RemoteActivateAbility()
    {
        CastingAbilityManager.Instance.ActivateAbility_Remotely();
    }

    public void Propagate_RemoteActivateAbility()
    {
        photonView.RPC(
            "RemoteActivateAbility",
            RpcTarget.Others
       );
    }

    public void Propagate_AbilityManagerSync(string ablityToCast, string attackerName, string[] defenders,
                                                string[] applicationData, bool[] abilitySuccessList,
                                                AbilitySelectType abilitySelectType,
                                                Vector3 radiusSelectPosition = default(Vector3),
                                                Vector3 radiusSelectRotation = default(Vector3)
                                                
    )
    {
        photonView.RPC(
            "SyncCastingAbilityManagers",
            RpcTarget.Others,
            ablityToCast,
            attackerName,
            defenders,
            applicationData,
            abilitySuccessList,
            radiusSelectPosition,
            radiusSelectRotation,
            abilitySelectType
       );
    }
}
