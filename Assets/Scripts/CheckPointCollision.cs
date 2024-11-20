using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointCollision : MonoBehaviour
{
    [SerializeField]
    CarController carController;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("CheckPoint"))
        {
            carController.CheckPointHit(other.GetComponent<CheckPoint>().CheckPointIndex);
        }
    }
}
