using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
//Handles minimizing and closing of a window
public class WindowControlView : MonoBehaviour
{
    //Set in Editor
    public GameObject minimizableView;
    public GameObject maximizeButton;

    public void Start()
    {
        maximizeButton.SetActive(false);
    }

    public void Mimimize()
    {
        //Hiding view
        minimizableView.SetActive(false);

        //Disabling colliders so they do not obstruct
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.GetComponent<BoundsControl>().enabled = false;

        //Spawn Maximize Button
        maximizeButton.SetActive(true);
    }

    public void Maximize()
    {
        minimizableView.SetActive(true);

        gameObject.GetComponent<Collider>().enabled = true;
        gameObject.GetComponent<BoundsControl>().enabled = true;

        //Despawn Maximize Button
        maximizeButton.SetActive(false);
    }

    public void CloseWindow()
    {
        Destroy(gameObject);
    }
}
