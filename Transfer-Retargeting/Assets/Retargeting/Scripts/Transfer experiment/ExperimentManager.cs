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
    PathManager pathScript;
    SceneConfiguration sceneConfiguration;

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

		Tuple tmp = find(currentTrialIndex);
		if (tmp.first == null) {
			print("Trial not found: P = " + part + ", block = " + blck +
				  ", trial = " + trial + ", ID = " + currentTrialIndex);
			tmp = find(0);
			currentTrialIndex = tmp.second;
		}
		currentTrial = tmp.first;

		/*foreach(Trial t in trials) {
			t.print();
		}*/

		trialManager = GetComponent<TrialManager>();
		screenManager = GetComponent<ScreenManager>();
        uduinoScript = GameObject.Find("Uduino").GetComponent<MultipleUduinoManager>();
        pathScript = GetComponent<PathManager>();
        sceneConfiguration = GetComponent<SceneConfiguration>();

		csvSaver = GetComponent<CSVSaver>();

        print("INIT::ExperimentManager::DONE");
	}

	// Update is called once per frame
	void LateUpdate () {
		switch(step) {
			case -1:
				if (Input.GetKeyDown(KeyCode.KeypadEnter)) {
					step++;
					uduinoScript.BroadcastCommand("Calibrate");
					screenManager.start = true;
					pathScript.ShowPath(0);
	        	}
				break;
			case 0:
				if (screenManager.step==1) {
					step++;
					nextTrial();
					applyTrial();
					print("EXEC::ExperimentManager::Intro Over");
				}
				break;
			case 1:
				// Start experiment
				trialManager.start = true;
				trialManager.SetCondition(Int32.Parse(currentTrial.parameters[3]));
				currentTrial.print();
				break;
			default:
				break;
		}

		if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
	}

	public void LogContinous(string time, int index, Vector3 positionR, Vector3 orientationR, Vector3 positionV, Vector3 orientationV, Vector3 positionP, Vector3 orientationP, List<float> acceleration, int[] collisions, float score) {
		csvSaver.writeContinousEntry(currentTrial, time, index, positionR, orientationR, positionV, orientationV, positionP, orientationP, acceleration, collisions, score);
	}

	public void LogDiscrete(string time, string startTrialTime, int index, Vector3 positionR, Vector3 orientationR, Vector3 positionP, Vector3 orientationP, List<int> obstaclesHit, int[] collisions, float score) {
		csvSaver.writeDiscreteEntry(currentTrial, time, startTrialTime, index, positionR, positionR, positionP, orientationP, obstaclesHit, collisions, score);
	}

	void nextTrial() {
		currentTrialIndex++;
		currentTrial=trials[currentTrialIndex];
		print(currentTrial.parameters[4]);
	}

	void applyTrial() {
		currentTrial.print();

		switch(currentTrial.parameters[4]) {
			case "VBW":
			print("!");
				trialManager.SetCondition((int)Condition.VBW);
				sceneConfiguration.resetPhantoms();
				break;
			case "V":
				trialManager.SetCondition((int)Condition.V);
				sceneConfiguration.setPhantomsOnGrabbables();
				break;
			case "RW1":
				trialManager.SetCondition((int)Condition.RW1);
				sceneConfiguration.setPhantomsOnGrabbables();
				break;
			case "RW4":
				trialManager.SetCondition((int)Condition.RW4);
				sceneConfiguration.setPhantomsOnGrabbables();
				break;
			default:
				print("Unmanaged group parameter: " + currentTrial.parameters[4]);
				break;
		}
	}

	public void EndTrial() {
		print("RESET::ExperimentManager::Scene resetting");

		string id = currentTrial.parameters[0];
		print(id);
		nextTrial();
		applyTrial();
		print(currentTrial.parameters[0]);
		trialManager.ResetScene(string.Equals(id, currentTrial.parameters[0]));
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
