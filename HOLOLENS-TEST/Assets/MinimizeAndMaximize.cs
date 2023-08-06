using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimizeAndMaximize : MonoBehaviour
{
    string labelText;
    
    public GameObject minimizeButtonPrefab;
    public GameObject maximizeButtonPrefab;
    public GameObject labelPrefab;

    Vector3 currentScale;

    GameObject maximizeButtonInstance = null;
    GameObject minimizeButtonInstance = null;
    GameObject labelInstance = null;

    void Start()
    {
        //Create Minimize button
        SpawnObject(minimizeButtonPrefab, ref minimizeButtonInstance);
        minimizeButtonInstance.GetComponent<Button>().onClick.AddListener(SpawnMinimizedView);
        PositionButton(minimizeButtonInstance);

        //Create maximize button and disable it
        SpawnObject(maximizeButtonPrefab, ref maximizeButtonInstance);
        maximizeButtonInstance.GetComponent<Button>().onClick.AddListener(SpawnMaximizedView);
        PositionButton(maximizeButtonInstance);
        maximizeButtonInstance.SetActive(false);
    }

    public void SpawnMinimizedView()
    {
        //Despawn minimizable UI
        currentScale = gameObject.transform.localScale;
        gameObject.transform.localScale = Vector3.zero;
        minimizeButtonInstance.SetActive(false);

        //Spawn maximize button and the label
        SpawnObject(maximizeButtonPrefab, ref maximizeButtonInstance);
        SpawnObject(labelPrefab, ref labelInstance);
    }

    public void SpawnMaximizedView()
    {
        //Spawn the minimized UI
        gameObject.transform.localScale = currentScale;
        SpawnObject(minimizeButtonPrefab, ref minimizeButtonInstance);

        //Despawn maximizeButton and the label
        maximizeButtonInstance.SetActive(false);
        labelInstance.SetActive(false);
    }

    void SpawnObject(GameObject prefab, ref GameObject instance)
    {
        if (instance == null)
        {
            instance = Instantiate(prefab, transform.parent);
        }

        instance.SetActive(true);
    }

    void PositionButton(GameObject buttonInstance)
    {
        Rect canvasRect = transform.parent.GetComponent<RectTransform>().rect;
        Rect buttonRect = minimizeButtonInstance.GetComponent<RectTransform>().rect;

        float x = canvasRect.width / 2 + buttonRect.width / 2 + 10;
        float y = canvasRect.height / 2 - buttonRect.height / 2;
        float z = minimizeButtonInstance.GetComponent<RectTransform>().localPosition.z;
        Debug.Log(new Vector3(x, y, z));

        buttonInstance.GetComponent<RectTransform>().localPosition = new Vector3(x, y, z);
    }
}
