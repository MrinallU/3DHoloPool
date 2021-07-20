using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities;

public class CueController : MonoBehaviour
{
    public Vector3 initPos;
    public float threshold = 0.5f;

    private const float PinchThreshold = 0.1f;
    private bool shooting;
    private bool finishShot;
    private bool shotStopped;
    private float charge;
    private GameObject buttonController;

    // Start is called before the first frame update
    void Start()
    {
        buttonController = GameObject.FindGameObjectWithTag("buttoncontroller");
        shooting = false;
        finishShot = false;
        shotStopped = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (shooting)
        {
            charge = System.Math.Abs(initPos.z - gameObject.transform.position.z);
            if (charge > threshold)
            {
                if (!IsPinching(Handedness.Any))
                {
                    gameObject.GetComponent<Rigidbody>().AddForce(0, 0, charge * 100);
                    Debug.Log(charge);
                    shooting = false;
                    finishShot = true;
                    buttonController.GetComponent<ButtonController>().BreakAndShootPressed();
                }
            }

        }
        if (finishShot)
        {
            if (gameObject.transform.position.z >= initPos.z + .1)
            {
                Debug.Log("reached");
                if (!shotStopped)
                {
                    gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    shotStopped = true;
                    finishShot = false;
                }
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
        initPos = gameObject.transform.position;
        shooting = shoot;
    }
}

