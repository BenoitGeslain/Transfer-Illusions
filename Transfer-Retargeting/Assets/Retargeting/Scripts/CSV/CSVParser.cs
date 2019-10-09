using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

public class CSVParser : MonoBehaviour {

	public TextAsset csvFile;
	public List<Trial> trials = new List<Trial>();

	char lineSeperater = '\n';
	char fieldSeperator = ',';

	void read() {
		string[] trialsTmp = csvFile.text.Split(lineSeperater);
		foreach (string trial in trialsTmp) {
			string[] field = trial.Split(fieldSeperator);
			trials.Add(new Trial(field));
		}
	}

	public List<Trial> GetTrials() {
		read();
		return trials;
	}
}
