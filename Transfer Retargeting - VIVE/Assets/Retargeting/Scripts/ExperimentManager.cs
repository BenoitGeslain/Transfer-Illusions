using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Tuple {
	public Trial first;
	public int second;

	public Tuple(Trial t, int s) {
		first = t;
		second = s;
	}
}

public class ExperimentManager : MonoBehaviour {

	public GameObject[] paths;
	public int part, blck, trial;
	//public GameObject proxy;

	List<Trial> trials;
	Trial currentTrial;
	int currentTrialIndex;

	void Start () {
		currentTrialIndex = part +
							blck*Parameters.blockFactor +
							trial*Parameters.trialFactor;
		trials = GetComponent<CSVParser>().GetTrials();
		// foreach (Trial trial in trials) {
		// 	print(trial.id);
		// }
		Tuple tmp = find(currentTrialIndex);
		if (tmp.first == null) {
			print("Trial not found: P = " + part + ", block = " + blck +
				  ", trial = " + trial + ", ID = " + currentTrialIndex);
			tmp = find(0);
			currentTrialIndex = tmp.second;
		}
		currentTrial = tmp.first;
		printCurrentTrial();
		applyTrial();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("space")) {	// If user reached end of trial
            nextTrial();
			resetTrial();
			applyTrial();
			printCurrentTrial();
        }
	}

	void applyTrial() {
		GameObject path;
		int pathIndex;
		if (Int32.TryParse(currentTrial.parameters[5], out pathIndex)) {
			path = Instantiate(paths[pathIndex-1], new Vector3(4.5f, -4.5f, 5f), Quaternion.identity);
			path.transform.parent = this.transform;
		} else {
			print("Unmanaged trial path parameter: " + currentTrial.parameters[5] + " --> Can't parse or NaN");
		}

		switch(currentTrial.parameters[4]) {
		case "I":

			break;
		case "T":

			break;
		case "B":

			break;
		case "null":	// No group change
			break;
		default:
			print("Unmanaged group parameter: " + currentTrial.parameters[4]);
			break;
		}
	}

	void resetTrial() {
		 foreach (Transform child in this.transform) {
			 Destroy(child.gameObject);
		 }
	}

	void nextTrial() {
		currentTrialIndex++;
		currentTrial=trials[currentTrialIndex];
	}

	public Tuple find(int id) {
		for (int i=0; i<trials.Count; i++) {
			if (trials[i].id == id) {
				return new Tuple(trials[i], i);
			}
		}
		return new Tuple(null, -1);;
	}

	public Trial find(int p, int b, int t) {
		int id = p + b*Parameters.blockFactor + t*Parameters.trialFactor;
		foreach (Trial trial in trials) {
			print(trial.id + " " + id);
			if (trial.id == id) {
				return trial;
			}
		}
		return null;
	}

	public string[] GetCurrentTrial() {
		return new string[] {currentTrial.parameters[0], currentTrial.parameters[1], currentTrial.parameters[2], currentTrial.parameters[3]};
	}

	void printCurrentTrial() {

		print("Current trial - Participant: " + currentTrial.parameters[0] +
						   " - Block: " + currentTrial.parameters[2] +
						   " - Trial: " + currentTrial.parameters[3] +
						   " - Index: " + currentTrialIndex +
						   " - ID: " + currentTrial.id);
		print("Conditions - Group: " + currentTrial.parameters[4] +
						" - Path: " + currentTrial.parameters[5]);
	}
}
