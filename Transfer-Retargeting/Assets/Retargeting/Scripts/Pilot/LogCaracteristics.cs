using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogCaracteristics : MonoBehaviour {

	public Transform[] realCube, virtualCube;

    void Start() {
    	StreamWriter writer = new StreamWriter("Data/caracteristics.csv", false);

        string[] data = new string[6];

        data = new string[]{"Index", "Warping", "X", "Y", "Z", "Total"};
        for (int j=0; j<data.Length; j++) {
			if (j!=data.Length-1)
				writer.Write(data[j] + ";");
			else 
				writer.WriteLine(data[j]);
		}

        for (int i=0; i<realCube.Length; i++) {
        	if (i==realCube.Length-1)
        		data = new string[]{i.ToString(), "0", ((realCube[0].position.x-realCube[i].position.x)*100).ToString("F1"), ((realCube[0].position.y-realCube[i].position.y)*100).ToString("F1"), ((realCube[0].position.z-realCube[i].position.z)*100).ToString("F1"), ((realCube[0].position-realCube[i].position).magnitude*100).ToString("F1")};
        	else
        		data = new string[]{i.ToString(), "0", ((realCube[i+1].position.x-realCube[i].position.x)*100).ToString("F1"), ((realCube[i+1].position.y-realCube[i].position.y)*100).ToString("F1"), ((realCube[i+1].position.z-realCube[i].position.z)*100).ToString("F1"), ((realCube[i+1].position-realCube[i].position).magnitude*100).ToString("F1")};
        	for (int j=0; j<data.Length; j++) {
				if (j!=data.Length-1)
					writer.Write(data[j] + "&");
				else 
					writer.WriteLine(data[j]);
			}

        	data = new string[]{i.ToString(), "1", ((realCube[i].position.x-virtualCube[i].position.x)*100).ToString("F1"),((realCube[i].position.y-virtualCube[i].position.y)*100).ToString("F1"), ((realCube[i].position.z-virtualCube[i].position.z)*100).ToString("F1"), ((realCube[i].position-virtualCube[i].position).magnitude*100).ToString("F1")};
        	for (int j=0; j<data.Length; j++) {
				if (j!=data.Length-1)
					writer.Write(data[j] + "&");
				else 
					writer.WriteLine(data[j]);
			}
        }
        writer.Flush();
		writer.Close();
    }

    // Update is called once per frame
    void Update() {
        
    }
}
