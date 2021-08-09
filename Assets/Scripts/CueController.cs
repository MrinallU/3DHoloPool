using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine.XR.WSA.Input;
using Microsoft.MixedReality.Toolkit;
using UnityEngine.XR;
using System;

public class CueController : MonoBehaviour
{
    #region Public Variables

    #endregion

    #region Private Variables

    private bool shooting;
    private bool finishShot;
    private bool cueBallContact;
    private float charge;
    private float cueLocalY;
    private float shotThreshold = 0.000005f;
    private float distanceOffset = 0.01f;
    private float forceMultiplier = 50000000f;
    private float timeCounter;
    private const float shotResetStep = .5f;
    private const float shotTime = 0.2f;
    private const float PinchThreshold = 0.1f;
    private Vector3 initPos;
    private Quaternion shootAngle;
    private Quaternion startRotation;
    private Rigidbody cueRigidbody;
    private Collider cueBallCollider;
    private Collider cueCollider;
    private Collider poolTableCollider;
    private GameObject buttonController;
    private GameObject[] borders;
    private GameObject[] stripes;
    private GameObject[] solids;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        buttonController = GameObject.FindGameObjectWithTag("buttoncontroller");

        borders = GameObject.FindGameObjectsWithTag("border");
        stripes = GameObject.FindGameObjectsWithTag("stripe");
        solids = GameObject.FindGameObjectsWithTag("solid");

        cueRigidbody = gameObject.GetComponent<Rigidbody>();

        cueBallCollider = GameObject.FindGameObjectWithTag("cueBall").GetComponent<Collider>();
        cueCollider = gameObject.GetComponent<Collider>();
        poolTableCollider = GameObject.FindGameObjectWithTag("table").GetComponent<Collider>();

        startRotation = gameObject.transform.localRotation;

        shooting = false;
        finishShot = false;
        cueBallContact = false;

        SetIgnoreCollision(cueBallCollider, true);
        SetIgnoreCollision(GameObject.FindGameObjectWithTag("eightBall").GetComponent<Collider>(), true);
        SetIgnoreCollision(poolTableCollider, true);

        SetIgnoreCollisions(borders, true);
        SetIgnoreCollisions(stripes, true);
        SetIgnoreCollisions(solids, true);

    }

    // Update is called once per frame
    void Update()
    {

        if (shooting) 
        {
            Shoot();
        }
        else
        {
            NotShooting();
        }

    }

    public void Shoot() // Propel the cue forwards after it is released
    {
        if (finishShot)
        {
            FinishShot();
        } 
        else
        {
            SetIgnoreCollision(cueBallCollider, false);
            charge = System.Math.Abs(initPos.y - gameObject.transform.localPosition.y);
            if (charge > shotThreshold)
            {
                if (!IsPinching(Handedness.Any))
                {
                    cueBallContact = false;
                    shootAngle = gameObject.transform.localRotation; 
                    cueRigidbody.AddRelativeForce(0, 1500, 0);
                    cueRigidbody.freezeRotation = true;
                    timeCounter = 0;
                    finishShot = true;
                }
            }
        }
    }

    public void FinishShot() // Stop the cue after a certain amount of time has passed
    { 
        timeCounter += Time.deltaTime;
        if (timeCounter >= shotTime || cueBallContact == true)
        {
            Debug.Log("Shot Finished");
            cueRigidbody.velocity = Vector3.zero;
            cueRigidbody.angularVelocity = Vector3.zero;
            if (transform.localPosition != initPos) // Gradually move the cue back to its initial shooting position
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, initPos, shotResetStep);
            }
            else
            {
                finishShot = false;
                cueBallContact = false;
                buttonController.GetComponent<ButtonController>().BreakAndShootPressed(); // Deactivate shoot mode
            }
        }
    }

    public void NotShooting()
    {
        SetIgnoreCollision(cueBallCollider, true); // Only ignores the cue ball's trigger collider
        if (IsPinching(Handedness.Any))
        {
            cueRigidbody.freezeRotation = false;
        }
        else
        {
            cueRigidbody.velocity = Vector3.zero;
            cueRigidbody.angularVelocity = Vector3.zero;
            cueRigidbody.freezeRotation = true;
        }
    }

    public static bool IsPinching(Handedness trackedHand)
    {
        return HandPoseUtils.CalculateIndexPinch(trackedHand) > PinchThreshold;
    }

    public void InitiateShooting(bool shoot)
    {
        if (!shoot)
        {
            shooting = shoot;
            return;
        }
        initPos = gameObject.transform.localPosition;
        shooting = shoot;
    }

    public float GetCharge()
    {
        return charge * forceMultiplier;
    }

    public void SetCueBallContact(bool b)
    {
        cueBallContact = b;
    }

    public Quaternion GetStartRotation()
    {
        return startRotation;
    }

    public void SetIgnoreCollisions(GameObject[] collisionObjects, bool b)
    {
        foreach (GameObject currObject in collisionObjects)
        {
            Physics.IgnoreCollision(currObject.GetComponent<Collider>(), cueCollider, b);
        }
    }

    public void SetIgnoreCollision(Collider someCollider, bool b)
    {
        Physics.IgnoreCollision(someCollider, cueCollider, b);
    }
}

