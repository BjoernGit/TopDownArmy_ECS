using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReduceGravity : MonoBehaviour
{
    private float duration = 5f;
    private Rigidbody rb;

    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        ResetGravityTimer();
    }

    public void ResetGravityTimer()
    {
        duration = 5f;
        rb.useGravity = false;
    }

    private void Update()
    {
        if (duration <= 0f)
        {
            rb.useGravity = true;
        }
        else
        {
            duration -= Time.deltaTime;
        }
    }
}
