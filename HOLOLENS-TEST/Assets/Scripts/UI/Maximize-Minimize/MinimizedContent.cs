using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class MinimizedContent : MonoBehaviour
{
    public Button button;
    public TMP_Text title;
    public RectTransform background;

    Vector3 currentScale;

    public void SetTitle(string text)
    {
        title.text = text;

        float parentWidth = gameObject.transform.parent.GetComponent<RectTransform>().rect.width;
        float buttonWidth = button.GetComponent<RectTransform>().rect.width;

        title.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,  parentWidth);
        background.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, parentWidth);
    }

    public void Despawn()
    {
        currentScale = gameObject.transform.localScale;
        gameObject.transform.localScale = Vector3.zero;
    }

    public void Spawn(string labelText)
    {
        if (labelText != title.text)
            SetTitle(labelText);
        gameObject.transform.localScale = currentScale;
    }

    public void AddButtonFunction(UnityAction call)
    {
        button.onClick.AddListener(call);
    }
}
