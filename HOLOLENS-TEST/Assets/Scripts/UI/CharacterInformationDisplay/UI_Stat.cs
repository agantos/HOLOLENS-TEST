using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Stat : MonoBehaviour
{
    public TMP_Text statName;
    public TMP_Text value;

    public void SetStatName(string s)
    {
        if (s.Length > 5)
        {
            if (s.Contains(" "))
            {
                string[] words = s.Split(' ');  // Split the input string by space
                s = words[0].Substring(0, 3) + " " + words[1].Substring(0, 3);
                statName.text = s;
            }
            else
                statName.text = s.Substring(0, 3);
        }            
        else
            statName.text = s;
    }

    public void SetValue(int i)
    {
        value.text = i.ToString();
    }
}
