using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterUI_SectionTitle : MonoBehaviour
{
    public GameObject title;

    public void SetTitle(string s)
    {
        TMP_Text text = title.GetComponent<TMP_Text>();
        text.text = s;
        title.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (text.fontSize - text.fontSize/5) * s.Length);
    }
}
