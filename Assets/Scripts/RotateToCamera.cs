using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToCamera : MonoBehaviour
{
    private GameObject cam;

    // Start is called before the first frame update
    void Start()
    {
        FindNewCam();
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = cam.transform.rotation;
    }

    public void FindNewCam()
    {
        cam = GameObject.Find("Main Camera");
    }
}
