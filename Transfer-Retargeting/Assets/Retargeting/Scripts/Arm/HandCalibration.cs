using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCalibration : MonoBehaviour
{
    public Transform armHandTracked;

    // TrialManager trialScript;
    SphereSize trialScript;

    void Start() {
        // trialScript = GameObject.Find("World").GetComponent<TrialManager>();
        trialScript = GameObject.Find("World").GetComponent<SphereSize>();
    }
    
    void Update() {
    	if (!trialScript.IsWarping()) {
            print("Locking position");
			for (int i = 0; i<this.transform.childCount; i++) {
	            this.transform.GetChild(i).position = armHandTracked.GetChild(i).position;
	            this.transform.GetChild(i).eulerAngles = armHandTracked.GetChild(i).eulerAngles;
	        }
    	}
    }
}
