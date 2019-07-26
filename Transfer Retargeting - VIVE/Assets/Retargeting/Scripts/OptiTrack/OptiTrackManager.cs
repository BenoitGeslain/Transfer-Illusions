using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptiTrackManager : MonoBehaviour
{
    public Transform tableMarkers;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            print("Calibrating");
            this.transform.position = tableMarkers.position;
            this.transform.rotation = tableMarkers.rotation;
        }
    }
}
