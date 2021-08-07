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
    private float shotThreshold = 0.000005f;
    private float distanceOffset = 0.01f;
    private float forceMultiplier = 50000000f;
    private const float PinchThreshold = 0.1f;
    private bool shooting;
    private bool finishShot;
    private bool shotStopped;
    private float charge;
    private Vector3 initPos;
    private GameObject buttonController;
    private GameObject cueBall;
    private Collider cueBallCollider;
    private Collider cueCollider;
    private Rigidbody cueRigidbody;
    private Quaternion shootAngle;
    private Quaternion startRotation;
    private float shootLocalZPos;
    private float cueLocalY;
    private GameObject[] borders;
    private GameObject[] stripes;
    private GameObject[] solids;
    private GameObject poolTable; 
    //private var hand = new IMixedRealityHand;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        startRotation = gameObject.transform.localRotation;

        buttonController = GameObject.FindGameObjectWithTag("buttoncontroller");
        cueRigidbody = gameObject.GetComponent<Rigidbody>();
        shooting = false;
        finishShot = false;
        shotStopped = false;

        cueBall = GameObject.FindGameObjectWithTag("cueBall");
        cueBallCollider = cueBall.GetComponent<Collider>();
        cueCollider = gameObject.GetComponent<Collider>();
        borders = GameObject.FindGameObjectsWithTag("border");
        poolTable = GameObject.FindGameObjectWithTag("table");
        stripes = GameObject.FindGameObjectsWithTag("stripe");
        solids = GameObject.FindGameObjectsWithTag("solid");

        Physics.IgnoreCollision(cueBallCollider, cueCollider);

        Physics.IgnoreCollision(GameObject.FindGameObjectWithTag("eightBall").GetComponent<Collider>(), cueCollider);
        foreach (GameObject currBorder in borders)
        {
            Physics.IgnoreCollision(currBorder.GetComponent<Collider>(), cueCollider);
        }

        foreach (GameObject currBall in stripes)
        {
            Physics.IgnoreCollision(currBall.GetComponent<Collider>(), cueCollider);
        }

        foreach (GameObject currBall in solids)
        {
            Physics.IgnoreCollision(currBall.GetComponent<Collider>(), cueCollider);
        }

        Physics.IgnoreCollision(poolTable.GetComponent<Collider>(), cueCollider);
    }

    // Update is called once per frame
    void Update()
    {

        //gameObject.GetComponent<Transform>().position = new Vector3(gameObject.GetComponent<Transform>().position.x ,-4.79f, gameObject.GetComponent<Transform>().position.z);
        //gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, cueLocalY, gameObject.transform.localPosition.z);
        if (shooting) // Propel the cue forwards after it is released
        {
            Physics.IgnoreCollision(cueBallCollider, cueCollider, false);
            charge = System.Math.Abs(initPos.y - gameObject.transform.localPosition.y); 
           // Debug.Log("charge: " + charge + ", initPos.y: " + initPos.y);
            if (charge > shotThreshold)
            {
                Debug.Log("charge > shotThreshold");
                if (!IsPinching(Handedness.Any))
                {
                    shootAngle = gameObject.transform.localRotation;
                    shootLocalZPos = gameObject.transform.localPosition.z;
                    //cueRigidbody.AddRelativeForce(0, System.Math.Min(charge * forceMultiplier, 3000), 0);
                  
                    Debug.Log("Charge * forceMultiplier " + charge * forceMultiplier);
                    cueRigidbody.AddRelativeForce(0, 750, 0);
                    cueRigidbody.freezeRotation = true;
                    shooting = false;
                    finishShot = true;
                    buttonController.GetComponent<ButtonController>().BreakAndShootPressed();
                    // calculate timer duration 
                    // start timer
                   
                }
            }

        }
        else if (finishShot) // Stop the cue once it reaches a certain distance past its starting position
        {
            //gameObject.transform.localRotation = shootAngle; // keep resetting

            Debug.Log("localPos: " + Math.Abs(gameObject.transform.localPosition.y) + ", initPos.y + distanceOffset: " + Math.Abs(initPos.y - distanceOffset));
            // if timer expires
            if (gameObject.transform.localPosition.y >= initPos.y + distanceOffset)
            {
                Debug.Log("reached");
                if (!shotStopped)
                {
                    // reset timer

                    cueRigidbody.velocity = Vector3.zero;
                    //gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, initPos.y, gameObject.transform.localPosition.z);
                    shotStopped = true;
                    finishShot = false;
                }
            }
        } else // Stop the cue from drifting after letting go of it (not when shooting)
        {
            Physics.IgnoreCollision(cueBallCollider, cueCollider, true);
            if (IsPinching(Handedness.Any))
            {
                cueRigidbody.freezeRotation = false;
            } else {
                cueRigidbody.velocity = Vector3.zero;
                cueRigidbody.angularVelocity = Vector3.zero;
                cueRigidbody.freezeRotation = true;
            }
        }

        if (shotStopped && !finishShot)
        {
            shotStopped = false;
        }
    }

    public static bool IsPinching(Handedness trackedHand)
    {
        return HandPoseUtils.CalculateIndexPinch(trackedHand) > PinchThreshold;
    }



    public void HandleShooting(bool shoot)
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

    public Quaternion GetStartRotation()
    {
        return startRotation;
    }
}

