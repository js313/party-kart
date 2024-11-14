using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField]
    float maxSpeed;
    [SerializeField]
    Rigidbody rb;
    [SerializeField]
    float forwardAcceleration, reverseAcceleration;
    [SerializeField]
    float turnStrength;
    [SerializeField]
    float dragOnGround, dragInAir;
    [SerializeField]
    float gravityMod;

    float forwardForce;
    float turnForce;

    bool grounded;
    [SerializeField]
    Transform groundRayPoint;
    [SerializeField]
    LayerMask groundLayer;
    [SerializeField]
    float groundRayLength;

    void Start()
    {
        rb.transform.parent = null;
    }

    void FixedUpdate()
    {
        grounded = false;
        RaycastHit hit;

        if (Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLength, groundLayer))
        {
            grounded = true;
        }

        if (grounded)
        {
            // accelerator
            rb.AddForce(transform.forward * forwardForce);
            rb.drag = dragOnGround;
        }
        else
        {
            rb.drag = dragInAir;
            rb.AddForce(Vector3.down * gravityMod);
        }
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        if (verticalInput > 0)
        {
            forwardForce = verticalInput * forwardAcceleration;
        }
        else
        {
            forwardForce = verticalInput * reverseAcceleration;
        }

        turnForce = Input.GetAxis("Horizontal");

        if (grounded && verticalInput != 0)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles +
                (Time.deltaTime * turnStrength * turnForce * Math.Sign(verticalInput) * (rb.velocity.magnitude / maxSpeed) * Vector3.up));
        }

        transform.position = rb.position;
    }
}
