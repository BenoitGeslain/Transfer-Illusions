using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneConfiguration : MonoBehaviour
{
	public bool saveConfig = false;

	CSVSaver csvSaver;

    GameObject[] grabbables;
    GameObject[] phantoms;
    GameObject world, button;

    public Vector3[] phantomPos;

    void Start() {
        grabbables = GameObject.FindGameObjectsWithTag("Grabbable");
        phantoms = GameObject.FindGameObjectsWithTag("Phantom");
        world = GameObject.Find("World");
        button = GameObject.Find("Button");

        phantomPos = new Vector3[grabbables.Length];

        csvSaver = GetComponent<CSVSaver>();
    }

    void Update() {
        if (saveConfig || Input.GetKeyDown(KeyCode.Space)) {
        	foreach (GameObject g in grabbables) {
        		csvSaver.writeConfig(DateTime.Now.ToString("HH:mm:ss.fff"), g.name, g.transform.position, g.transform.eulerAngles);
            }
        	foreach (GameObject p in phantoms) {
        		csvSaver.writeConfig(DateTime.Now.ToString("HH:mm:ss.fff"), p.name, p.transform.position, p.transform.eulerAngles);
            }

            for (int i=0; i<phantoms.Length; i++) {
                phantomPos[i] = phantoms[i].transform.position;
            }

            csvSaver.writeConfig(DateTime.Now.ToString("HH:mm:ss.fff"), world.name, world.transform.position, world.transform.eulerAngles);
            csvSaver.writeConfig(DateTime.Now.ToString("HH:mm:ss.fff"), button.name, button.transform.position, button.transform.eulerAngles);
        	
            saveConfig = false;
        }
    }

    public void setPhantomsOnGrabbables() {
        /*for (int i=0; i<phantoms.Length; i++) {
            phantomPos[i] = phantoms[i].transform.position;
        }

        for (int i=0; i<grabbables.Length; i++) {
            phantoms[i].transform.position = grabbables[i].transform.position;
        }*/
    }

    public void resetPhantoms() {
        /*for (int i=0; i<grabbables.Length; i++) {
            phantoms[i].transform.position = phantomPos[i];
        }*/
    }
}
