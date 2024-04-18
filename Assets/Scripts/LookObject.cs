using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookObject : MonoBehaviour
{
    public GameObject objectToControl;

    [SerializeField] float speed;
    [SerializeField] float cameraSpd;
    [SerializeField] float mouseYBlocker;
    [SerializeField] Transform target;
    [SerializeField] float zoomSpeed;
    [SerializeField] float fovMin, orthographicSizeMin;
    [SerializeField] float fovMax, orthographicSizeMax;

    Camera myCamera;
    CharacterController controller;
    Vector3 moveDirection = Vector3.zero;

    private void Start()
    {
        myCamera = GetComponent<Camera>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        //ControlWithKeys();
        if (Input.GetAxis("Mouse ScrollWheel") > 0.01f
            || Input.GetAxis("Mouse ScrollWheel") < -0.01f) ZoomInOut();
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(1)) ControlWithMouse();
        if (Input.GetMouseButton(2)) MoveCameraWith3();
    }

    void ControlWithKeys() // move camera with keys
    {
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;
        controller.Move(moveDirection * Time.deltaTime);
    }

    void ControlWithMouse() // rotate object with mouse
    {
        if (Input.GetAxis("Mouse Y") > mouseYBlocker
            || Input.GetAxis("Mouse Y") < -mouseYBlocker)
            objectToControl.transform.RotateAround(target.position, target.right, -Input.GetAxis("Mouse Y") * cameraSpd * Time.fixedDeltaTime);

        objectToControl.transform.RotateAround(target.position, target.up, -Input.GetAxis("Mouse X") * cameraSpd * Time.fixedDeltaTime);
    }


    void MoveCameraWith3() // move camera at X and Y with scrollWheel
    {
        if (Input.GetAxis("Mouse X") > 0) transform.position += new Vector3(Input.GetAxisRaw("Mouse X") * Time.fixedDeltaTime * speed,
                                                                            Input.GetAxisRaw("Mouse Y") * Time.fixedDeltaTime * speed, 0);
        else if (Input.GetAxis("Mouse X") < 0) transform.position += new Vector3(Input.GetAxisRaw("Mouse X") * Time.fixedDeltaTime * speed,
                                                                                 Input.GetAxisRaw("Mouse Y") * Time.fixedDeltaTime * speed, 0);
    }

    void ZoomInOut()
    {
        if (myCamera.orthographic)
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0) myCamera.orthographicSize += zoomSpeed;
            if (Input.GetAxis("Mouse ScrollWheel") > 0) myCamera.orthographicSize -= zoomSpeed;
            myCamera.orthographicSize = Mathf.Clamp(myCamera.orthographicSize, orthographicSizeMin, orthographicSizeMax);
        }
        else
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0) myCamera.fieldOfView += zoomSpeed;
            if (Input.GetAxis("Mouse ScrollWheel") > 0) myCamera.fieldOfView -= zoomSpeed;
            myCamera.fieldOfView = Mathf.Clamp(myCamera.fieldOfView, fovMin, fovMax);
        }
    }
}
