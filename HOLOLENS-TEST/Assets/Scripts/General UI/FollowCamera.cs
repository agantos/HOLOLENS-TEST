using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    Camera cam;
    Vector3 startingPosition;
    Quaternion startingRotation;

    public bool RotateWithCamera = true;
    public bool MoveWithCamera = true;

    // Start is called before the first frame update
    void Start()
    {
        cam = FindAnyObjectByType<Camera>();
        startingPosition = transform.position;
        startingRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        if(MoveWithCamera )
            transform.position = startingPosition + cam.transform.position;

        if (RotateWithCamera)
            transform.rotation = startingRotation * cam.transform.rotation;
    }
}
