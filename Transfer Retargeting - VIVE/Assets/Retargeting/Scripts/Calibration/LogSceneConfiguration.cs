using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogSceneConfiguration : MonoBehaviour
{
	public bool saveConfig = false;

	CSVSaver csvSaver;

    GameObject[] grabbables;
    GameObject[] phantoms;
    GameObject world;

    void Start() {
        grabbables = GameObject.FindGameObjectsWithTag("Grabbable");
        phantoms = GameObject.FindGameObjectsWithTag("Phantom");
        world = GameObject.Find("World");

        csvSaver = GetComponent<CSVSaver>();
    }

    void Update() {
        if (saveConfig) {
        	foreach (GameObject g in grabbables) {
        		csvSaver.writeConfig(DateTime.Now.ToString("HH:mm:ss.fff"), g.name, g.transform.position, g.transform.eulerAngles);
        	}
        	foreach (GameObject p in phantoms) {
        		csvSaver.writeConfig(DateTime.Now.ToString("HH:mm:ss.fff"), p.name, p.transform.position, p.transform.eulerAngles);
        	}
        	csvSaver.writeConfig(DateTime.Now.ToString("HH:mm:ss.fff"), world.name, world.transform.position, world.transform.eulerAngles);
        	saveConfig = false;
        }
    }
}
