using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalSettings : object
{    
    public static bool isSelecting = false;
    public static bool isAbout = false;
    public static bool isPause = false;
    public static bool isExit = false;
    public static bool isExitConfirm = false;
    public static bool isNOConfirm = false;
    public static bool isYESConfirm = false;
    public static bool isInstructions = true;
    public static bool allowedPlacing = false;
    public static bool allowedPlacingDialog = false;
    public static bool allowedShooting = false;


    public static bool isGameOver = false;
    public static float Trigger = 0.0f;
    public static float MaxTrigger = 0.0f;


    public static void DebugText(string txt)
    {
            TextMesh textObject = GameObject.Find("descStart").GetComponent<TextMesh>();
            textObject.text = txt;
    }
    public static void SetCursorVisible(bool setting)
    {
        MeshRenderer r;
        foreach (GameObject obj1 in GameObject.FindGameObjectsWithTag("focus"))
        {
            if (null != obj1.GetComponent<MeshRenderer>())
            {
                r = obj1.GetComponent<MeshRenderer>();
                r.enabled = setting;
            }

        }
    } 
}


