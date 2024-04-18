using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllModelsContainer : MonoBehaviour
{
    public AnimationClip[] animClip;
    [SerializeField] GameObject camObject, mainModel;
    public GameObject[] models;
    public List<MeshRenderer> modelsOff;
    public int objectToMoveNum;
    public bool moveToCam, clickedMoveToCam;
    [SerializeField] Transform target, specCamPoint;
    [SerializeField] float speed;
    Vector3 startPosOfObj, startPosOfCam;
    Quaternion startRotOfObj, startRotOfCam;
    //float lastCamSize;
    public Component[] renderers;
    [SerializeField] Transform[] parents;
    private void Start()
    {
        for(int i=0;i <= parents.Length - 1; i++)
        {
            renderers = parents[i].GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer rends in renderers)
                modelsOff.Add(rends);
        }
    }
    private void Update()
    {
        if (clickedMoveToCam == true) ClickedGoToCam();
        if (moveToCam == true) GoToCamera();
    }

    void GoToCamera()
    {
        //camObject.GetComponent<Camera>().orthographicSize = 2;
        specCamPoint.LookAt(mainModel.transform);
        camObject.GetComponent<Camera>().fieldOfView = 90;
        camObject.transform.position = specCamPoint.position;
        camObject.transform.rotation = specCamPoint.rotation;
        models[objectToMoveNum].transform.position = Vector3.MoveTowards(models[objectToMoveNum].transform.position, target.position, speed * Time.deltaTime);
        if (models[objectToMoveNum].transform.position == target.position) moveToCam = false;
    }

    void ClickedGoToCam()
    {
        GetComponent<Animator>().enabled = false;
        Transform specPosNew = camObject.GetComponent<ObjectContainer>().positionsLookAtObj[objectToMoveNum].transform;
        specCamPoint.position = new Vector3(specPosNew.position.x, specPosNew.position.y, specPosNew.position.z);
        //models[objectToMoveNum].transform.LookAt(target);
        //lastCamSize = camObject.GetComponent<Camera>().orthographicSize;
        startPosOfCam = camObject.transform.position;
        startRotOfCam = camObject.transform.rotation;
        startPosOfObj = models[objectToMoveNum].transform.position;
        startRotOfObj = models[objectToMoveNum].transform.rotation;
        camObject.GetComponent<CameraControl>().enabled = false;
        camObject.GetComponent<LookObject>().objectToControl = models[objectToMoveNum];
        camObject.GetComponent<LookObject>().enabled = true;
        moveToCam = true;
        clickedMoveToCam = false;
    }

    public void GoBackHome()
    {
        camObject.GetComponent<Camera>().fieldOfView = 90;
        GetComponent<Animator>().enabled = true;
        camObject.GetComponent<CharacterController>().enabled = false;
        camObject.transform.position = startPosOfCam;
        camObject.transform.rotation = startRotOfCam;
        //models[objectToMoveNum].GetComponent<Animator>().Play()
        //camObject.GetComponent<Camera>().orthographicSize = lastCamSize;
        camObject.GetComponent<LookObject>().enabled = false;
        camObject.GetComponent<LookObject>().objectToControl = null;
        models[objectToMoveNum].transform.position = startPosOfObj;
        models[objectToMoveNum].transform.rotation = startRotOfObj;
        camObject.GetComponent<CameraControl>().enabled = true;
        moveToCam = false;
        //camObject.GetComponent<ObjectContainer>().InfoObjects[objectToMoveNum].SetActive(!camObject.GetComponent<ObjectContainer>().InfoObjects[objectToMoveNum].activeSelf);
        camObject.GetComponent<CharacterController>().enabled = true;
    }

}
