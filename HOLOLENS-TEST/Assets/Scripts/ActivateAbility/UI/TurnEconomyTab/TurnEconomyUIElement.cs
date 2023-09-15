using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Microsoft.MixedReality.Toolkit.UI;

public class TurnEconomyUIElement : MonoBehaviour
{
    public Interactable button;
    public TMP_Text usesText, nameText;

    //Sets the text of the uses text field
    public void SetUses(int current, int max)
    {
        string display = current + " / " + max;
        usesText.text = (display);
    }

    //Sets the image of the button
    void SetButtonImageGameObject()
    {

    }

    public void SetButtonOnClick(UnityEngine.Events.UnityAction method)
    {
        button.OnClick.AddListener(method);
    }

    public void SetNameGameobject(string name)
    {
        nameText.SetText(name);
    }
}
