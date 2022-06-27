using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;
    public float smoothTime;
    public Vector3 cameraOffset; //Started with (0.25, 0.25, -2.63)
    private Vector3 velocity = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, playerTransform.position + cameraOffset, ref velocity, smoothTime); 
    }
}
