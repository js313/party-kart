using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    [SerializeField]
    AudioSource impactSound;
    [SerializeField]
    int groundLayer;
    [SerializeField]
    float minImpactPitch, maxImpactPitch;

    private void OnCollisionEnter(Collision collision)
    {
        //'return'ing from this function was causing issues while registering collisions with the back of the ramp
        if (collision.gameObject.layer != groundLayer)
        {
            impactSound.Stop();
            impactSound.pitch = Random.Range(minImpactPitch, maxImpactPitch);
            impactSound.Play();
        }
    }
}
