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

public enum Condition {VBW=0, V=1, RW1=2, RW4=3}

public class ExperimentManager : MonoBehaviour {

	public GameObject[] paths;
	public int part, blck, trial;
	//public GameObject proxy;

	TrialManager trialManager;
	ScreenManager screenManager;

	List<Trial> trials;
	Trial currentTrial;
	int currentTrialIndex;

	int step = 0;

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

		trialManager = GetComponent<TrialManager>();
		screenManager = GetComponent<ScreenManager>();
	}

	// Update is called once per frame
	void Update () {
		switch(step) {
			case 0:
				// show introduction text
				break;
			case 1:
				// Start experiment
				trialManager.enabled = true;
				break;
		}
		/*if (Input.GetKeyDown("space")) {	// If user reached end of trial
            nextTrial();
			resetTrial();
			applyTrial();
			printCurrentTrial();
        }*/
	}

	void NextTrial() {
		currentTrialIndex++;
		currentTrial=trials[currentTrialIndex];
	}

	void ApplyTrial(TrialManager m) {
		// Condition
		switch(currentTrial.parameters[4]) {
		case "VBW":
			m.condition = (int)Condition.VBW;
			break;
		case "V":
			m.condition = (int)Condition.V;
			break;
		case "RW1":
			m.condition = (int)Condition.RW1;
			break;
		case "RW4":
			m.condition = (int)Condition.RW4;
			break;
		default:
			print("Unmanaged group parameter: " + currentTrial.parameters[4]);
			break;
		}
	}

	void resetTrial() {

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
