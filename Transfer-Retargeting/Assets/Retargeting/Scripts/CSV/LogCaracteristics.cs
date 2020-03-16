using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogCaracteristics : MonoBehaviour {

	public Transform[] realCube, virtualCube;

    void Start() {
        StreamWriter writer = new StreamWriter("Data/caracteristics.csv", false);
        StreamWriter writerOverleaf = new StreamWriter("Data/caracteristicsOverleaf.csv", false);

        string[] data = new string[6];
        string[] dataOverleaf = new string[4];

        data = new string[]{"Index", "Warping", "X", "Y", "Z", "Total"};
        for (int j=0; j<data.Length; j++) {
			if (j!=data.Length-1)
				writer.Write(data[j] + ";");
			else 
				writer.WriteLine(data[j]);
		}

        for (int i=0; i<realCube.Length; i++) {
            if (i==0)
                data = new string[]{i.ToString(), "0", ((realCube[i].position.x-realCube[5].position.x)*100).ToString("F1"), ((realCube[i].position.y-realCube[5].position.y)*100).ToString("F1"), ((realCube[i].position.z-realCube[5].position.z)*100).ToString("F1"), ((realCube[i].position-realCube[5].position).magnitude*100).ToString("F1")};
            else
                data = new string[]{i.ToString(), "0", ((realCube[i].position.x-realCube[i-1].position.x)*100).ToString("F1"), ((realCube[i].position.y-realCube[i-1].position.y)*100).ToString("F1"), ((realCube[i].position.z-realCube[i-1].position.z)*100).ToString("F1"), ((realCube[i].position-realCube[i-1].position).magnitude*100).ToString("F1")};
            
            if (i==0)
                dataOverleaf = new string[]{((realCube[i].position.x-realCube[5].position.x)*100).ToString("F1"), ((realCube[i].position.y-realCube[5].position.y)*100).ToString("F1"), ((realCube[i].position.z-realCube[5].position.z)*100).ToString("F1"), ((realCube[i].position-realCube[5].position).magnitude*100).ToString("F1")};
            else
                dataOverleaf = new string[]{((realCube[i].position.x-realCube[i-1].position.x)*100).ToString("F1"), ((realCube[i].position.y-realCube[i-1].position.y)*100).ToString("F1"), ((realCube[i].position.z-realCube[i-1].position.z)*100).ToString("F1"), ((realCube[i].position-realCube[i-1].position).magnitude*100).ToString("F1")};
            
            for (int j=0; j<data.Length; j++) {
                if (j!=data.Length-1)
                    writer.Write(data[j] + "&");
                else 
                    writer.WriteLine(data[j]);
            }

            writerOverleaf.Write("\\multirow{2}{*}{1} & Movement  & ");
            for (int j=0; j<dataOverleaf.Length-1; j++) {
                writerOverleaf.Write(dataOverleaf[j] + "&");
            }
            writerOverleaf.WriteLine(dataOverleaf[dataOverleaf.Length-1] + "\\\\");

        	data = new string[]{((realCube[i].position.x-virtualCube[i].position.x)*100).ToString("F1"),((realCube[i].position.y-virtualCube[i].position.y)*100).ToString("F1"), ((realCube[i].position.z-virtualCube[i].position.z)*100).ToString("F1"), ((realCube[i].position-virtualCube[i].position).magnitude*100).ToString("F1")};
        	for (int j=0; j<data.Length; j++) {
				if (j!=data.Length-1)
					writer.Write(data[j] + "&");
				else 
					writer.WriteLine(data[j]);
			}

            dataOverleaf = new string[]{((realCube[i].position.x-virtualCube[i].position.x)*100).ToString("F1"),((realCube[i].position.y-virtualCube[i].position.y)*100).ToString("F1"), ((realCube[i].position.z-virtualCube[i].position.z)*100).ToString("F1"), ((realCube[i].position-virtualCube[i].position).magnitude*100).ToString("F1")};
            writerOverleaf.Write(" & Warping & ");
            for (int j=0; j<dataOverleaf.Length-1; j++) {
                writerOverleaf.Write(dataOverleaf[j] + "&");
            }
            writerOverleaf.WriteLine(dataOverleaf[dataOverleaf.Length-1] + "\\\\");
            writerOverleaf.WriteLine("\\hline");
        }
        writer.Flush();
        writer.Close();
        writerOverleaf.Flush();
        writerOverleaf.Close();
    }

    // Update is called once per frame
    void Update() {
        
    }
}
