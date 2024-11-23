using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField]
    float maxSpeed;
    // Getter
    public float MaxSpeed => maxSpeed;
    [SerializeField]
    Rigidbody rb;
    // Getter
    public Rigidbody Rb => rb;

    [SerializeField]
    float forwardAcceleration, reverseAcceleration;
    [SerializeField]
    float turnStrength;
    [SerializeField]
    float dragOnGround, dragInAir;
    [SerializeField]
    float gravityMod;

    float forwardForce;
    float turnInput, verticalInput;

    bool grounded;
    [SerializeField]
    Transform groundRayPoint1, groundRayPoint2;
    [SerializeField]
    LayerMask groundLayer;
    [SerializeField]
    float groundRayLength;

    [SerializeField]
    Transform leftFrontWheel, rightFrontWheel;
    [SerializeField]
    float maxWheelTurn;

    [SerializeField]
    ParticleSystem[] dustTrails;
    [SerializeField]
    float maxEmission, emissionFadeSpeed;
    float emissionRate;

    [SerializeField]
    AudioSource engineSound;
    [SerializeField]
    float minEnginePitch, maxEnginePitch;
    [SerializeField]
    AudioSource skidSound;
    [SerializeField]
    float skidSoundFadeSpeed;
    [SerializeField]
    float skidThreshold;

    int nextCheckPoint, currentLap;
    float currentLapTime = 0f, bestLapTime = float.PositiveInfinity;

    [SerializeField]
    bool isComp;
    [SerializeField]
    float compPointRange = 5f, compPointVariance = 1f;
    [SerializeField]
    float compMaxTurnAngle = 15f, compAcceleration = 5f, compSpeedInputWhileTurning = 0.8f;
    float compSpeedInput = 0, compRandomSpeedMod = 1f;

    [SerializeField]
    int targetPointIndex = 0;
    Vector3 targetPoint;

    void Start()
    {
        currentLap = 0;
        nextCheckPoint = 0;
        rb.transform.parent = null;
        if (isComp)
        {
            compSpeedInput = 1f;
            targetPoint = RandomiseTarget(RaceManager.instance.GetCheckPointPosition(targetPointIndex), compPointVariance);
            compRandomSpeedMod = UnityEngine.Random.Range(0.9f, 1.3f);
        }
    }

    // Quaternions are fuckin hard

    // Think of ways to stop or massively reduce the motion perpendicular to the tyres
    void FixedUpdate()
    {
        //// Prevent lateral movement
        //// Get the velocity of the Rigidbody
        //Vector3 velocity = rb.velocity;

        //// Calculate the component of the velocity along transform.right
        //Vector3 rightDirection = transform.right;
        //Vector3 velocityAlongRight = Vector3.Dot(velocity, rightDirection) * rightDirection;

        //// Subtract the velocity component along transform.right from the Rigidbody's velocity
        //rb.velocity = velocity - velocityAlongRight;

        grounded = false;
        Vector3 targetNormal = transform.rotation.eulerAngles;

        if (Physics.Raycast(groundRayPoint1.position, -transform.up, out RaycastHit hit, groundRayLength, groundLayer))
        {
            grounded = true;
            targetNormal = hit.normal;
        }
        if (Physics.Raycast(groundRayPoint2.position, -transform.up, out hit, groundRayLength, groundLayer))
        {
            targetNormal = (targetNormal + hit.normal) / 2f;
        }

        if (grounded)
        {
            // Accelerator
            rb.AddForce(transform.forward * forwardForce);
            rb.drag = dragOnGround;
            // Rotate car to match surface rotation
            transform.rotation = Quaternion.FromToRotation(transform.up, targetNormal) * transform.rotation;
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

        transform.position = rb.position;
    }

    void Update()
    {
        currentLapTime += Time.deltaTime;

        if (!isComp)
        {
            UIManager.instance.SetLapTime(currentLapTime, isCurrentLap: true);

            verticalInput = Input.GetAxis("Vertical");
            if (verticalInput > 0)
            {
                forwardForce = verticalInput * forwardAcceleration;
            }
            else
            {
                forwardForce = verticalInput * reverseAcceleration;
            }

            turnInput = Input.GetAxis("Horizontal");
        }
        else
        {
            if (Vector3.SqrMagnitude(transform.position - targetPoint) <= compPointRange * compPointRange)
            {
                TargetReached();
            }

            Vector3 targetDir = targetPoint - transform.position;
            float targetDirAngle = Vector3.Angle(transform.forward, targetDir); // Can use SignedAngle function too.

            // Use SignedAngle instead of doing this
            if (Vector3.Cross(transform.forward, targetDir).y < 0)
            {
                targetDirAngle = -targetDirAngle;
            }

            turnInput = Mathf.Clamp(targetDirAngle / compMaxTurnAngle, -1f, 1f); // Similar to Input.getAxis("Horizontal")

            if (targetDirAngle <= compAcceleration)
            {
                compSpeedInput = Mathf.MoveTowards(compSpeedInput, 1f, compAcceleration);
            }
            else
            {
                compSpeedInput = Mathf.MoveTowards(compSpeedInput, compSpeedInputWhileTurning, compAcceleration);
            }

            verticalInput = compSpeedInput;
            forwardForce = compSpeedInput * forwardAcceleration * compRandomSpeedMod;
        }

        SharedFunctions();
    }

    public void CheckPointHit(int hitCheckPointIndex)
    {
        if (nextCheckPoint == hitCheckPointIndex)
        {
            if (nextCheckPoint == 0)
            {
                LapCompleted();
            }
            nextCheckPoint++;

            if (nextCheckPoint == RaceManager.instance.checkPointsCount)
            {
                nextCheckPoint = 0;
            }

            if (isComp)
            {
                //TargetReached();    // Cars running into obstacles, so turned it off for now
            }
        }
    }

    public void TargetReached()
    {
        targetPointIndex = (targetPointIndex + 1) % RaceManager.instance.checkPointsCount;
        targetPoint = RandomiseTarget(RaceManager.instance.GetCheckPointPosition(targetPointIndex), compPointVariance);
    }

    Vector3 RandomiseTarget(Vector3 point, float targetVariance)
    {
        point += new Vector3(UnityEngine.Random.Range(-targetVariance, targetVariance), 0, UnityEngine.Random.Range(-targetVariance, targetVariance));
        return point;
    }

    void LapCompleted()
    {
        currentLap++;
        if (currentLapTime < bestLapTime && currentLap > 1)
        {
            bestLapTime = currentLapTime;
        }
        if (!isComp) UIManager.instance.SetLapCounter(currentLap);

        if (!isComp && bestLapTime != float.PositiveInfinity)
        {
            UIManager.instance.SetLapTime(bestLapTime, isCurrentLap: false);
        }
        currentLapTime = 0f;
    }

    void SharedFunctions()
    {
        // Turn
        if (grounded && verticalInput != 0)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles +
                (Time.deltaTime * turnStrength * turnInput * Math.Sign(verticalInput) * (rb.velocity.magnitude / maxSpeed) * Vector3.up));
        }

        leftFrontWheel.localRotation = Quaternion.Euler(leftFrontWheel.localRotation.eulerAngles.x, turnInput * maxWheelTurn, transform.localRotation.eulerAngles.z);
        rightFrontWheel.localRotation = Quaternion.Euler(rightFrontWheel.localRotation.eulerAngles.x, turnInput * maxWheelTurn, rightFrontWheel.localRotation.eulerAngles.z);

        // Particle emission
        emissionRate = Mathf.MoveTowards(emissionRate, 0f, emissionFadeSpeed * Time.deltaTime);

        if (grounded && (Math.Abs(turnInput) > skidThreshold || rb.velocity.magnitude < maxSpeed / 2))
        {
            emissionRate = maxEmission;
        }

        for (int i = 0; i < dustTrails.Length; i++)
        {
            ParticleSystem.EmissionModule emission = dustTrails[i].emission;
            if (rb.velocity.magnitude <= 0.5f) emissionRate = 0;
            emission.rateOverTime = emissionRate;
        }

        // Audio
        engineSound.pitch = Mathf.Lerp(minEnginePitch, maxEnginePitch, rb.velocity.magnitude / maxSpeed);
        if (grounded && Math.Abs(turnInput) > skidThreshold && rb.velocity.magnitude > maxSpeed / 2)
        {
            skidSound.volume = Mathf.MoveTowards(skidSound.volume, 1f, skidSoundFadeSpeed * Time.deltaTime);
            if (!skidSound.isPlaying) skidSound.Play();
        }
        else
        {
            skidSound.volume = Mathf.MoveTowards(skidSound.volume, 0f, skidSoundFadeSpeed * Time.deltaTime);
            if (skidSound.volume < 0.1f) skidSound.Stop();
        }
    }
}
