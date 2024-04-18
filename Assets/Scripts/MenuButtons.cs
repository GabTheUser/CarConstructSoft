using UnityEngine;
using UnityEngine.UI;
public class MenuButtons : MonoBehaviour
{
    public GameObject buttonContainer, containerOnButton, closeObjectControlButtons, ExitMenu;
    [Header("Cameras")] [SerializeField] GameObject[] cameras;
    [Header("Back and Info Button Scr")] 
    public ButtonScr infoButton;
    public ButtonScr backButton;

    [SerializeField] GameObject intructObj;
    [SerializeField] Text bGText;
    bool changeBG, instructionsOn;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CallExitMenu();
        }
    }

    public void TurnOnButtonMenu()
    {
        buttonContainer.SetActive(true);
        containerOnButton.SetActive(false);
    }
    public void TurnOffButtonMenu()
    {
        buttonContainer.SetActive(false);
        containerOnButton.SetActive(true);
    }

    public void ChangeBG()
    {
        changeBG = !changeBG;
        if (changeBG == true)
        {
            cameras[0].GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
            cameras[1].GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
            bGText.text = "Solid Color";
        }
        else
        {
            cameras[0].GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
            cameras[1].GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
            bGText.text = "Squares";
        }
    }

    public void OnOffInstruct()
    {
        instructionsOn = !instructionsOn;
        intructObj.SetActive(instructionsOn);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void DoNotExit()
    {
        ExitMenu.SetActive(false);
    }

    public void CallExitMenu()
    {
        ExitMenu.SetActive(!ExitMenu.activeSelf);
    }
}
