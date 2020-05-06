using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunRotation : MonoBehaviour
{
    public float rotationSpeed = 10f;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(Vector3.right, Time.deltaTime * rotationSpeed);
    }
}
