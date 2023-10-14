using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public TMP_Text text;

    public void Inititalize(string s, bool isDamage)
    {
        //Set and color the text
        string t = s;

        if (isDamage)
        {
            t = ColorString(t, AbilityDisplayColors.DAMAGE);
        }
        else
            t = ColorString(t, AbilityDisplayColors.ABILITY);

        text.text = t;

        //Position Text
        PositionText();

        //Set text to be Destroyed
        Destroy(gameObject, 0.4f);
    }

    void PositionText()
    {
        float x = 0;
        float y = 2.5f;
        float z = 0;

        gameObject.GetComponent<RectTransform>().localPosition = new Vector3(x,y,z);
    }

    public static string ColorString(string s, AbilityDisplayColors type)
    {
        string coloredString = "<color=" + AbilityDisplayGeneralMethods.Instance.colorTypesDictionary[type] + ">";
        coloredString += s;
        coloredString += "</color>";
        return coloredString;
    }
}
