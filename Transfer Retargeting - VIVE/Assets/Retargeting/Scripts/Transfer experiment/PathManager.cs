using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    GameObject[] grabbables;
    GameObject[] phantoms;
    Transform phantomIntro;
    public int i = -1;

    void Start() {
        grabbables = GameObject.FindGameObjectsWithTag("Grabbable");
        phantoms = GameObject.FindGameObjectsWithTag("Phantom");
        phantomIntro = GameObject.Find("Cube Phantom Intro").transform;
    }

    void OnDrawGizmos() {
    	switch (i) {
    		case 0:
    			Gizmos.DrawLine(phantoms[0].transform.position,
    							new Vector3(phantoms[0].transform.position.x, phantoms[0].transform.position.y + 0.065f, phantoms[0].transform.position.z));

    			Gizmos.DrawLine(new Vector3(phantoms[0].transform.position.x, phantoms[0].transform.position.y + 0.065f, phantoms[0].transform.position.z),
    							new Vector3(phantomIntro.position.x, phantoms[0].transform.position.y + 0.065f, phantomIntro.position.z));

    			Gizmos.DrawLine(new Vector3(phantomIntro.position.x, phantoms[0].transform.position.y + 0.065f, phantomIntro.position.z),
    							phantomIntro.position);
    			break;
    		case 1:
    			Gizmos.DrawLine(grabbables[0].transform.position,
    							new Vector3(grabbables[0].transform.position.x, grabbables[0].transform.position.y + 0.271f, grabbables[0].transform.position.z));

    			Gizmos.DrawLine(new Vector3(grabbables[0].transform.position.x, grabbables[0].transform.position.y + 0.271f, grabbables[0].transform.position.z),
    							new Vector3(grabbables[0].transform.position.x, grabbables[0].transform.position.y + 0.271f, phantoms[1].transform.position.z));

    			Gizmos.DrawLine(new Vector3(grabbables[0].transform.position.x, grabbables[0].transform.position.y + 0.271f, phantoms[1].transform.position.z),
    							new Vector3(phantoms[1].transform.position.x, grabbables[0].transform.position.y + 0.271f, phantoms[1].transform.position.z));

    			Gizmos.DrawLine(new Vector3(phantoms[1].transform.position.x, grabbables[0].transform.position.y + 0.271f, phantoms[1].transform.position.z),
    							phantoms[1].transform.position);
    			break;
    		case 2:
    			Gizmos.DrawLine(grabbables[1].transform.position,
    							new Vector3(grabbables[1].transform.position.x, grabbables[1].transform.position.y + 0.271f, grabbables[1].transform.position.z));
    			
    			Gizmos.DrawLine(new Vector3(grabbables[1].transform.position.x, grabbables[1].transform.position.y + 0.271f, grabbables[1].transform.position.z),
    							new Vector3(phantoms[2].transform.position.x, grabbables[1].transform.position.y + 0.271f, grabbables[1].transform.position.z));
    			
    			Gizmos.DrawLine(new Vector3(phantoms[2].transform.position.x, grabbables[1].transform.position.y + 0.271f, grabbables[1].transform.position.z),
    							phantoms[2].transform.position);
    			break;
    		case 3:
    			Gizmos.DrawLine(grabbables[2].transform.position,
    							new Vector3(grabbables[2].transform.position.x + 0.12f, grabbables[2].transform.position.y, grabbables[2].transform.position.z));

    			Gizmos.DrawLine(new Vector3(grabbables[2].transform.position.x + 0.12f, grabbables[2].transform.position.y, grabbables[2].transform.position.z),
    							new Vector3(grabbables[2].transform.position.x + 0.12f, grabbables[2].transform.position.y - 0.05f, grabbables[2].transform.position.z - 0.2f));

    			Gizmos.DrawLine(new Vector3(grabbables[2].transform.position.x + 0.12f, grabbables[2].transform.position.y - 0.05f, grabbables[2].transform.position.z - 0.2f),
    							new Vector3(phantoms[3].transform.position.x, grabbables[2].transform.position.y - 0.05f, grabbables[2].transform.position.z - 0.2f));

    			Gizmos.DrawLine(new Vector3(phantoms[3].transform.position.x, grabbables[2].transform.position.y - 0.05f, grabbables[2].transform.position.z - 0.2f),
    							new Vector3(phantoms[3].transform.position.x, phantoms[3].transform.position.y, grabbables[2].transform.position.z - 0.2f));

    			Gizmos.DrawLine(new Vector3(phantoms[3].transform.position.x, phantoms[3].transform.position.y, grabbables[2].transform.position.z - 0.2f),
    							phantoms[3].transform.position);
    			break;
    		default:
    			break;
    	};
    }

    public void ShowPath(int id) {
    	i = id;
    }

    public void HidePath() {
    	i = -1;
    }
}
