using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BaseStat : MonoBehaviour
{
    public TMP_Text statName;
    public TMP_Text value;

    public void SetStatName(string s)
    {
        statName.text = s;
    }

    public void SetValue(int i)
    {
        value.text = i.ToString();
    }
}
