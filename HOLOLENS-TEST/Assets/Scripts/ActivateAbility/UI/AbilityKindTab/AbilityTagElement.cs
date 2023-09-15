using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

using Microsoft.MixedReality.Toolkit.UI;

public class AbilityTagElement : MonoBehaviour
{
    public Interactable button;
    public TMP_Text nameText;
    public string abilityTag;

    public void SetNameGameobject(string name)
    {
        nameText.SetText(name);
    }

    public void SetButtonOnClick(UnityEngine.Events.UnityAction method)
    {
        button.OnClick.AddListener(method);
    }
}
