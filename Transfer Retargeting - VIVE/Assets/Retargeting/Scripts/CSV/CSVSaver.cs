using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVSaver : MonoBehaviour {

	public string continousPath = "Data/continous.csv", discretePath = "Data/discrete.csv";

	StreamWriter writer;

    void Start () {
		writer = new StreamWriter(continousPath, true);
		write(Parameters.tracebackHeader);
		writer.Flush();
		writer.Close();

		writer = new StreamWriter(discretePath, true);
		write(Parameters.experimentHeader);
		writer.Flush();
		writer.Close();
	}

	void Update () {

	}

	public void writeContinousEntry(Trial trial, string time, int index, Vector3 positionR, Vector3 orientationR, Vector3 positionV, Vector3 orientationV, float score) {
		writer = new StreamWriter(continousPath, true);

		write(new string[] {trial.parameters[0], trial.parameters[1], trial.parameters[2], trial.parameters[3], trial.parameters[4], time,
							index.ToString(), positionR.x.ToString(), positionR.y.ToString(), positionR.z.ToString(), orientationR.x.ToString(),
							orientationR.y.ToString(), orientationR.z.ToString(), positionV.x.ToString(), positionV.y.ToString(), positionV.z.ToString(),
							orientationV.x.ToString(), orientationV.y.ToString(), orientationV.z.ToString(), score.ToString()});
		writer.Flush();
		writer.Close();
	}

	public void writeDiscreteEntry(Trial trial, string timeCube, int index, Vector3 positionError, float orientationError, int obstaclesHit, float score) {
		writer = new StreamWriter(discretePath, true);

		write(new string[] {trial.parameters[0], trial.parameters[1], trial.parameters[2], trial.parameters[3], trial.parameters[4],
							timeCube, index.ToString(), positionError.x.ToString(), positionError.y.ToString(), positionError.z.ToString(),
							orientationError.ToString(), obstaclesHit.ToString(), score.ToString()});
		writer.Flush();
		writer.Close();
	}

	void write(string[] d) {
		for (int i=0; i<d.Length; i++) {
			writer.Write(d[i]);
			if (i!=d.Length-1) {
				writer.Write(",");
			}
		}
		writer.WriteLine("");
	}
}
