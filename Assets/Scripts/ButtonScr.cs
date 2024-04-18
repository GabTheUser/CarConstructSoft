using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ButtonScr : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] string buttonType;
    [SerializeField] int buttonNum;
    [SerializeField] GameObject newSubButtonsContainer;
    public bool interactible;
    MenuButtons menuButtonsScr;
    AllModelsContainer allModelsScr;
    bool transparent;
    public bool infoIsOn;
    public ButtonScr thisButtonScr;
    private void Start()
    {
        menuButtonsScr = FindObjectOfType<MenuButtons>();
        allModelsScr = FindObjectOfType<AllModelsContainer>();
        if (buttonType == "info on")
        {
            if (infoIsOn == true)
            {
                GetComponentInChildren<Text>().color = new Color(1, 0.45f, 0, 1);
            }
            else
            {
                GetComponentInChildren<Text>().color = new Color(1, 1, 1, 1);
            }
        }
    }
    private void OnEnable()
    {
        if (buttonType == "info on")
        {
            if (infoIsOn == true)
            {
                GetComponentInChildren<Text>().color = new Color(1, 0.45f, 0, 1);
            }
            else
            {
                GetComponentInChildren<Text>().color = new Color(1, 1, 1, 1);
            }
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        switch (buttonType)
        {
            case "active":
                if (interactible == true)
                {
                    if (buttonNum + 1 < GetComponentInParent<AllButtonsActivators>().allButtons.Length)
                    {
                        GetComponentInParent<AllButtonsActivators>().allButtons[buttonNum + 1].interactable = true;
                        GetComponentInParent<AllButtonsActivators>().allButtons[buttonNum + 1].GetComponent<ButtonScr>().interactible = true;
                    }
                    //allModelsScr.gameObject.GetComponent<Animator>().enabled = true;
                    FindObjectOfType<CameraControl>().ChangeCamPos(buttonNum);
                    newSubButtonsContainer.SetActive(true);
                    gameObject.SetActive(false);
                }
                break;
            case "look":
                allModelsScr.objectToMoveNum = buttonNum;
                allModelsScr.clickedMoveToCam = true;

                menuButtonsScr.infoButton.thisButtonScr = transform.parent.GetChild(1).GetComponent<ButtonScr>();
                menuButtonsScr.backButton.buttonNum = buttonNum;
                menuButtonsScr.infoButton.buttonNum = buttonNum;
               
                Debug.LogError(menuButtonsScr.infoButton.infoIsOn);
                Debug.LogError(infoIsOn);

                GetComponentInParent<MenuButtons>().closeObjectControlButtons.SetActive(true);
                GetComponentInParent<MenuButtons>().buttonContainer.SetActive(false);
                break;
            case "stop look":
                allModelsScr.GoBackHome();

                Debug.LogError(thisButtonScr);
                menuButtonsScr.infoButton.thisButtonScr = null;

                GetComponentInParent<MenuButtons>().buttonContainer.SetActive(true);
                GetComponentInParent<MenuButtons>().closeObjectControlButtons.SetActive(false);
                break;
            case "info on":

                infoIsOn = !infoIsOn;
                
                if (thisButtonScr != null)
                    thisButtonScr.infoIsOn = infoIsOn;
                menuButtonsScr.infoButton.infoIsOn = infoIsOn;

                FindObjectOfType<ObjectContainer>().InfoObjects[buttonNum].SetActive(infoIsOn);
                if (infoIsOn == true)
                {
                    GetComponentInChildren<Text>().color = new Color(1, 0.45f, 0, 1);
                }
                else
                {
                    GetComponentInChildren<Text>().color = new Color(1, 1, 1, 1);
                }
                break;
            case "transparent":
                transparent = !transparent;
                List<MeshRenderer> meshesNotToChange = new List<MeshRenderer>(); // creates local list of objects which won't be changed
                for (int c = 0; c <= allModelsScr.models[buttonNum].GetComponent<RenderCollecter>().myRenders.Length - 1; c++)
                {
                    meshesNotToChange.Add(allModelsScr.models[buttonNum].GetComponent<RenderCollecter>().myRenders[c]); 
                }
                if (transparent == true) // this will make em transparent
                {
                    GetComponentInChildren<Text>().color = new Color(1, 0.45f, 0, 1);
                    for (int i = 0; i < allModelsScr.modelsOff.Count; i++)
                    {
                        if (meshesNotToChange.Contains(allModelsScr.modelsOff[i]) == false)
                            allModelsScr.modelsOff[i].material.color = new Color(1, 1, 1, 0.5f);
                    }
                    for(int i = 0; i < meshesNotToChange.Count; i++)
                    {
                        if (meshesNotToChange[i] != null)
                            meshesNotToChange[i].gameObject.GetComponent<Outline>().enabled = true;
                    }
                }
                else
                {
                    GetComponentInChildren<Text>().color = new Color(1, 1, 1, 1);
                    for (int i = 0; i < allModelsScr.modelsOff.Count; i++)
                    {
                        if (meshesNotToChange.Contains(allModelsScr.modelsOff[i]) == false)
                            allModelsScr.modelsOff[i].material.color = new Color(1, 1, 1, 1);
                    }

                    for (int i = 0; i < meshesNotToChange.Count; i++)
                    {
                        if (meshesNotToChange[i] != null)
                            meshesNotToChange[i].gameObject.GetComponent<Outline>().enabled = false;
                    }
                }
                break;
            case "repeat":
                FindObjectOfType<CameraControl>().controlingItem = buttonNum;
                allModelsScr.GetComponent<Animator>().Play(allModelsScr.animClip[buttonNum].name);
                //FindObjectOfType<CameraControl>().TurnOnOffOutline();
                break;
        }

    }
}
