using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MinimizeAndMaxmizeUI : MonoBehaviour
{
    public string label;
    Vector3 scale;

    public GameObject minimizedLabelPrefab;
    public GameObject maximizeButtonPrefab;

    GameObject minimizedLabelInstance = null;
    GameObject maximizeButtonInstance = null;

    public void Minimize()
    {
        scale = transform.localScale;
        transform.localScale = new Vector3(0, 0, 0);

        CreateMaximizeButton();
        CreateMinimizedLabel();
    }

    public void Maximize()
    {
        transform.localScale = scale;
        minimizedLabelInstance.SetActive(false);
        maximizeButtonInstance.SetActive(false);
    }

    void CreateMaximizeButton()
    {
        if (maximizeButtonInstance == null)
            maximizeButtonInstance = Instantiate(maximizeButtonPrefab);
    }

    void CreateMinimizedLabel()
    {
        if(minimizedLabelInstance == null)
        {
            minimizedLabelInstance = Instantiate(minimizedLabelPrefab);
            var text = minimizedLabelInstance.GetComponent<TextMeshProUGUI>();
            text.text = label;
        }

        minimizedLabelInstance.SetActive(true);      
    }
}
