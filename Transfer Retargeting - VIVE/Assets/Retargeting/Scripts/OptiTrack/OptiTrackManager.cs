using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptiTrackManager : MonoBehaviour {
    
    TrialManager trialManager;
    public Transform tableMarkers;

    void Start() {
        trialManager = GetComponent<TrialManager>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.KeypadEnter)) {
            print("INIT::OptiTrackManager::Calibrated");
            this.transform.position = tableMarkers.position;
            this.transform.rotation = tableMarkers.rotation;
        }
    }
}
