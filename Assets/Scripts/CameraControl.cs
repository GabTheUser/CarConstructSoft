using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float cameraSpd;
    [SerializeField] float cameraSpdXY;
    [SerializeField] Transform target;
    [SerializeField] Transform[] cameraSides;
    [SerializeField] float mouseYBlocker;
    [SerializeField] float zoomSpeed;
    [SerializeField] float fovMin, orthographicSizeMin;
    [SerializeField] float fovMax, orthographicSizeMax;
    public int currentClickedObj;

    Camera myCamera;
    CharacterController controller;
    Vector3 moveDirection = Vector3.zero;
    Vector3[] camPositions = new Vector3[23];
    Quaternion[] camRotations = new Quaternion[23];
    [SerializeField] Vector3[] posAddObj;
    [SerializeField] Quaternion[] rotAddObj;
    [HideInInspector] public bool cameraIsWorking;

    AllModelsContainer allModelCont;

    bool outlined;
    [HideInInspector] public int controlingItem;
    private void Start()
    {
        allModelCont = FindObjectOfType<AllModelsContainer>();
        controller = GetComponent<CharacterController>();
        myCamera = GetComponent<Camera>();
        for(int i=0; i < cameraSides.Length; i++) // get position / rotation of side views
        {
            camPositions[i] = new Vector3(cameraSides[i].position.x, cameraSides[i].position.y, cameraSides[i].position.z);
            camRotations[i] = Quaternion.Euler(cameraSides[i].eulerAngles.x, cameraSides[i].eulerAngles.y, cameraSides[i].eulerAngles.z);
        }
        for (int i = 0; i < GetComponent<ObjectContainer>().positionsWhenAddObj.Length; i++) // get position / rotation of side views when object is being added
        {
            posAddObj[i] = new Vector3(GetComponent<ObjectContainer>().positionsWhenAddObj[i].transform.position.x,
                                       GetComponent<ObjectContainer>().positionsWhenAddObj[i].transform.position.y, 
                                       GetComponent<ObjectContainer>().positionsWhenAddObj[i].transform.position.z);
            rotAddObj[i] = Quaternion.Euler(GetComponent<ObjectContainer>().positionsWhenAddObj[i].transform.eulerAngles.x, 
                                            GetComponent<ObjectContainer>().positionsWhenAddObj[i].transform.eulerAngles.y,
                                            GetComponent<ObjectContainer>().positionsWhenAddObj[i].transform.eulerAngles.z);
        }
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            GetComponent<Camera>().enabled = false;
            allModelCont.GetComponent<Animator>().Play(allModelCont.animClip[0].name);
        }
        ControlWithKeys();
        if (   Input.GetKey(KeyCode.Alpha1) 
            || Input.GetKey(KeyCode.Alpha2)
            || Input.GetKey(KeyCode.Alpha3) 
            || Input.GetKey(KeyCode.Alpha4)) SetCameraView();

        if (   Input.GetAxis("Mouse ScrollWheel") > 0.01f 
            || Input.GetAxis("Mouse ScrollWheel") < -0.01f) ZoomInOut();
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(1)) ControlWithMouse();
        if (Input.GetMouseButton(2)) MoveCameraWith3();
    }

    void ControlWithKeys() // move camera with keys
    {
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;
        controller.Move(moveDirection * Time.deltaTime);
    }

    void ControlWithMouse() // rotate camera with mouse
    {
        if (   Input.GetAxis("Mouse Y") > mouseYBlocker 
            || Input.GetAxis("Mouse Y") < -mouseYBlocker)
            transform.RotateAround(target.position, target.right, -Input.GetAxis("Mouse Y") * cameraSpd * Time.fixedDeltaTime);

        transform.RotateAround(target.position, target.up, -Input.GetAxis("Mouse X") * cameraSpd * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0);
    }

    void SetCameraView() //set front/side/top view/isometric
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            transform.rotation = camRotations[0];
            transform.position = camPositions[0];
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            transform.rotation = camRotations[1];
            transform.position = camPositions[1];
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            transform.rotation = camRotations[2];
            transform.position = camPositions[2];
        }
        if (Input.GetKey(KeyCode.Alpha4))
        {
            transform.rotation = camRotations[3];
            transform.position = camPositions[3];
        }
    }

    void MoveCameraWith3() // move camera at X and Y with scrollWheel
    {
        if (Input.GetAxis("Mouse X") > 0) transform.position += new Vector3(Input.GetAxisRaw("Mouse X") * Time.fixedDeltaTime * cameraSpdXY,
                                                                            Input.GetAxisRaw("Mouse Y") * Time.fixedDeltaTime * cameraSpdXY, 0);
        else if (Input.GetAxis("Mouse X") < 0) transform.position += new Vector3(Input.GetAxisRaw("Mouse X") * Time.fixedDeltaTime * cameraSpdXY, 
                                                                                 Input.GetAxisRaw("Mouse Y") * Time.fixedDeltaTime * cameraSpdXY, 0);
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

    public void ChangeCamPos(int clickedNum)
    {
        controlingItem = clickedNum;

        GetComponent<CharacterController>().enabled = false;
        transform.position = posAddObj[clickedNum];
        transform.rotation = rotAddObj[clickedNum];

        GetComponent<Camera>().enabled = false;
        GetComponent<Camera>().fieldOfView = 90;
        if (allModelCont.models[clickedNum] != null)
        {
            allModelCont.models[clickedNum].SetActive(true);
        }

        if (allModelCont.animClip[clickedNum] != null)
        {
            allModelCont.GetComponent<Animator>().Play(allModelCont.animClip[clickedNum].name);
        }

        if (transform.position == posAddObj[clickedNum] && transform.rotation == rotAddObj[clickedNum])
        {
            GetComponent<CharacterController>().enabled = true;
        }
    }

    public void TurnOnOffOutline()
    {
        outlined = !outlined;
        List<MeshRenderer> meshesNotToChange = new List<MeshRenderer>();

        if (controlingItem != 2 && controlingItem != 4 && controlingItem != 2
            && controlingItem != 6 && controlingItem != 8 && controlingItem != 18
            && controlingItem != 21 && controlingItem != 23)
        {
            for (int c = 0; c <= allModelCont.models[controlingItem].GetComponent<RenderCollecter>().myRenders.Length - 1; c++)
            {
                meshesNotToChange.Add(allModelCont.models[controlingItem].GetComponent<RenderCollecter>().myRenders[c]);
            }
            if (outlined == true)
            {
                for (int i = 0; i < meshesNotToChange.Count; i++)
                {
                    if (meshesNotToChange[i] != null)
                        meshesNotToChange[i].gameObject.GetComponent<Outline>().enabled = true;
                }
            }
            else
            {
                for (int i = 0; i < meshesNotToChange.Count; i++)
                {
                    if (meshesNotToChange[i] != null)
                        meshesNotToChange[i].gameObject.GetComponent<Outline>().enabled = false;
                }
            }
        }
        else
        {
            int newColItem = controlingItem - 1;
            for (int c = 0; c <= allModelCont.models[newColItem].GetComponent<RenderCollecter>().myRenders.Length - 1; c++)
            {
                meshesNotToChange.Add(allModelCont.models[newColItem].GetComponent<RenderCollecter>().myRenders[c]);
            }
            if (outlined == true)
            {
                for (int i = 0; i < meshesNotToChange.Count; i++)
                {
                    if (meshesNotToChange[i] != null)
                        meshesNotToChange[i].gameObject.GetComponent<Outline>().enabled = true;
                }
            }
            else
            {
                for (int i = 0; i < meshesNotToChange.Count; i++)
                {
                    if (meshesNotToChange[i]!=null)
                    meshesNotToChange[i].gameObject.GetComponent<Outline>().enabled = false;
                }
            }
        }
    }
}
