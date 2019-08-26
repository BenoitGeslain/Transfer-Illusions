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

	// Participant, block and trial ID to continue experiment
	public int part, blck, trial;

	TrialManager trialManager;		// Handles all the trial computation and state machine
	ScreenManager screenManager;	// Handles what is displayed on the screen
    MultipleUduinoManager uduinoScript;

	CSVSaver csvSaver;

	List<Trial> trials;
	Trial currentTrial;
	int currentTrialIndex;

	int step = -1;

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
        uduinoScript = GameObject.Find("Uduino").GetComponent<MultipleUduinoManager>();

		csvSaver = GetComponent<CSVSaver>();

        print("INIT::ExperimentManager::DONE");
	}

	// Update is called once per frame
	void Update () {
		switch(step) {
			case -1:
				if (Input.GetKeyDown(KeyCode.KeypadEnter) && step==-1) {
					step++;
					uduinoScript.BroadcastCommand("Calibrate");
					screenManager.start = true;
	        	}
				break;
			case 0:
				if (screenManager.index==7) {
					step++;
					print("EXEC::ExperimentManager::Intro Over");
				}
				break;
			case 1:
				// Start experiment
				trialManager.start = true;
				trialManager.condition = (int)Condition.VBW;
				break;
			default:
				break;
		}
	}

	public void LogContinous(string time, int index, Vector3 positionR, Vector3 orientationR, Vector3 positionV, Vector3 orientationV, List<float> acceleration, float score) {
		csvSaver.writeContinousEntry(currentTrial, time, index, positionR, orientationR, positionV, orientationV, acceleration, score);
	}

	public void LogDiscrete(string time, int index, Vector3 positionError, float orientationError, int obstaclesHit, List<int> hitCount, float score) {
		csvSaver.writeDiscreteEntry(currentTrial, time, index, positionError, orientationError, obstaclesHit, hitCount, score);
	}

	void nextTrial() {
		currentTrialIndex++;
		currentTrial=trials[currentTrialIndex];
	}

	void applyTrial() {
		// Condition
		switch(currentTrial.parameters[4]) {
			case "VBW":
				trialManager.condition = (int)Condition.VBW;
				break;
			case "V":
				trialManager.condition = (int)Condition.V;
				break;
			case "RW1":
				trialManager.condition = (int)Condition.RW1;
				break;
			case "RW4":
				trialManager.condition = (int)Condition.RW4;
				break;
			default:
				print("Unmanaged group parameter: " + currentTrial.parameters[4]);
				break;
		}
	}

	public void EndTrial() {
		print("RESET::ExperimentManager::Scene resetting");
		nextTrial();
		applyTrial();
		trialManager.ResetScene();
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
