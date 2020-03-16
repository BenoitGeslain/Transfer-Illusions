using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVSaver : MonoBehaviour {

	public string continousPath = "Data/continous.csv", discretePath = "Data/discrete.csv", sceneConfiguration = "Data/config.csv";

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

		writer = new StreamWriter(sceneConfiguration, true);
		write(Parameters.sceneConfiguration);
		writer.Flush();
		writer.Close();
	}

	public void writeContinousEntry(Trial trial, string time, int index,
									Vector3 positionR, Vector3 orientationR,
									Vector3 positionV, Vector3 orientationV,
									Vector3 positionP, Vector3 orientationP,
									float velocity, float cubeTime, int nbError,
									bool warping,
									float cubeDistGone, float handDistGone,
									List<float> acceleration, int[] collisions,
									float score,
									bool pause) {
		writer = new StreamWriter(continousPath, true);

		write(new string[] {trial.parameters[0], trial.parameters[1], trial.parameters[2], trial.parameters[3], trial.parameters[4],
							index.ToString(),
							time,
							positionR.x.ToString(), positionR.y.ToString(), positionR.z.ToString(),
							orientationR.x.ToString(), orientationR.y.ToString(), orientationR.z.ToString(),
							positionV.x.ToString(), positionV.y.ToString(), positionV.z.ToString(),
							orientationV.x.ToString(), orientationV.y.ToString(), orientationV.z.ToString(),
							positionP.x.ToString(), positionP.y.ToString(), positionP.z.ToString(),
							orientationP.x.ToString(), orientationP.y.ToString(), orientationP.z.ToString(),
							velocity.ToString(), cubeTime.ToString(), nbError.ToString(),
							warping.ToString(),
							cubeDistGone.ToString(), handDistGone.ToString(),
							string.Join(";", acceleration),
							collisions[0].ToString(), collisions[1].ToString(), collisions[2].ToString(), collisions[3].ToString(),
							score.ToString(),
							pause.ToString()});
		writer.Flush();
		writer.Close();
	}

	public void writeDiscreteEntry(Trial trial, string timeCube, string StartTimeTrial, string EndTimeTrial,
								   int index,
								   Vector3 positionR, Vector3 orientationR,
								   Vector3 positionV, Vector3 orientationV,
								   Vector3 positionP, Vector3 orientationP,
								   float velocity, float cubeTime, int nbError,
								   float cubeDistGone, float handDistGone,
								   List<int> hitCount, int[] collisions,
								   float score) {
		writer = new StreamWriter(discretePath, true);

		write(new string[] {trial.parameters[0], trial.parameters[1], trial.parameters[2], trial.parameters[3], trial.parameters[4], index.ToString(),
							timeCube, StartTimeTrial, EndTimeTrial,
							positionR.x.ToString(), positionR.y.ToString(), positionR.z.ToString(),
							orientationR.x.ToString(), orientationR.y.ToString(), orientationR.z.ToString(),
							positionV.x.ToString(), positionV.y.ToString(), positionV.z.ToString(),
							orientationV.x.ToString(), orientationV.y.ToString(), orientationV.z.ToString(),
							positionP.x.ToString(), positionP.y.ToString(), positionP.z.ToString(),
							orientationP.x.ToString(), orientationP.y.ToString(), orientationP.z.ToString(),
							velocity.ToString(), cubeTime.ToString(), nbError.ToString(),
							cubeDistGone.ToString(), handDistGone.ToString(),
							string.Join(";", hitCount),
							collisions[0].ToString(), collisions[1].ToString(), collisions[2].ToString(), collisions[3].ToString(),
							score.ToString()});
		writer.Flush();
		writer.Close();
	}

	public void writeConfig(string time, string cubeName, int i, Vector3 positionR, Vector3 orientationR) {
		writer = new StreamWriter(sceneConfiguration, true);

		write(new string[] {time, cubeName, i.ToString(),
							positionR.x.ToString(), positionR.y.ToString(), positionR.z.ToString(),
							orientationR.x.ToString(), orientationR.y.ToString(), orientationR.z.ToString()});
		writer.Flush();
		writer.Close();
	}

	void write(string[] d) {
		for (int i=0; i<d.Length; i++) {
			writer.Write(d[i]);
			if (i!=d.Length-1) {
				writer.Write(";");
			}
		}
		writer.WriteLine("");
	}
}
