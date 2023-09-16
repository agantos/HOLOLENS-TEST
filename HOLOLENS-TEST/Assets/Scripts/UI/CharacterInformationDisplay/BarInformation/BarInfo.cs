using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BarInfo : MonoBehaviour
{
    public RawImage icon;
    public Image colorIcon;
    public TMP_Text statName;
    public TMP_Text value;
    public float startingWidth;

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
        if(value)
            value.text = minValue + "/" + maxValue;

        SetColoredSize(minValue, maxValue);
    }

    public void SetColoredSize(int minValue, int maxValue)
    {
        float number = (float)minValue / (float)maxValue * startingWidth;
        colorIcon.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, number);
    }

    public void SetColor(string color) { 
        colorIcon.color = GetColor(color);
    }

    static Color GetColor(string hex)
    {
        if (hex.Length < 6)
        {
            throw new System.FormatException("Needs a string with a length of at least 6");
        }

        //RGB values
        var r = hex.Substring(0, 2);
        var g = hex.Substring(2, 2);
        var b = hex.Substring(4, 2);
        
        //Transparency "A" value
        string alpha;
        if (hex.Length >= 8)
            alpha = hex.Substring(6, 2);
        else
            alpha = "FF";

        return new Color((int.Parse(r, NumberStyles.HexNumber) / 255f),
                       (int.Parse(g, NumberStyles.HexNumber) / 255f),
                       (int.Parse(b, NumberStyles.HexNumber) / 255f),
                       (int.Parse(alpha, NumberStyles.HexNumber) / 255f));
    }

}
