using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//Singleton Button that cancels the activation of current selected ability
public class CancelAbilityButton : Button
{

    public void Deactivate()
    {
        this.gameObject.SetActive(false);
    }

    public void Activate()
    {
        this.gameObject.SetActive(true);
    }

    public void DeactivateAbility()
    {
        CastingAbilityManager.GetInstance().CancelActivation();
    }
}
