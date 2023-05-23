using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiusSelectScript : MonoBehaviour
{
    public GameObject selectToken;
    public int radius;
    //should represent what 1 feet is in the game
    public float baseRadiusX, baseRadiusY, baseRadiusZ;

    // Start is called before the first frame update
    void Start()
    {

        SetScale();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetScale()
    {
        gameObject.transform.localScale = new Vector3(radius * baseRadiusX, radius * baseRadiusY, radius * baseRadiusZ);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            GameObject token = Object.Instantiate(selectToken);            
            token.transform.SetParent(other.transform.parent.transform, false);
            float playerHeight = other.transform.localPosition.y + other.transform.localScale.y / 2;
            token.transform.localPosition += new Vector3(0, playerHeight + 0.1f, 0);
        }

    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Destroy(other.gameObject.transform.parent.transform.Find("IsSelected(Clone)").gameObject);
        }
        
    }

}
