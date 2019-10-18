using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataExtractor {

	ExperimentManager manager;

    public GameObject leftHand, rightHand;

	public DataExtractor() {
		GameObject exp = GameObject.Find("Experiment");
		if (exp == null) {
			Debug.Log("/!\\ Error:: can't find experiment gameobject");
		}
		manager = exp.GetComponent<ExperimentManager>();
		if (manager == null) {
			Debug.Log("/!\\ Error:: can't find ExperimentManager gameobject");
		}
	}

	public string[] GetTracebackData() {
        Debug.Log(leftHand.transform.position + "    " + rightHand.transform.position);
		var tmp = manager.GetCurrentTrial();
		return new string[] {tmp[0], tmp[1], tmp[2], tmp[3], Time.time.ToString("N3"),
                             leftHand.transform.position.x.ToString(), leftHand.transform.position.y.ToString(), leftHand.transform.position.z.ToString(),
                             rightHand.transform.position.x.ToString(), rightHand.transform.position.y.ToString(), rightHand.transform.position.z.ToString()};
	}

	public string[] GetExperimentData() {
		var tmp = manager.GetCurrentTrial();
		return new string[] {tmp[0], tmp[1], tmp[2], tmp[3], Time.time.ToString("N3")};
	}
}
