using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowScript : MonoBehaviour
{
    List<GameObject> list = new List<GameObject>();
    public float x_left= -155, x_middle = -6.9f, x_right = 143.9f;

    bool isInitialized = false;
    float[] positions = new float[3];

    public void Initialize()
    {
        positions[0] = x_left;
        positions[1] = x_middle;
        positions[2] = x_right;
    }

    public GameObject AddElement(GameObject Prefab)
    {
        if (!isInitialized)
            Initialize();

        GameObject instance = Instantiate(Prefab, transform);
        instance.transform.localPosition = new Vector3(positions[list.Count], 0, 0);
        list.Add(instance);

        return instance;
    }

    public bool IsFull()
    {
        return list.Count == 3;
    }

    public void ClearState()
    {
        foreach(GameObject obj in list)
        {
            Destroy(obj);
        }

        list.Clear();
    }
}
