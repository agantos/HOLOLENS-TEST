using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BarInfo : MonoBehaviour
{
    public RawImage icon;
    public TMP_Text statName;
    public TMP_Text value;

    public void SetIcon(string iconPath)
    {
        icon.texture = Resources.Load<Texture2D>(iconPath);
    }

    public void SetStatName(string name)
    {
        statName.text = name;
    }

    public void SetValue(int minValue, int maxValue)
    {
        value.text = minValue + "/" + maxValue;
    }

}
