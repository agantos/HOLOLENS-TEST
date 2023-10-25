using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    Camera cam;
    Vector3 startingPosition;
    Quaternion startingRotation;

    private Vector3 offset;

    public bool RotateWithCamera = true;
    public bool MoveWithCamera = true;

    // Start is called before the first frame update
    void Start()
    {
        cam = FindAnyObjectByType<Camera>();
        offset = transform.position - cam.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        if(MoveWithCamera )
            transform.position = cam.transform.position + offset;

        if (RotateWithCamera)
            transform.rotation = cam.transform.rotation;
    }
}
