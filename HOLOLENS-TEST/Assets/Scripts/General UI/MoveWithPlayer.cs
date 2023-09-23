using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithPlayer : MonoBehaviour
{

    Vector3 startingPosition;
    Quaternion startingRotation;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        startingRotation = transform.rotation;
    }
    
    // Update is called once per frame
    void Update()
    {
        cam = FindAnyObjectByType<Camera>();

    }

    void OnGUI()
    {
        transform.position = startingPosition + cam.transform.position;
        transform.rotation = startingRotation * cam.transform.rotation;        
    }
}
