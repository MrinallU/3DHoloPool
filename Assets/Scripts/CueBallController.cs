using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueBallController : MonoBehaviour
{
    private GameObject cue;
    private float charge;

    // Start is called before the first frame update
    void Start()
    {
        cue = GameObject.FindGameObjectWithTag("cue");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        //charge = collider.GetComponent<CueController>().GetCharge();
        //Debug.Log("Charge: " + charge);
        if (collider.gameObject.name == "Cue")
        {
            Debug.Log("Cue Detected");
            //cue.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
        Debug.Log("Collision Detected");
    }
}
