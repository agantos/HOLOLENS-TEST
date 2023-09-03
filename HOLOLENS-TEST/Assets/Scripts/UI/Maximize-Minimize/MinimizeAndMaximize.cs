using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MinimizeAndMaximize : MonoBehaviour
{
    string labelText = "Not set yet";
    
    public GameObject   minimizeButtonPrefab;
    public GameObject   MinimizedContentPrefab;
    public TMP_Text     title;

    Vector3 currentScale;

    GameObject minimizeButtonInstance = null;
    GameObject minimizedContentInstance = null;
    GameObject parentCanvas;


    void Start()
    {
        parentCanvas = transform.parent.gameObject;

        //Create Minimize button
        SpawnObject(minimizeButtonPrefab, ref minimizeButtonInstance);
        minimizeButtonInstance.GetComponent<Button>().onClick.AddListener(SpawnMinimizedView);
        PositionTopOfCanvas(minimizeButtonInstance);
        PositionRightOfCanvas(minimizeButtonInstance);

        //Create maximize button and disable it
        minimizedContentInstance = Instantiate(MinimizedContentPrefab, parentCanvas.transform);
        minimizedContentInstance.GetComponent<MinimizedContent>().AddButtonFunction(SpawnMaximizedView);
        minimizedContentInstance.GetComponent<MinimizedContent>().Despawn();        
    }

    public void SpawnMinimizedView()
    {
        //Despawn minimizable UI
        currentScale = gameObject.transform.localScale;
        gameObject.transform.localScale = Vector3.zero;
        minimizeButtonInstance.SetActive(false);

        //Spawn minimized view
        PositionTopOfCanvas(minimizedContentInstance);
        PositionMinimizedContentHorizontal();
        
        minimizedContentInstance.GetComponent<MinimizedContent>().Spawn(labelText);
        minimizedContentInstance.GetComponent<MinimizedContent>().SetTitle(title.text);
    }

    public void SpawnMaximizedView()
    {
        //Spawn the minimized UI
        gameObject.transform.localScale = currentScale;
        SpawnObject(minimizeButtonPrefab, ref minimizeButtonInstance);

        //Despawn maximizeButton and the label
        minimizedContentInstance.GetComponent<MinimizedContent>().Despawn();
    }

    void SpawnObject(GameObject prefab, ref GameObject instance)
    {
        if (instance == null)
        {
            instance = Instantiate(prefab, transform.parent);
        }

        instance.SetActive(true);
    }

    void PositionTopOfCanvas(GameObject toPosition)
    {
        Rect canvasRect = transform.parent.GetComponent<RectTransform>().rect;
        Rect toPositionRect = toPosition.GetComponent<RectTransform>().rect;

        float x = toPosition.GetComponent<RectTransform>().localPosition.x;
        float y = canvasRect.height / 2 - toPositionRect.height / 2;
        float z = toPosition.GetComponent<RectTransform>().localPosition.z;

        toPosition.GetComponent<RectTransform>().localPosition = new Vector3(x, y, z);
    }

    void PositionRightOfCanvas(GameObject toPosition)
    {
        Rect canvasRect = transform.parent.GetComponent<RectTransform>().rect;
        Rect toPositionRect = toPosition.GetComponent<RectTransform>().rect;
        
        float x = canvasRect.width / 2 + toPositionRect.width / 2 + 10;
        float y = toPosition.GetComponent<RectTransform>().localPosition.y;
        float z = toPosition.GetComponent<RectTransform>().localPosition.z;

        toPosition.GetComponent<RectTransform>().localPosition = new Vector3(x, y, z);
    }

    void PositionMinimizedContentHorizontal()
    {
        Rect canvasRect = transform.parent.GetComponent<RectTransform>().rect;
        Rect toPositionRect = minimizedContentInstance.GetComponent<RectTransform>().rect;

        float maximizeButtonWidth = minimizedContentInstance.GetComponent<MinimizedContent>().button.gameObject.GetComponent<RectTransform>().rect.width;

        float x = (10 + maximizeButtonWidth) / 2;
        float y = minimizedContentInstance.GetComponent<RectTransform>().localPosition.y;
        float z = minimizedContentInstance.GetComponent<RectTransform>().localPosition.z;

        minimizedContentInstance.GetComponent<RectTransform>().localPosition = new Vector3(x, y, z);
    }
}
