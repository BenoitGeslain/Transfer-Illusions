using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmWarping : MonoBehaviour
{
    public Transform armHandTracked;

    TrialManager trialScript;

    void Sart() {
    	trialScript = GameObject.Find("World").GetComponent<TrialManager>();
    }
    
    void Update() {
    	if (!trialScript.IsWarping()) {
			for (int i = 0; i<this.transform.childCount; i++) {
	            this.transform.GetChild(i).position = armHandTracked.GetChild(i).position;
	            this.transform.GetChild(i).eulerAngles = armHandTracked.GetChild(i).eulerAngles;
	        }
    	}
    }
}
