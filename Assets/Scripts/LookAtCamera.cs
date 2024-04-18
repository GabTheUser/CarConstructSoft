using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Transform mainCam;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.Find("Main Camera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(mainCam, Vector3.up);
    }
}
