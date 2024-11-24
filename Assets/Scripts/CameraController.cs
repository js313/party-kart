using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    CarController car;
    Vector3 offset;

    [SerializeField]
    float minDistance, maxDistance;
    float activeDistance;

    [SerializeField]
    Transform rememberRelativePosFromCar;

    void Start()
    {
        offset = transform.position - rememberRelativePosFromCar.position;
        activeDistance = minDistance;
    }

    void Update()
    {
        activeDistance = Mathf.Lerp(minDistance, maxDistance, car.Rb.velocity.magnitude / car.maxSpeed);
        transform.position = car.transform.position + offset.normalized * activeDistance;
    }
}
