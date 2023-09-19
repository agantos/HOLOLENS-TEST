using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActivateAbilityUIManager : MonoBehaviour
{
    public TextMeshPro title;

    Vector3 scale;

    // Start is called before the first frame update
    void Start()
    {
        scale = gameObject.transform.localScale;
    }

    public void ActivateAbility()
    {
        CastingAbilityManager.GetInstance().ActivateAbility();
        Deactivate();
    }

    public void CancelActivation()
    {
        CastingAbilityManager.GetInstance().CancelActivation();
        Deactivate();
    }

    public void Activate()
    {
        //"Spawn" object
        gameObject.transform.localScale = scale;

        title.text = SelectAbilityUIManager.Instance.UI_Info.currentAbility;
    }

    public void Deactivate()
    {
        //"Despawn" object
        gameObject.transform.localScale = Vector3.zero;
    }

}
