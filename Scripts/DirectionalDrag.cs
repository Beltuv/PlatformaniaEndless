using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalDrag : MonoBehaviour
{
    public float x;
    public float y;
    public float z;
    Rigidbody2D rb;
    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
    }
     
    void Update ()
    {
        Vector3 vel = transform.InverseTransformDirection(rb.velocity);
        vel.x*= x;
        vel.y*= y;
        vel.z*= z;
        rb.AddForce(transform.TransformDirection(vel));
    }
}
