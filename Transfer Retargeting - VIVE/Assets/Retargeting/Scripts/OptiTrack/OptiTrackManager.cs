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
            print("INIT::OptiTrackManager::Calibrated");
            this.transform.position = tableMarkers.position;
            this.transform.rotation = tableMarkers.rotation;
        }
    }
}
