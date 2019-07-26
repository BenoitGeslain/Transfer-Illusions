using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneReset : MonoBehaviour
{

    GameObject[] grabbables;
    GameObject[] phantoms;
    GameObject[] clones;


    Vector3[] grabbablesPosition;
    Vector3[] phantomPosition;

    // Start is called before the first frame update
    void Start()
    {
		grabbables = GameObject.FindGameObjectsWithTag("Grabbable");
	    phantoms = GameObject.FindGameObjectsWithTag("Phantom");

	    for(int i=0; i<grabbables.Length; i++) {
	    	grabbablesPosition[i] = grabbables[i].transform.position;
	    }
	    for(int i=0; i<phantoms.Length; i++) {
	    	phantomPosition[i] = phantoms[i].transform.position;
	    }
    }

    void resetScene() {
    	// réinitialise la position de tous les objects dans la scène
    	for(int i=0; i<grabbables.Length; i++) {
	    	grabbables[i].transform.position = grabbablesPosition[i];
	    }
	    for(int i=0; i<phantoms.Length; i++) {
	    	phantoms[i].transform.position = phantomPosition[i];
	    }

	    // Détruit tous les clones du cube tracké
	    Transform clonesPrent = GameObject.Find("/World/Clones").transform;
	    foreach(Transform clone in transform) {
	    	Destroy(clone.gameObject);
	    }
    }
}
