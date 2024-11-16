using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField]
    GameObject[] cameras;
    private int currentCamIndex = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            cameras[currentCamIndex].SetActive(false);
            currentCamIndex = (currentCamIndex + 1) % cameras.Length;
            cameras[currentCamIndex].SetActive(true);
        }
    }
}
