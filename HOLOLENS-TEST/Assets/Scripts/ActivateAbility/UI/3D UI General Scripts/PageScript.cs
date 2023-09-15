using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageScript : MonoBehaviour
{
    public RowScript topRow, bottomRow;
    public GameObject AddElement(GameObject Prefab)
    {
        GameObject instance;
        if(topRow.IsFull())
        {
            instance = bottomRow.GetComponent<RowScript>().AddElement(Prefab);
        }
        else
        {
            instance = topRow.GetComponent<RowScript>().AddElement(Prefab);
        }

        return instance;
    }

    public bool IsFull()
    {
        return (topRow.IsFull() && bottomRow.IsFull());
    }

    public void ClearState()
    {
        topRow.ClearState();
        bottomRow.ClearState();

        Destroy(topRow);
        Destroy(bottomRow);
    }
}
