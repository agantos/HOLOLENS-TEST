using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageScript : MonoBehaviour
{
    public GameObject topRow, bottomRow;
    List<GameObject> instances = new List<GameObject>();

    public GameObject AddElement(GameObject Prefab)
    {
        GameObject instance;
        if(instances.Count >= 3)
        {
            instance = bottomRow.GetComponent<RowScript>().AddElement(Prefab);
            instances.Add(instance);
        }
        else
        {
            instance = topRow.GetComponent<RowScript>().AddElement(Prefab);
            instances.Add(instance);
        }

        return instance;
    }

    public bool IsFull()
    {
        return instances.Count == 6;
    }
}
