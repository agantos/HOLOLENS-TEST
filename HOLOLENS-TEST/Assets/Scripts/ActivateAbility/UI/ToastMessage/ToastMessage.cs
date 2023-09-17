using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
public class ToastMessage : MonoBehaviour
{
    public TMP_Text text;

    void Start()
    {
    }

    public void SetText(string s)
    {
        text.text = s;
        text.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, s.Length * 10);
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }

    public void DestroyInSeconds(int s)
    {
        Invoke("DestroyObject", s);
    }
}

