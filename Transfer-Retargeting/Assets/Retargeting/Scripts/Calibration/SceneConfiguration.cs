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
    GameObject[] warpedCubes;
    GameObject world, button;

    public Vector3[] phantomPos;

    void Start() {
        grabbables = GameObject.FindGameObjectsWithTag("Grabbable");
        phantoms = GameObject.FindGameObjectsWithTag("Phantom");
        warpedCubes = GameObject.FindGameObjectsWithTag("WarpedCubes");
        world = GameObject.Find("World");
        button = GameObject.Find("Button");

        phantomPos = new Vector3[grabbables.Length];

        csvSaver = GetComponent<CSVSaver>();
    }

    void Update() {
        if (saveConfig || Input.GetKeyDown(KeyCode.Space)) {
            int i=0;
        	foreach (GameObject g in grabbables) {
        		csvSaver.writeConfig(DateTime.Now.ToString("HH:mm:ss.fff"), g.name, i++, g.transform.position, g.transform.eulerAngles);
            }
            i=0;
        	foreach (GameObject p in phantoms) {
        		csvSaver.writeConfig(DateTime.Now.ToString("HH:mm:ss.fff"), p.name, i++, p.transform.position, p.transform.eulerAngles);
            }
            i=0;
            foreach (GameObject c in warpedCubes) {
                csvSaver.writeConfig(DateTime.Now.ToString("HH:mm:ss.fff"), c.name + " " + i++, i, c.transform.position, c.transform.eulerAngles);
            }

            csvSaver.writeConfig(DateTime.Now.ToString("HH:mm:ss.fff"), world.name, 0, world.transform.position, world.transform.eulerAngles);
            csvSaver.writeConfig(DateTime.Now.ToString("HH:mm:ss.fff"), button.name, 0, button.transform.position, button.transform.eulerAngles);

            if (Input.GetKeyDown(KeyCode.Space)) {
                csvSaver.writeConfig("Good data", " ", 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f));
            }
        	
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
