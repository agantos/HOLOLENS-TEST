using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
public class KeyAbilityInfo : Button
{

    public RawImage image;
    public TMP_Text abilityName;

    string path = "UI/AbilitiesImages";

    public void SetImage(string imageName)
    {
        image.texture = Resources.Load<Texture2D>(path + imageName);
    }

    public void SetName(string s)
    {
        abilityName.text = s;
    }

    public void SetOnClick(UnityAction action)
    {
        onClick.AddListener(action);
    }
}
