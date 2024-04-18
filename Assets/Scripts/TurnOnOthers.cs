using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnOthers : MonoBehaviour
{
    [SerializeField] GameObject[] TurnThisObjects;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i <= TurnThisObjects.Length - 1; i++) TurnThisObjects[i].SetActive(true);
    }
}
