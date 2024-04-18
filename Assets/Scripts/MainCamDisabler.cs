using UnityEngine;

public class MainCamDisabler : MonoBehaviour
{
    GameObject mainCam;
    GameObject uiMenu;
    void Start()
    {
        mainCam = GameObject.Find("Main Camera");
        uiMenu = GameObject.Find("Main Menu");
    }
    void Update()
    {
        if (GetComponent<Camera>().enabled == true)
        {
            uiMenu.SetActive(false);
            mainCam.GetComponent<Camera>().enabled = false;
        }
        else
        {
            uiMenu.SetActive(true);
            mainCam.GetComponent<Camera>().enabled = true;
        }
    }
}
