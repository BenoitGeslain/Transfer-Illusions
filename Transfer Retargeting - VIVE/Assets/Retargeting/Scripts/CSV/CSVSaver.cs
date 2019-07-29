using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVSaver : MonoBehaviour {

	public string tracebackPath, experimentPath;
	int tracebackLength, experimentLength;

	StreamWriter writer;

	DataExtractor extractor;

    public GameObject leftHand, rightHand;

    void Start () {
		tracebackLength = Parameters.tracebackHeader.Length;
		experimentLength = Parameters.experimentHeader.Length;

		writer = new StreamWriter(tracebackPath, true);
		write(Parameters.tracebackHeader);
		writer.Flush();
		writer.Close();

		writer = new StreamWriter(experimentPath, true);
		write(Parameters.experimentHeader);
		writer.Flush();
		writer.Close();

		extractor = new DataExtractor();
        extractor.leftHand = leftHand;
        extractor.rightHand = rightHand;
	}

	void Update () {

	}

	public void writeContinousEntry(Trial trial, int index, Vector3 position, Vector3 orientation) {
		writer = new StreamWriter(tracebackPath, true);

		write(new string[] {trial.parameters[0]; });

		writer.Flush();
		writer.Close();
	}

	public void writeDiscreteEntry(Trial trial, int index, Vector3 positionError, Vector3 orientationError, int obstaclesHit) {
		string[] data = extractor.GetExperimentData();
		if (data.Length!=experimentLength) {
			print("/!\\ Error: Wrong number of parameters to write");
			return;
		}

		writer = new StreamWriter(experimentPath, true);

		write(data);

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
