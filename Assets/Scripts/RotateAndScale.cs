using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAndScale : MonoBehaviour
{
    private readonly float rotationSpeed = 150f;
    public bool doRotationAndScale = true;

    private Vector3 normalSize;

    private void Start()
    {
        normalSize = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        if (doRotationAndScale)
        {
            RotationAndScale();
        }
    }

    public void StopRotationAndScale()
    {
        doRotationAndScale = false;
    }

    void RotationAndScale()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * rotationSpeed);
        transform.localScale = normalSize * (1.5f + 0.3f * Mathf.Sin(Time.time * 3f));
        //transform.localScale = new Vector3(0.08f + 0.01f * Mathf.Sin(Time.time * 4f), 0.08f + 0.01f * Mathf.Sin(Time.time * 4f), 0.08f + 0.01f * Mathf.Sin(Time.time * 4f));
    }


}